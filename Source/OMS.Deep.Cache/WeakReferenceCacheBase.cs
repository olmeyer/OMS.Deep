#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

#endregion


#pragma warning disable 1591

namespace OMS.Deep.Cache
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TKey"> </typeparam>
    /// <typeparam name="TValue"> </typeparam>
    /// <typeparam name="TReference"> </typeparam>
    public abstract class WeakReferenceCacheBase<TKey, TValue, TReference> : CacheBase, IDisposable
        where TValue : class
        where TReference : CachedReference<TValue>
    {
        private const uint DefaultPeriod = 60000;
        private bool _autoCollect;
        private IDictionary<TKey, TReference> _cache;
        private Timer _collectionTimer;
        private uint _period = DefaultPeriod;

        protected WeakReferenceCacheBase( string id )
            : base( id )
        {
            Initialize( true, DefaultPeriod );
        }

        protected WeakReferenceCacheBase( bool autoCollect, string id )
            : base( id )
        {
            Initialize( autoCollect, DefaultPeriod );
        }

        protected WeakReferenceCacheBase( uint period, string id )
            : base( id )
        {
            Initialize( true, period );
        }

        protected bool AutoCollect
        {
            get => _autoCollect;
            set
            {
                lock( SyncRoot )
                {
                    if( _autoCollect != value && _cache != null )
                    {
                        _autoCollect = value;
                        EnableTimer( _autoCollect );
                    }
                }
            }
        }

        protected bool AutoDisposeKey { get; set; }


        #region IDisposable Members

        void IDisposable.Dispose()
        {
            lock( SyncRoot )
            {
                if( _cache != null )
                {
                    AutoCollect = false;
                    Clear();
                    _cache = null;
                }
            }
        }

        #endregion


        protected IEnumerable<TKey> FindKeysFromValue( TValue value )
        {
            IList<TKey> keys;
            lock( SyncRoot )
            {
                keys = new List<TKey>( _cache.Keys );
            }

            var result = new List<TKey>();
            foreach( var key in keys )
            {
                if( !TryGetValue( key, out var itemValue ) )
                    continue;

                if( ReferenceEquals( itemValue, value ) || itemValue.Equals( value ) )
                    result.Add( key );
            }

            return result;
        }

        protected TReference Add( TKey key, TValue value )
        {
#if DEBUG
            if( key.Equals( value ) )
                throw new Exception( "The value must not be used as key" );
#endif

            lock( SyncRoot )
            {
                var reference = CreateCachedReference( value );
                _cache.Add( key, reference );

                return reference;
            }
        }

        protected bool TryGetValue( TKey key, out TValue value )
        {
            var found = false;
            value = null;
            lock( SyncRoot )
            {
                if( _cache.TryGetValue( key, out var reference ) )
                {
                    // Assign the reference to a lokal object before calling IsAlive().
                    // Otherwise the garbage collector could dispose the object between
                    // the calls to IsAlive and Target.
                    var localValue = reference.Target;
                    if( reference.IsAlive )
                    {
                        found = true;
                        OnGetReference( reference );
                        value = localValue;
                    }
                    else
                    {
                        // Object is not valid. Removed it from the dictionary.
                        Remove( key );
                    }
                }

                return found;
            }
        }

        protected void Remove( TKey key )
        {
            lock( SyncRoot )
            {
                _cache.Remove( key );
                if( AutoDisposeKey )
                {
                    if( key is IDisposable disposable )
                        disposable.Dispose();
                }
            }
        }

        protected void Clear()
        {
            List<TKey> keys = null;
            lock( SyncRoot )
            {
                if( _cache != null )
                {
                    if( AutoDisposeKey )
                        keys = new List<TKey>( _cache.Keys );

                    _cache.Clear();
                }
            }

            if( keys != null )
            {
                foreach( var disposableKey in keys.OfType<IDisposable>() )
                    disposableKey.Dispose();
            }
        }

        protected bool Contains( TKey key )
        {
            lock( SyncRoot )
            {
                return _cache.ContainsKey( key );
            }
        }

        protected void Collect()
        {
            ThreadPool.QueueUserWorkItem( Collect );
        }

        private void Initialize( bool autoCollect, uint period )
        {
            _period = period;
            _cache = new Dictionary<TKey, TReference>();
            AutoCollect = autoCollect;
        }

        private void EnableTimer( bool enable )
        {
            if( enable )
            {
                _collectionTimer = new Timer( Collect, null, 0
                                             , (_period > 0) ? (int)_period : (int)DefaultPeriod );
            }
            else
            {
                if( _collectionTimer != null )
                {
                    _collectionTimer.Dispose();
                    _collectionTimer = null;
                }
            }
        }

        private void Collect( object stateInfo )
        {
#if FORCE_COLLECT && DEBUG
// TODO: Make this configurable. (Is that really needed?)
			GC.Collect(3, GCCollectionMode.Forced);
#endif
            // FORCE_COLLECT && DEBUG

            Dictionary<TKey, TReference> pairs;
            lock( SyncRoot )
            {
                // Get all keys at once since we cannot iterate over the dictionaries key collection.
                if( _cache == null )
                    return;
                pairs = new Dictionary<TKey, TReference>( _cache );
            }

            // Test each item in the cache. However, the cache must be still accessible by the clients.
            // Therefore use a single lock for each test to avoid long lasting locking.
            foreach( var pair in pairs )
            {
                lock( SyncRoot )
                {
                    // The cache might have been destroyed in the meantime.
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable HeuristicUnreachableCode
// Resharper doesn't know anything about multi-threading. _cache can be set to null in another thread.
                    if( _cache == null )
                        return;
// ReSharper restore HeuristicUnreachableCode
// ReSharper restore ConditionIsAlwaysTrueOrFalse

                    var reference = pair.Value;
                    if( !OnTestReference( reference ) && !reference.IsAlive )
                        Remove( pair.Key );
                }
            }
        }

        protected abstract bool OnTestReference( TReference reference );

        protected abstract TReference CreateCachedReference( TValue target );

        protected abstract void OnGetReference( TReference refernce );
    }
}

#pragma warning restore 1591