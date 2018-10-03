namespace OMS.Deep.Cache
{
    /// <summary>
    ///     The dynamic cache holds key value pairs whereby the value is hold as a weak reference. If there is no more
    ///     outstanding reference to a cache entry, it will be automatically removed from the cache. Further, the cache checks,
    ///     how frequently an object is requested. Frequently used objects will get a high counter value and therefore will not be removed immediately
    ///     from the cache if there are no references to this object.
    /// </summary>
    /// <typeparam name="TKey"> The key type. </typeparam>
    /// <typeparam name="TValue"> The value type. </typeparam>
    public interface IDynamicCache<TKey, TValue> : ICache<TKey, TValue>
    {
        /// <summary>
        ///     Gets the default counter values. Any change of counter valus in the returned object will directly affect the
        ///     caching behaviour.
        /// </summary>
        CounterValues CounterValues { get; }


        /// <summary>
        ///     Adds a new key value pair to the cache. Use the ICache.AddValue(TKey key, V value) if you would like to apply
        ///     default behaviour for the new entry.
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <param name="value"> The value. </param>
        /// <param name="counterValues"> The counter values. </param>
        void Add( TKey key, TValue value, CounterValues counterValues );


        /// <summary>
        ///     Removes all invalid cache entries. Collect() also decreases the usage count of the cached items and removes seldom
        ///     used objects from the cache.
        /// </summary>
        void Collect();
    }
}