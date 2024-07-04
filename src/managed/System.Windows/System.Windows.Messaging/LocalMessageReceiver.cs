//
// Contact:
//   Moonlight List (moonlight-list@lists.ximian.com)
//
// Copyright 2009-2010 Novell, Inc.
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

using Mono;
using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Windows.Messaging
{

    public sealed partial class LocalMessageReceiver : INativeEventObjectWrapper, IDisposable
    {
        static char[] InvalidChars = { ',', ':' };
        static List<string> any_domain = new List<string> { "*" };
        public static readonly IEnumerable<string> AnyDomain = new ReadOnlyCollection<string>(any_domain);

        DependencyObjectHandle handle;

        public LocalMessageReceiver(string receiverName)
            : this(receiverName, ReceiverNameScope.Domain, null, false)
        {
        }

        public LocalMessageReceiver(string receiverName,
                         ReceiverNameScope nameScope,
                         IEnumerable<string> allowedSenderDomains)
            : this(receiverName, nameScope, allowedSenderDomains, true)
        {
        }

        private LocalMessageReceiver(string receiverName,
                         ReceiverNameScope nameScope,
                         IEnumerable<string> allowedSenderDomains,
                         bool checkDomains)
            : this(NativeMethods.local_message_receiver_new(receiverName,
                                      (int)nameScope), true)
        {
            if (receiverName == null)
                throw new ArgumentNullException("receiverName");
            if (receiverName.Length > 256)
                throw new ArgumentException("receiverName");

            string[] sender_domains;

            if (checkDomains)
            {
                if (allowedSenderDomains == null)
                    throw new ArgumentNullException("allowedSenderDomains");

                List<string> domains = new List<string>();
                foreach (string s in allowedSenderDomains)
                {
                    if (s == null)
                        throw new ArgumentNullException("allowedSenderDomains");
                    if ((s.Length > 256) || (s.IndexOfAny(InvalidChars) != -1))
                        throw new ArgumentException("allowedSenderDomains");
                    domains.Add(s);
                }
                sender_domains = domains.ToArray();
                this.allowedSenderDomains = new ReadOnlyCollection<string>(domains);
            }
            else
            {
                sender_domains = new string[] { "*" };
                // AllowedSenderDomains needs to be 'null'
            }

            NativeMethods.local_message_receiver_set_allowed_sender_domains(NativeHandle, sender_domains, sender_domains.Length);
        }

        internal LocalMessageReceiver(IntPtr raw, bool dropref)
        {
            NativeHandle = raw;
            if (dropref)
                NativeMethods.event_object_unref(raw);
        }

        public void Listen()
        {
            listening = true;
            NativeMethods.local_message_receiver_listen(NativeHandle);
        }

        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
                NativeMethods.local_message_receiver_dispose(NativeHandle);
            }
        }

        public IEnumerable<string> AllowedSenderDomains => allowedSenderDomains;

        [MonoTODO("IE7 Protected Mode only (not applicable to Moonlight)")]
        public bool DisableSenderTrustCheck
        {
            get => disableSenderTrustCheck;
            set
            {
                if (disposed)
                    throw new ObjectDisposedException(String.Empty);
                if (listening)
                    throw new InvalidOperationException();
                disableSenderTrustCheck = value;
            }
        }

        public ReceiverNameScope NameScope => (ReceiverNameScope)NativeMethods.local_message_receiver_get_receiver_name_scope(NativeHandle);

        public string ReceiverName => NativeMethods.local_message_receiver_get_receiver_name(NativeHandle);

        public event EventHandler<MessageReceivedEventArgs> MessageReceived
        {
            add => EventList.RegisterEvent(this, EventIds.LocalMessageReceiver_MessageReceivedEvent, value, Events.CreateMessageReceivedEventArgsEventHandlerDispatcher(value));
            remove => EventList.UnregisterEvent(this, EventIds.LocalMessageReceiver_MessageReceivedEvent, value);
        }

        IEnumerable<string> allowedSenderDomains;
        bool disableSenderTrustCheck;

        bool listening;
        bool disposed;


        internal IntPtr NativeHandle
        {
            get => handle.Handle;
            set
            {
                if (handle != null)
                {
                    throw new InvalidOperationException("native handle is already set");
                }

                NativeDependencyObjectHelper.AddNativeMapping(value, this);
                handle = new DependencyObjectHandle(value, this);
            }
        }

        IntPtr INativeEventObjectWrapper.NativeHandle
        {
            get => NativeHandle;
            set => NativeHandle = value;
        }

        void INativeEventObjectWrapper.MentorChanged(IntPtr mentor_ptr)
        {
        }

        Kind INativeEventObjectWrapper.GetKind()
        {
            return Kind.LOCALMESSAGERECEIVER;
        }

        private EventHandlerList event_list;


        EventHandlerList INativeEventObjectWrapper.EventList => EventList;

        private EventHandlerList EventList
        {
            get
            {
                if (event_list == null)
                    event_list = new EventHandlerList(this);
                return event_list;
            }
        }
    }
}
