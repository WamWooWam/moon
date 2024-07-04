//
// DesignerProperties Unit Tests
//
// Contact:
//   Moonlight List (moonlight-list@lists.ximian.com)
//
// Copyright 2008, 2010 Novell, Inc.
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
using System.ComponentModel;
using System.Windows;
using System.Windows.Shapes;

using Mono.Moonlight.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MoonTest.System.ComponentModel {

	[TestClass]
	public class DesignerPropertiesTest {

		private Rectangle rect;

		[TestInitialize]
		public void Setup ()
		{
			rect = new Rectangle ();
		}

		[TestCleanup]
		public void Cleanup()
		{
			rect = null;
		}

		[TestMethod]
		public void GetIsInDesignMode ()
		{
			Assert.Throws<ArgumentNullException> (delegate {
				DesignerProperties.GetIsInDesignMode (null);
			}, "null");
			Assert.IsFalse (DesignerProperties.GetIsInDesignMode (rect), "Rectangle");
		}

		[TestMethod]
		public void SetIsInDesignMode ()
		{
			Assert.Throws<ArgumentNullException> (delegate {
				DesignerProperties.SetIsInDesignMode (null, true);
			}, "null");

			Assert.Throws<NotImplementedException> (delegate {
				DesignerProperties.SetIsInDesignMode (rect, true);
			}, "Rectangle/True");

			rect.SetValue (DesignerProperties.IsInDesignModeProperty, true);
			Assert.IsInstanceOfType<bool>(rect.ReadLocalValue(DesignerProperties.IsInDesignModeProperty), "locally set");
			Assert.IsTrue((bool) rect.GetValue(DesignerProperties.IsInDesignModeProperty), "#getter true");
			Assert.IsFalse (DesignerProperties.GetIsInDesignMode (rect), "Rectangle/SetValue/False/1");
			Assert.IsTrue ((bool) rect.GetValue (DesignerProperties.IsInDesignModeProperty), "Rectangle/GetValue/True");

			Assert.Throws<NotImplementedException> (delegate {
				DesignerProperties.SetIsInDesignMode (rect, false);
			}, "Rectangle/False");

			rect.SetValue (DesignerProperties.IsInDesignModeProperty, false);
			Assert.IsFalse (DesignerProperties.GetIsInDesignMode (rect), "Rectangle/SetValue/False/2");
			Assert.IsFalse ((bool) rect.GetValue (DesignerProperties.IsInDesignModeProperty), "Rectangle/GetValue/False");

			DesignerProperties.SetIsInDesignMode(Application.Current.RootVisual, true);
			Assert.IsTrue(DesignerProperties.GetIsInDesignMode(Application.Current.RootVisual), "Is in design");

			Assert.Throws<NotImplementedException>(delegate {
				DesignerProperties.SetIsInDesignMode(rect, true);
			}, "Rectangle/False");
		}

		[TestMethod]
		public void CurrentApplication ()
		{
			Assert.IsFalse (DesignerProperties.GetIsInDesignMode (Application.Current.RootVisual), "Current");
			DesignerProperties.SetIsInDesignMode (Application.Current.RootVisual, true);
			try {
				Assert.IsTrue (DesignerProperties.GetIsInDesignMode (Application.Current.RootVisual), "GetIsInDesignMode/true");
				Assert.IsTrue ((bool) Application.Current.RootVisual.GetValue (DesignerProperties.IsInDesignModeProperty), "GetValue/true");

				Application.Current.RootVisual.SetValue (DesignerProperties.IsInDesignModeProperty, false);
				Assert.IsFalse (DesignerProperties.GetIsInDesignMode (Application.Current.RootVisual), "GetIsInDesignMode/false");
				Assert.IsFalse ((bool) Application.Current.RootVisual.GetValue (DesignerProperties.IsInDesignModeProperty), "GetValue/false");
			}
			finally {
				DesignerProperties.SetIsInDesignMode (Application.Current.RootVisual, false);
			}
		}
	}
}
