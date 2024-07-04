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
// Copyright (c) 2010 Novell, Inc. (http://www.novell.com)
//
// Authors:
//	Rolf Bjarne Kvinge (RKvinge@novell.com)
//

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace System.ComponentModel
{
#if NET_2_1
	[TypeForwardedFrom ("System.Windows.Data, Version=2.0.5.0, Culture=Neutral, PublicKeyToken=31bf3856ad364e35")]
#endif
    public interface IEditableCollectionView
    {
        object AddNew();
        void CancelEdit();
        void CancelNew();
        void CommitEdit();
        void CommitNew();
        void EditItem(object item);
        void Remove(object item);
        void RemoveAt(int index);
        bool CanAddNew { get; }
        bool CanCancelEdit { get; }
        bool CanRemove { get; }
        object CurrentAddItem { get; }
        object CurrentEditItem { get; }
        bool IsAddingNew { get; }
        bool IsEditingItem { get; }
        NewItemPlaceholderPosition NewItemPlaceholderPosition { get; set; }
    }
}
