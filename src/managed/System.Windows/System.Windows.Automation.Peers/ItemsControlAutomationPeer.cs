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
// Copyright (c) 2008 Novell, Inc. (http://www.novell.com)
//
// Contact:
//   Moonlight Team (moonlight-list@lists.ximian.com)
//

using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;

namespace System.Windows.Automation.Peers
{

    public abstract class ItemsControlAutomationPeer : FrameworkElementAutomationPeer
    {
        protected ItemsControlAutomationPeer(ItemsControl owner) : base(owner)
        {
            owner.Items.ItemsChanged += (o, e) =>
            {
                RaiseAutomationEvent(AutomationEvents.StructureChanged);
            };
        }

        protected virtual ItemAutomationPeer CreateItemAutomationPeer(object item)
        {
            throw new NotImplementedException();
        }

        public override object GetPattern(PatternInterface patternInterface)
        {
            if (patternInterface == PatternInterface.Scroll)
            {
                ScrollViewer patternImplementor = ScrollPatternImplementor;
                if (patternImplementor != null)
                {
                    AutomationPeer peer
                        = CreatePeerForElement(patternImplementor);
                    return peer.GetPattern(patternInterface);
                }
                return null;
            }

            return base.GetPattern(patternInterface);
        }

        protected override List<AutomationPeer> GetChildrenCore()
        {
            ItemsControl itemsControl = (ItemsControl)Owner;
            if (itemsControl.Items.Count == 0)
                return null;

            List<AutomationPeer> children = new List<AutomationPeer>();
            for (int index = 0; index < itemsControl.Items.Count; index++)
            {
                UIElement item = GetItem(itemsControl, index);
                if (item == null)
                    return null;

                AutomationPeer peer = CreatePeerForElement(item);
                if (peer != null)
                    children.Add(peer);
            }

            return children;
        }

        internal virtual ScrollViewer ScrollPatternImplementor => null;

        private UIElement GetItem(ItemsControl itemsControl, int index)
        {
            var item = (UIElement)itemsControl.ItemContainerGenerator.ContainerFromIndex(index);
            if (item == null)
                item = itemsControl.Items[index] as UIElement;
            return item;
        }
    }

}
