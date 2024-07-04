using System;

namespace Mono
{

    abstract class WeakEventListener : IWeakListener
    {

        WeakReference listener;
        protected IListenEventRaised Listener
        {
            get => (IListenEventRaised)listener.Target;
            set => listener = new WeakReference(value);
        }

        public WeakEventListener(IListenEventRaised listener)
        {
            Listener = listener;
        }

        public abstract void Detach();
    }
}

