//
// Unit tests for ManipulationCompletedEventArgs
//
// Contact:
//   Moonlight List (moonlight-list@lists.ximian.com)
//
// Copyright (C) 2010 Novell, Inc (http://www.novell.com)
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

using Mono.Moonlight.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MoonTest.System.Windows.Input {

	[TestClass]
	public class ManipulationCompletedEventArgsTest {

		[TestMethod]
		public void NonDesign ()
		{
			Assert.Throws<NotImplementedException> (delegate {
				new ManipulationCompletedEventArgs ();
			}, "ctor");
		}

		[TestMethod]
		public void InDesign ()
		{
			ManipulationCompletedEventArgs mcea = null;

			DesignerProperties.SetIsInDesignMode (Application.Current.RootVisual, true);
			try {
				// creation time limitation only
				mcea = new ManipulationCompletedEventArgs ();
			}
			finally {
				DesignerProperties.SetIsInDesignMode (Application.Current.RootVisual, false);
			}

			Assert.IsNull (mcea.FinalVelocities, "FinalVelocities");
			Assert.IsFalse (mcea.Handled, "Handled");
			Assert.IsFalse (mcea.IsInertial, "IsInertial");
			Assert.IsNull (mcea.ManipulationContainer, "ManipulationContainer");
			Assert.AreEqual (new Point (0,0), mcea.ManipulationOrigin, "ManipulationOrigin");
			Assert.IsNull (mcea.OriginalSource, "OriginalSource");
			Assert.IsNull (mcea.TotalManipulation, "TotalManipulation");
		}
	}
}

