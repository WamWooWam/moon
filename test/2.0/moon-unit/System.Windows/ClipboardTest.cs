//
// Unit tests for Clipboard
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
using System.Security;
using System.Windows;

using Mono.Moonlight.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MoonTest.System.Windows {

	[TestClass]
	public class ClipboardTest {

		[TestMethod]
		public void ContainsText_NonUserInitiated ()
		{
			// can return true or false - i.e. it does not need to be user initiated
			Clipboard.ContainsText ();
		}

		[TestMethod]
		public void GetText_NonUserInitiated ()
		{
			Assert.Throws<SecurityException> (delegate {
				Clipboard.GetText ();
			}, "GetText");
		}

		[TestMethod]
		public void SetText_Null ()
		{
			// null check is done before user-initiated check
			Assert.Throws<ArgumentNullException> (delegate {
				Clipboard.SetText (null);
			}, "SetText");
		}

		[TestMethod]
		public void SetText_NonUserInitiated ()
		{
			Assert.Throws<SecurityException> (delegate {
				Clipboard.SetText ("hey!");
			}, "SetText");
		}
	}
}

