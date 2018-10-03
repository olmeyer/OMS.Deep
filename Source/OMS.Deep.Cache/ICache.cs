#region

using System;
using System.Collections.Generic;

#endregion


namespace OMS.Deep.Cache
{
    /// <summary>
    ///     Common cache interface.
    /// </summary>
    /// <typeparam name="TKey"> The key type. </typeparam>
    /// <typeparam name="TValue"> The value type. </typeparam>
    public interface ICache<TKey, TValue> : IDisposable
    {
        /// <summary>
        ///     Return a synchronization object. This can be used to synchronize calls to the cache.
        /// </summary>
        /// <remarks>
        ///     The cache implementation is thread safe. Thus, there might only be few cases in which external cache
        ///     synchronization is needed.
        ///     Using external cache synchronization may result in performance issues.
        /// </remarks>
        object SyncRoot { get; }

        /// <summary>
        ///     Adds a new key value pair to the cache.
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <param name="value"> The value. </param>
        void Add( TKey key, TValue value );


        /// <summary>
        ///     Tries to get the value identified by the key.
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <param name="value"> The value. </param>
        /// <returns> </returns>
        bool TryGetValue( TKey key, out TValue value );


        /// <summary>
        ///     Removes the key value pair from the cache.
        /// </summary>
        /// <param name="key"> </param>
        void Remove( TKey key );


        /// <summary>
        ///     Removes all values from the cache.
        /// </summary>
        void Clear();


        /// <summary>
        ///     Tests whether the cache contains an object with the given key.
        /// </summary>
        /// <param name="key"> The key to search for. </param>
        /// <returns> Returns true if the key has been found, otherwise false. </returns>
        bool Contains( TKey key );


        /// <summary>
        ///     Return a set of keys which associated values are equal to a search value.
        /// </summary>
        /// <param name="value">The value to search for.</param>
        /// <returns>An enumerable of keys.</returns>
        /// <remarks>
        ///     There is no guarantee that a key from the result set is still in the
        ///     cache when trying to read the associated value. TryGetValue may return null.
        /// </remarks>
        IEnumerable<TKey> FindKeysFromValue( TValue value );
    }
}