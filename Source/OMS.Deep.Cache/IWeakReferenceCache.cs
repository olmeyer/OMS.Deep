using System;
using System.Collections.Generic;

namespace OMS.Deep.Cache
{
    /// <summary>
    ///     The weak reference cache holds key value pairs whereby the value is hold as a weak reference. If there is no more
    ///     outstanding reference to a cache entry, it will be automatically removed from the cache.
    /// </summary>
    /// <typeparam name="TKey"> The type of the key. </typeparam>
    /// <typeparam name="TValue"> The type of the value. </typeparam>
    public interface IWeakReferenceCache<TKey, TValue> : ICache<TKey, TValue>
    {
        /// <summary>
        ///     Returns true if autocollect is enabled. Set the value to false, to disable the auto collect. In this case Collect()
        ///     must be called to remobe invalid cache entries.
        /// </summary>
        bool AutoCollect { get; set; }


        bool AutoDisposeKey { get; set; }

        /// <summary>
        ///     Removes all invalid cache entries.
        /// </summary>
        void Collect();

        event EventHandler<ObjectRemovedEventArgs<IEnumerable<TKey>>> ObjectRemoved;

   }

    public class ObjectRemovedEventArgs<TKey> : EventArgs
    {
        public ObjectRemovedEventArgs(TKey key)
        {
            Key = key;
        }

        public TKey Key { get; private set; }
    }
}

