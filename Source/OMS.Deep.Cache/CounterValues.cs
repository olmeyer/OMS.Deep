namespace OMS.Deep.Cache
{
    /// <summary>
    /// </summary>
    public class CounterValues
    {
        /// <summary>
        /// </summary>
        public static uint DefaultDec = 1;


        /// <summary>
        /// </summary>
        public static uint DefaultInc = 3;


        /// <summary>
        /// </summary>
        public static uint DefaultMax = 20;


        /// <summary>
        /// </summary>
        public uint Decrement = DefaultDec;


        /// <summary>
        /// </summary>
        public uint Increment = DefaultInc;


        /// <summary>
        /// </summary>
        public uint Max = DefaultMax;


        /// <summary>
        /// </summary>
        public CounterValues()
        {
        }


        /// <summary>
        /// </summary>
        /// <param name="increment"> </param>
        /// <param name="decrement"> </param>
        /// <param name="max"> </param>
        public CounterValues( uint increment, uint decrement, uint max )
        {
            Increment = (increment != 0) ? increment : DefaultInc;
            Decrement = (decrement != 0) ? decrement : DefaultDec;
            Max = (max != 0) ? max : DefaultMax;
        }
    }
}