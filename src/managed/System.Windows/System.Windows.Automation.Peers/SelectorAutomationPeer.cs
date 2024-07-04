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
// Copyright (c) 2009 Novell, Inc. (http://www.novell.com)
//
// Contact:
//   Moonlight Team (moonlight-list@lists.ximian.com)
//

using System.Windows;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Linq;
using System.Collections.Generic;

namespace System.Windows.Automation.Peers
{

    public abstract class SelectorAutomationPeer : ItemsControlAutomationPeer, ISelectionProvider
    {

        protected SelectorAutomationPeer(Selector owner) : base(owner)
        {
            owner.SelectionChanged += (o, e) =>
            {
                // Selection Pattern Automation Events
                // - CanSelectMultiple and IsSelectionRequired not raised because value doesn't change/
                // - Selection.SelectionProperty:
                RaisePropertyChangedEvent(SelectionPatternIdentifiers.SelectionProperty,
                                           GetProviderArrayFromPeer(oldSelectedPeer),
                               GetProviderArrayFromPeer(GetSelectedAutomationPeer()));

                // SelectionItem Pattern Automation Events
                // (Only raising SelectionItemPatternOnElementSelected because Selector 
                //  supports one selected item)
                if (oldSelectedPeer != null) // Old Selected Item
                    oldSelectedPeer.RaisePropertyChangedEvent(SelectionItemPatternIdentifiers.IsSelectedProperty,
                                                               true,
                                           false);
                oldSelectedPeer = GetSelectedAutomationPeer();
                if (oldSelectedPeer != null)
                { // New Selected Item
                    oldSelectedPeer.RaisePropertyChangedEvent(SelectionItemPatternIdentifiers.IsSelectedProperty,
                                                               false,
                                           true);
                    oldSelectedPeer.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementSelected);
                }
            };
            oldSelectedPeer = GetSelectedAutomationPeer();
        }

        public override object GetPattern(PatternInterface patternInterface)
        {
            if (patternInterface == PatternInterface.Selection)
                return this;

            return base.GetPattern(patternInterface);
        }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return base.GetAutomationControlTypeCore();
        }

        protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
        {
            return base.CreateItemAutomationPeer(item);
        }

        #region ISelectionProvider realization

        IRawElementProviderSimple[] ISelectionProvider.GetSelection()
        {
            return GetProviderArrayFromPeer(GetSelectedAutomationPeer());
        }

        bool ISelectionProvider.CanSelectMultiple => false;

        bool ISelectionProvider.IsSelectionRequired => false;

        #endregion

        private AutomationPeer GetSelectedAutomationPeer()
        {
            Selector selector = (Selector)Owner;
            if (selector.SelectedIndex == -1)
                return null;

            return CreatePeerForElement((UIElement)selector.ItemContainerGenerator.ContainerFromIndex(selector.SelectedIndex));
        }

        private IRawElementProviderSimple[] GetProviderArrayFromPeer(AutomationPeer peer)
        {
            if (peer != null)
                return new IRawElementProviderSimple[] { ProviderFromPeer(peer) };
            return null;
        }

        private AutomationPeer oldSelectedPeer;

        #region Internal properties

        internal override ScrollViewer ScrollPatternImplementor => ((Selector)Owner).TemplateScrollViewer;

        #endregion
    }
}
