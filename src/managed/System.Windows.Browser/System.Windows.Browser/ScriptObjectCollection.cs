/*
 * ScriptObjectCollection.cs.
 *
 * Contact:
 *   Moonlight List (moonlight-list@lists.ximian.com)
 *
 * Copyright 2008 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 * 
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Windows.Browser
{
    public sealed class ScriptObjectCollection : ScriptObject, IEnumerable, IEnumerable<ScriptObject>
    {
        internal ScriptObjectCollection(IntPtr handle) : base(handle)
        {
        }

        public int Count => GetProperty<int>("length");

        public ScriptObject this[int i]
        {
            get
            {
                return (ScriptObject)Invoke("item", i);
            }
        }

        IEnumerator<ScriptObject> IEnumerable<ScriptObject>.GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
                yield return this[i];
        }
    }
}
