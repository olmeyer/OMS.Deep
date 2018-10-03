namespace OMS.Deep.Cache
{
    /// <summary>
    ///     Base class for all caches.
    /// </summary>
    public class CacheBase
    {
        private readonly static LockObject SGlobalLockObject = new LockObject();
        private static int s_idCount;

#if CALL_GC
        private static Timer s_gcTimer;
        static CacheBase()
        {
            sm_gcTimer = new Timer(new TimerCallback(CallGC), null, 0, 60000);
        }

		private static void CallGC(object stateInfo)
        {
            System.GC.Collect();
        }
#endif

        internal static LockObject GlobalLockObject
        {
            get { return SGlobalLockObject; }
        }

        internal LockObject SyncRoot { get; private set; }

        private readonly string _id;
        private readonly LockObject _mLockObject = new LockObject();


        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="id"> The identifier of the cache. </param>
        public CacheBase( string id )
        {
            SyncRoot = new LockObject();

            lock( GlobalLockObject )
            {
                if( id == null )
                    id = string.Empty;

                _id = id + (++s_idCount);
            }
        }


        internal LockObject LockObject
        {
            get { return _mLockObject; }
        }


        /// <summary>
        ///     Gets the identifier of the cache.
        /// </summary>
        public string Id
        {
            get { return _id; }
        }
    }
}