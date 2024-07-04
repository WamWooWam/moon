using System;
using System.Windows;

namespace Mono
{

    class DependencyObjectHandle
    {
        IntPtr handle;

        public IntPtr Handle
        {
            get => handle;
            private set => handle = value;
        }

        public INativeEventObjectWrapper Object
        {
            get; private set;
        }

        public DependencyObjectHandle(IntPtr handle, INativeEventObjectWrapper wrapper)
        {
            Object = wrapper;
            Handle = handle;
        }

        ~DependencyObjectHandle()
        {
            NativeDependencyObjectHelper.FreeNativeMapping(Handle);
            Handle = IntPtr.Zero;
        }
    }
}

