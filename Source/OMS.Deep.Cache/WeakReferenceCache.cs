#region

using System;


#region

using System.Collections.Generic;
using System.Diagnostics;

#endregion

#endregion


#pragma warning disable 1591

namespace OMS.Deep.Cache
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TKey"> </typeparam>
    /// <typeparam name="TValue"> </typeparam>
    public class WeakReferenceCache<TKey, TValue>
        : WeakReferenceCacheBase<TKey, TValue, CachedReference<TValue>>
            , IWeakReferenceCache<TKey, TValue>
        where TValue : class
    {
        /// <summary>
        /// </summary>
        /// <param name="id"> </param>
        public WeakReferenceCache( string id )
            : base( id )
        {
        }


        /// <summary>
        /// </summary>
        /// <param name="autoCollect"> </param>
        /// <param name="id"> </param>
        public WeakReferenceCache( bool autoCollect, string id )
            : base( autoCollect, id )
        {
        }


        /// <summary>
        /// </summary>
        /// <param name="period"> </param>
        /// <param name="id"> </param>
        public WeakReferenceCache( uint period, string id )
            : base( period, id )
        {
        }


        protected override bool OnTestReference( CachedReference<TValue> reference )
        {
            return false;
        }


        protected override CachedReference<TValue> CreateCachedReference( TValue target )
        {
            return new CachedReference<TValue>( target );
        }


        protected override void OnGetReference( CachedReference<TValue> refernce )
        {
            // Just do nothing.
        }


        #region IWeakReferenceCache<TKey,TValue> Members

        void IWeakReferenceCache<TKey, TValue>.Collect()
        {
            Collect();
        }


        bool IWeakReferenceCache<TKey, TValue>.AutoCollect
        {
            get => AutoCollect;
            set => AutoCollect = value;
        }


        bool IWeakReferenceCache<TKey, TValue>.AutoDisposeKey
        {
            get => AutoDisposeKey;
            set => AutoDisposeKey = value;
        }


        void ICache<TKey, TValue>.Add( TKey key, TValue value )
        {
            Debug.Assert( value != null, "Value must not be null." );
            Add( key, value );
        }


        bool ICache<TKey, TValue>.TryGetValue( TKey key, out TValue value )
        {
            return TryGetValue( key, out value );
        }


        void ICache<TKey, TValue>.Remove( TKey key )
        {
            Remove( key );
        }


        void ICache<TKey, TValue>.Clear()
        {
            Clear();
        }


        bool ICache<TKey, TValue>.Contains( TKey key )
        {
            return Contains( key );
        }

        IEnumerable<TKey> ICache<TKey, TValue>.FindKeysFromValue( TValue value )
        {
            return FindKeysFromValue( value );
        }

        object ICache<TKey, TValue>.SyncRoot => SyncRoot;

        #endregion
    }
}

#pragma warning restore 1591