#region

using System.Collections.Generic;

#endregion


namespace OMS.Deep.Cache
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TKey"> </typeparam>
    /// <typeparam name="TValue"> </typeparam>
    public class DynamicCache<TKey, TValue>
        : WeakReferenceCacheBase<TKey, TValue, CachedCounterReference<TValue>>
            , IDynamicCache<TKey, TValue>
        where TValue : class
    {
        private readonly CounterValues _counterValues = new CounterValues();


        /// <summary>
        /// </summary>
        /// <param name="id"> </param>
        public DynamicCache( string id )
            : base( id )
        {
        }


        /// <summary>
        /// </summary>
        /// <param name="period"> </param>
        /// <param name="id"> </param>
        public DynamicCache( uint period, string id )
            : base( period, id )
        {
        }


        #region IDynamicCache<TKey,TValue> Members

        void ICache<TKey, TValue>.Add( TKey key, TValue value )
        {
            (this as IDynamicCache<TKey, TValue>).Add( key, value, _counterValues );
        }


        bool ICache<TKey, TValue>.TryGetValue( TKey key, out TValue value )
        {
            return TryGetValue( key, out value );
        }


        void ICache<TKey, TValue>.Remove( TKey key )
        {
            Remove( key );
        }


        bool ICache<TKey, TValue>.Contains( TKey key )
        {
            return Contains( key );
        }

        IEnumerable<TKey> ICache<TKey, TValue>.FindKeysFromValue( TValue value )
        {
            return FindKeysFromValue( value );
        }


        object ICache<TKey, TValue>.SyncRoot
        {
            get { return SyncRoot; }
        }


        void ICache<TKey, TValue>.Clear()
        {
            Clear();
        }


        void IDynamicCache<TKey, TValue>.Add( TKey key, TValue value, CounterValues counterValues )
        {
            // Add the new object.
            var reference = Add( key, value );

            // Set the counter values and increment the count.
            reference.CounterValues = counterValues;
            reference.Increment();
        }


        void IDynamicCache<TKey, TValue>.Collect()
        {
            Collect();
        }


        CounterValues IDynamicCache<TKey, TValue>.CounterValues
        {
            get { return _counterValues; }
        }

        #endregion


        protected override bool OnTestReference( CachedCounterReference<TValue> reference )
        {
            return (reference.Decrement() > 0);
        }


        protected override CachedCounterReference<TValue> CreateCachedReference( TValue target )
        {
            return new CachedCounterReference<TValue>( target, _counterValues );
        }


        protected override void OnGetReference( CachedCounterReference<TValue> reference )
        {
            reference.Increment();
        }
    }
}