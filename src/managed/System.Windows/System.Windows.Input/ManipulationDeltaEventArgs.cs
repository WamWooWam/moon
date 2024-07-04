// Contact:
//   Moonlight List (moonlight-list@lists.ximian.com)
//
// Copyright 2010 Novell, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System.ComponentModel;

namespace System.Windows.Input
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class ManipulationDeltaEventArgs : RoutedEventArgs
    {

        [MonoTODO("design-mode time not supported")]
        public ManipulationDeltaEventArgs()
        {
            if (!DesignerProperties.GetIsInDesignMode(Application.Current.RootVisual))
                throw new NotImplementedException();
        }

        public void Complete()
        {
            Console.WriteLine("NIEX: System.Windows.Input.ManipulationDeltaEventArgs:Complete");
            throw new NotImplementedException();
        }

        public void StartInertia()
        {
            Console.WriteLine("NIEX: System.Windows.Input.ManipulationDeltaEventArgs:StartInertia");
            throw new NotImplementedException();
        }

        public ManipulationDelta CumulativeManipulation { get; private set; }
        public ManipulationDelta DeltaManipulation { get; private set; }
        public bool Handled { get; set; }
        public bool IsInertial { get; private set; }
        public UIElement ManipulationContainer { get; private set; }
        public Point ManipulationOrigin { get; private set; }
        public ManipulationVelocities Velocities { get; private set; }
    }
}

