namespace OMS.Deep.Cache
{
    /// <summary>
    ///     Each time Increment() is called, the lifetime of the cached object will be increased. If Decrement() is called,
    ///     the lifetime will be decreased. If the lifetime goes to 0, the internel reference to the object will be set to
    ///     null.
    ///     If the object is not referenced by the application anymore, the garbage collector can destroy the object.
    /// </summary>
    /// <typeparam name="TValue"> </typeparam>
    public class CachedCounterReference<TValue> : CachedReference<TValue> where TValue : class
    {
        private CounterValues _counterValues = new CounterValues();
        private TValue _target;


        internal CachedCounterReference( TValue target )
            : base( target )
        {
        }


        internal CachedCounterReference( TValue target, CounterValues counterValues )
            : base( target )
        {
            _target = target;
            _counterValues = counterValues;
        }


        internal uint Count { get; private set; }


        internal CounterValues CounterValues
        {
            get { return _counterValues; }
            set { _counterValues = value ?? new CounterValues(); }
        }


        internal uint Increment()
        {
            // m_count can be more than Max.
            // The maximal m_count is Max+Increment-1.
            // I could do more sophisticated test but it would use more time without
            // great improvement.
            if( Count < _counterValues.Max )
            {
                Count += _counterValues.Increment;

                // Set a hard reference to the target as long as the counter is not 0.
                // Just get the object from the target property. It will be alive. No check is needed.
                if( _target == null )
                    _target = Target;
            }

            return Count;
        }


        internal uint Decrement()
        {
            if( Count > _counterValues.Decrement )
            {
                Count -= _counterValues.Decrement;
            }
            else if( Count >= 0 )
            {
                Count = 0;

                // Release the target field. Thus the GC can collect the object.
                // However, it is still available as a weak reference as long as the
                // GC didn't collect it.
                _target = null;
            }

            return Count;
        }
    }
}