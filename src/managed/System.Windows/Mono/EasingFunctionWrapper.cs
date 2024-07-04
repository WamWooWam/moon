
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;

namespace Mono
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate double EasingFunctionCallback(IntPtr easingFunctionBase, double normalizedTime);

    internal sealed class EasingFunctionWrapper : EasingFunctionBase
    {
        IEasingFunction function;

        public EasingFunctionWrapper(IEasingFunction function)
        {
            this.function = function;
            this.EasingMode = EasingMode.EaseIn;
        }

        protected override double EaseInCore(double normalizedTime)
        {
            return function.Ease(normalizedTime);
        }
    }
}
