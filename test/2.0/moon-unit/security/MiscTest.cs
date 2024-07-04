//
// Misc Security Unit Tests
//
// Contact:
//   Moonlight List (moonlight-list@lists.ximian.com)
//
// Copyright 2009 Novell, Inc.
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
using System.Diagnostics;
using System.Security;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;

using Mono.Moonlight.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// newer SMCS cannot compile this correctly - since the compiler prefers the version its compiling rather than the one in mscorlib.dll
// -> error CS0656: The compiler required member `System.Runtime.CompilerServices.RuntimeHelpers.InitializeArray(System.Array, System.RuntimeFieldHandle)' could not be found or is inaccessible
#if false
// this icall exists on both mono and sl
// note: CSC prefer this type when compiling this assembly, while SMCS prefers the original type from mscorlib.dll
namespace System.Runtime.CompilerServices {
	public class RuntimeHelpers {

		[MethodImpl (MethodImplOptions.InternalCall)]
		public static extern object GetObjectValue (object obj);
	}
}
#endif

namespace MoonTest.Security {

	[TestClass]
	public class MiscSecurityTest {

		[TestMethod]
		public void Array_Transparent ()
		{
			// transparent type
			FrameworkElement [] fe = new FrameworkElement [0];
			Assert.IsNotNull (fe, "FrameworkElement[]");
		}

		[TestMethod]
		[MoonlightBug ("actually it looks like a SL bug")]
		// reported as https://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=490406
		[ExpectedException (typeof (TypeLoadException))]
		public void Array_Critical ()
		{
			// exception occurs before reaching the code (JIT time) and the harness consider this a success
			Assert.IsFalse (true, "This fails before the method is executed");
			// critical type
			AppDomainManager [] adm = new AppDomainManager [0];
			Assert.IsNotNull (adm, "AppDomainManager[]");
		}

		[TestMethod]
		public void CreateInstance_Critical ()
		{
			Type t = typeof (AppDomainManager);
			Assert.IsNotNull (Array.CreateInstance (t, 1), "Array.CreateInstance-1");
			Assert.IsNotNull (Array.CreateInstance (t, 2, 2), "Array.CreateInstance-2");

			Assert.IsNotNull (Activator.CreateInstance (t.MakeArrayType (1), 1), "Activator.CreateInstance-1");
			Assert.IsNotNull (Activator.CreateInstance (t.MakeArrayType (2), 2, 2), "Activator.CreateInstance-2");
		}

		[TestMethod]
		public void MultidimentionalArrays ()
		{
			// transparent type
			FrameworkElement [,] fe = new FrameworkElement [0, 0];
			Assert.IsNotNull (fe, "FrameworkElement[,]");

			// critical type
			AppDomainManager [,] adm = new AppDomainManager [0, 0];
			Assert.IsNotNull (adm, "AppDomainManager[,]");
		}

		[TestMethod]
		public void JaggedArrays ()
		{
			// transparent type
			FrameworkElement [][] fe = new FrameworkElement [0][];
			Assert.IsNotNull (fe, "FrameworkElement[,]");

			// critical type
			AppDomainManager [][] adm = new AppDomainManager [0][];
			Assert.IsNotNull (adm, "AppDomainManager[,]");
		}

		[TestMethod]
		[ExpectedException (typeof (SecurityException))]
		[MoonlightBug ("smcs compiles this as using mscorlib, while csc compile this using the newly defined type")]
		public void RedefineNonCriticalInternalCall ()
		{
			RuntimeHelpers.GetObjectValue (null);
		}

		[MethodImpl (MethodImplOptions.InternalCall)]
		static extern void NonExistingInternalCall ();

		[TestMethod]
		[ExpectedException (typeof (SecurityException))]
		public void DefineNonExistingInternalCall ()
		{
			NonExistingInternalCall ();
		}

#if false
		// enabling this test would throw a TypeLoadException and stop moon-unit from working
		// same behavior happens on SL2
		[ComImport]
		[Guid ("782A4EF2-60CC-4237-9DB3-E7905B0D7AFD")]
		public class Imported {
		}

		[TestMethod]
		public void ComImport_ ()
		{
			// SL2 does not support COM but [ComImport] exists and the (mono in moonlight)
			// runtime expects to be capable to initiate COM when it sees this attribute
			// leading to a crash (g_assert on missing types) before rXXXXXX
			Assert.IsNotNull (new Imported ());
		}
#endif
		AppDomainManager GetCritical ()
		{
			return null;
		}

		bool SetCritical (AppDomainManager value)
		{
			return (value == null);
		}

		[TestMethod]
		[MoonlightBug ("we do not throw - but API is unusable")]
		public void Method_SecurityCritical ()
		{
			Assert.Throws<MethodAccessException> (delegate {
				Assert.IsNotNull (GetCritical ()); // Assert should not be executed
			}, "return value");
			Assert.Throws<MethodAccessException> (delegate {
				Assert.IsFalse (SetCritical (null)); // Assert should not be executed
			}, "parameter");
		}

		private AppDomainManager field;

		[TestMethod]
		[MoonlightBug ("we do not throw - but API is unusable")]
		public void Field_SecurityCritical ()
		{
			Assert.Throws<FieldAccessException> (delegate {
				Assert.IsNotNull (field, "read"); // Assert should not be executed
			}, "read");
			Assert.Throws<FieldAccessException> (delegate {
				field = null;
			}, "write");
		}

		[TestMethod]
		[MoonlightBug ("we do not throw - but API is unusable")]
		public void Local_SecurityCritical ()
		{
#pragma warning disable 219 // The variable `adm' is assigned but its value is never used
			AppDomainManager adm; 
			// we can't read from it -> Use of unassigned variable 'adm'
			Assert.Throws<FieldAccessException> (delegate {
				adm = null;
			}, "write");
		}
	}
}

