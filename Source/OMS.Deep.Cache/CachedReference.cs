#region

using System;

#endregion


namespace OMS.Deep.Cache
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TValue"> </typeparam>
    public class CachedReference<TValue> : WeakReference where TValue : class
    {
        internal CachedReference( TValue target )
            : base( target )
        {
        }


        internal new TValue Target
        {
            get => (TValue)base.Target;
            set => base.Target = value;
        }
    }
}