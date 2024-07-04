//
// XamlNode.cs
//
// Contact:
//   Moonlight List (moonlight-list@lists.ximian.com)
//
// Copyright 2011 Novell, Inc.
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

//#define LOGGING

using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Text.RegularExpressions;

namespace Mono.Xaml
{

    internal interface IXamlNode
    {
        string Prefix { get; }
        string LocalName { get; }
        string NamespaceURI { get; }
        string Name { get; }
        string Value { get; }
        XmlNodeType NodeType { get; }
    }


    internal struct XamlAttribute : IXamlNode
    {
        string prefix;
        string localName;
        string namespaceUri;
        string name;
        string val;

        public string Prefix => prefix;
        public string LocalName => localName;
        public string NamespaceURI => namespaceUri;
        public string Name => name;
        public string Value => val;

        public XmlNodeType NodeType => XmlNodeType.Attribute;

        public int Index;
        public bool IsMapping;
        public bool IsNsXaml;
        public bool IsNsClr;
        public bool IsNsIgnorable;
        public bool IsXmlDirective;

        public XamlAttribute(XmlReader reader)
        {
            prefix = reader.Prefix;
            localName = reader.LocalName;
            name = reader.Name;
            namespaceUri = reader.NamespaceURI;
            val = reader.Value;
            Index = 0;
            IsNsXaml = IsNsClr = IsNsIgnorable = false;
            IsMapping = (reader.Prefix == "xmlns" || reader.Name == "xmlns");
            IsXmlDirective = reader.Prefix == "xml";
        }

        public string ClrNamespace
        {
            get
            {
                if (IsNsClr)
                {
                    int end = NamespaceURI.IndexOf(';');
                    if (end == -1)
                        end = NamespaceURI.Length;
                    return NamespaceURI.Substring(14, end - 14);
                }
                return null;
            }
        }

        public string Assembly
        {
            get
            {
                if (IsNsClr && NamespaceURI.IndexOf(";assembly=") + 10 < NamespaceURI.Length)
                    return NamespaceURI.Substring(NamespaceURI.IndexOf(";assembly=") + 10);
                return null;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}=\"{1}\"", Name, Value);
        }
    }

    internal class XamlNode : IXmlLineInfo, IXamlNode
    {

#if LOGGING
		static void Log (string str, params object[] args)
		{
			Console.WriteLine (str, args);
		}
#endif
        static XamlNode()
        {
            cache = new Dictionary<string, XamlNode>();
        }

        public XamlNode()
        {
            Attributes = new Dictionary<string, XamlAttribute>();
            Children = new List<XamlNode>();
        }

        public bool HasAttributes => Attributes.Count > 0;

        public int AttributeCount => Attributes.Count;

        public XmlNodeType NodeType => isInAttr ? XmlNodeType.Attribute : type;

        public string NamespaceURI
        {
            get
            {
                int pos = attrpos;
                return isInAttr ? (from x in Attributes.Values where x.Index == pos select x.NamespaceURI).Single() : namespaceURI;
            }
        }

        public string Prefix
        {
            get
            {
                int pos = attrpos;
                return isInAttr ? (from x in Attributes.Values where x.Index == pos select x.Prefix).Single() : prefix;
            }
        }

        public string LocalName
        {
            get
            {
                int pos = attrpos;
                return isInAttr ? (from x in Attributes.Values where x.Index == pos select x.LocalName).Single() : localName;
            }
        }

        public string Name
        {
            get
            {
                int pos = attrpos;
                return isInAttr ? (from x in Attributes.Values where x.Index == pos select x.Name).Single() : name;
            }
        }

        public string Value
        {
            get
            {
                int pos = attrpos;
                return isInAttr ? (from x in Attributes.Values where x.Index == pos select x.Value).Single() : val;
            }
        }

        // this is only used when generating a xaml representation, and even then,
        // only if the node doesn't store the original xaml already
        Dictionary<string, string> Namespaces => Attributes.Values.Where(x => x.IsMapping).ToDictionary(x => x.Name == "xmlns" ? "" : x.LocalName, x => x.Value);

        string defaultXmlns;
        public string DefaultXmlns
        {
            get
            {
                if (defaultXmlns == null)
                    defaultXmlns = top.defaultXmlns;
                return defaultXmlns;
            }
            private set => defaultXmlns = value;
        }

        public string this[string n]
        {
            get
            {
                return GetAttribute(n);
            }
        }

        public string this[string n, string ns]
        {
            get
            {
                return (from o in Attributes.Values where o.LocalName == n && o.NamespaceURI == ns select o.Value).FirstOrDefault();
            }
        }

        public int LineNumber
        {
            get;
            private set;
        }

        public int LinePosition
        {
            get;
            private set;
        }

        public bool IsTopElement => this == top;

        public List<string> IgnorablePrefixes => top.ignorablePrefixes;

        public XamlNode Parent => parent;

        public bool HasLineInfo()
        {
            return LineNumber > -1;
        }

        public string GetAttribute(string name)
        {
            XamlAttribute ret;
            if (Attributes.TryGetValue(name, out ret))
                return ret.Value;
            return null;
        }

        public string ResolvePrefix(string prefix)
        {
            string ret = (from o in Attributes.Values where o.IsMapping && o.LocalName == prefix select o.Value).FirstOrDefault();
            if (ret != null || parent == null)
                return ret;
            return parent.ResolvePrefix(prefix);
        }

        public void MoveToAttribute(int i)
        {
            isInAttr = true;
            attrpos = i;
        }

        public void MoveToElement()
        {
            isInAttr = false;
        }

        public override string ToString()
        {
            if (!Parsed)
                return outerXml;

            if (start == null)
            {
                if (NodeType == XmlNodeType.Element)
                {
                    start = String.Format("<{0}", Name);
                    foreach (XamlAttribute att in Attributes.Values)
                    {
                        start += " " + att;
                    }
                }
                else if (NodeType == XmlNodeType.Text)
                    start = Value;
            }
            if (end == null && NodeType == XmlNodeType.Element)
                end = IsEmptyElement ? " />" : ">";

            return start + end;
        }

        public string OuterXml
        {
            get
            {
                if (outerXml == null)
                    outerXml = DoOuterXml(new List<string>(), true);
                return outerXml;
            }
            set => outerXml = value;
        }

        string DoOuterXml(List<string> xmlns, bool onTop)
        {
            if (NodeType != XmlNodeType.Element)
                return this.ToString();

            if (!Parsed && !onTop)
            {
                Regex rx = new Regex("xmlns:?\\w?=\".*\"");
                return rx.Replace(outerXml, "");
            }

            string xml = String.Format("<{0}", Name); ;

            if (!xmlns.Contains(prefix))
                xmlns.Add(prefix);

            foreach (XamlAttribute ai in Attributes.Values)
            {
                if (!ai.IsMapping)
                    xml += " " + ai;
                string p = ai.IsMapping ? (ai.Name == "xmlns" ? "" : ai.LocalName) : ai.Prefix;
                if (!xmlns.Contains(p))
                    xmlns.Add(p);
            }


            string str = "";
            if (!IsEmptyElement)
            {
                str = ">";
                end = String.Format("\n</{0}>", Name);
                foreach (XamlNode child in Children)
                    str += "\n" + child.DoOuterXml(xmlns, false);
            }
            else
                end = " />";

            if (onTop)
            {
                foreach (string p in xmlns)
                {
                    xml += String.Format(" xmlns{0}{1}=\"{2}\"", p == "" ? "" : ":", p, Namespaces[p]);
                }
            }

            return xml + str + end;
        }

        public string ReadOuterXml()
        {
            return OuterXml;
        }

        public static XamlNode Parse(string xml, NodeEvent evstart, NodeEvent evend, AttributeEvent evattr)
        {
            XamlNode root;
            bool exists = cache.TryGetValue(xml, out root);
            if (!exists)
                root = new XamlNode() { outerXml = xml };
            root.Parse(evstart, evend, evattr);
            if (!exists)
                cache[xml] = root;
            return root;
        }

        public void Parse(NodeEvent evstart, NodeEvent evend, AttributeEvent evattr)
        {
#if LOGGING
			Log ("Looping: Ignored? {0} Parsed? {1} {2}", this.Ignore, this.Parsed, outerXml);
#endif
            Ignore = false;

            if (!Initialized)
            {
                XmlReader reader = XmlReader.Create(new StringReader(outerXml));
                Parse(reader, parent, this, evstart, evend, evattr);
                Parsed = !Ignore;
                return;
            }

            if (evstart != null && (NodeType == XmlNodeType.Element || NodeType == XmlNodeType.Text))
                evstart(this);

            if (evattr != null)
            {
                foreach (XamlAttribute ai in Attributes.Values.Where(x => !x.IsNsIgnorable && !x.IsMapping))
                    evattr(this, ai);
            }

            if (NodeType == XmlNodeType.Element && !IsEmptyElement && !Ignore)
            {
                if (!Parsed)
                {
                    XmlReader reader = XmlReader.Create(new StringReader(outerXml));
                    reader.Read();
                    XamlNode child = null;
                    do
                    {
                        child = Parse(reader, this, null, evstart, evend, evattr, child != null ? child.Ignore : false);
                        if (child != null)
                        {
                            child.Parsed = !child.Ignore;
                            Children.Add(child);
                        }
                    } while (child != null);
                    Parsed = true;
                }
                else
                {
                    foreach (XamlNode child in Children)
                    {
                        child.Parse(evstart, evend, evattr);
                    }
                }
            }

            if (evend != null && NodeType == XmlNodeType.Element)
                evend(this);

            Ignore = false;
        }

        /**
		@node If it's not null, it probably means we're on the top node and just
				got called from public Parse method
		@parent If it's null, it means we're on the top node
		@skip don't do a read, we're already on the next node and ready to process it

		If this method returns null, it means it has reached the end of the current
		xml subtree (the end element of the parent node)
		**/
        static XamlNode Parse(XmlReader reader, XamlNode parent, XamlNode node,
                                NodeEvent evstart, NodeEvent evend,
                                AttributeEvent evattr, bool skip = false)
        {
            if (!skip)
                reader.Read();

            if (parent == null)
            {
                while (reader.NodeType != XmlNodeType.Element)
                    if (!reader.Read())
                        return null;
            }
            else
            {
                do
                {
                    while (reader.NodeType != XmlNodeType.Element &&
                           reader.NodeType != XmlNodeType.EndElement &&
                           reader.NodeType != XmlNodeType.Text)
                    {
                        if (!reader.Read())
                            return null;
                    }
                    if (reader.NodeType == XmlNodeType.Element &&
                        parent.top.IgnorablePrefixes.Contains(reader.Prefix))
                        reader.Skip();
                    else
                        break;
                } while (true);
            }

            if (reader.NodeType == XmlNodeType.EndElement)
                return null;

            if (node == null)
                node = new XamlNode();

            node.Parsed = false;
            node.Ignore = false;

            if (!node.Initialized)
            {
                node.parent = parent;

                if (parent != null)
                    node.top = parent.top;
                else
                {
                    node.ignorablePrefixes = new List<string>();
                    node.top = node;
                }

                node.type = reader.NodeType;
                node.namespaceURI = reader.NamespaceURI;
                node.prefix = reader.Prefix;
                node.name = reader.Name;
                node.localName = reader.LocalName;
                node.IsEmptyElement = reader.IsEmptyElement;
                node.HasValue = reader.HasValue;
                node.XmlSpace = reader.XmlSpace;
                node.LineNumber = -1;
                node.LinePosition = -1;
                node.IsNsXaml = node.NamespaceURI == XamlUri;
                node.IsNsClr = node.NamespaceURI.StartsWith("clr-namespace");
                if (parent == null)
                    node.DefaultXmlns = reader.GetAttribute("xmlns");

                if (node.IsNsClr)
                {
                    int end = node.NamespaceURI.IndexOf(';');
                    if (end == -1)
                        end = node.NamespaceURI.Length;
                    node.ClrNamespace = node.NamespaceURI.Substring(14, end - 14);
                }

                if (node.IsNsClr && node.NamespaceURI.IndexOf(";assembly=") + 10 < node.NamespaceURI.Length)
                    node.Assembly = node.NamespaceURI.Substring(node.NamespaceURI.IndexOf(";assembly=") + 10);

                if (node.HasValue)
                    node.val = reader.Value;


                IXmlLineInfo linfo = reader as IXmlLineInfo;
                if (linfo != null)
                {
                    node.LineNumber = linfo.LineNumber;
                    node.LinePosition = linfo.LinePosition;
                }

                if (reader.HasAttributes)
                {
                    string ig = reader.GetAttribute("Ignorable", IgnorableUri);
                    if (ig != null)
                    {
                        foreach (string s in ig.Split(' '))
                            node.IgnorablePrefixes.Add(s);
                    }

                    reader.MoveToFirstAttribute();

                    int count = 0;
                    do
                    {
                        // this filters out ignorable attributes
                        if (node.IgnorablePrefixes.Contains(reader.Prefix))
                            continue;

                        XamlAttribute ai = new XamlAttribute(reader)
                        {
                            Index = count++,
                        };

                        // an x: or equivalent attribute
                        ai.IsNsXaml = !ai.IsMapping && ai.NamespaceURI == XamlUri;
                        // an mc: or equivalent attribute (this attribute defines the list of ignorable prefixes)
                        ai.IsNsIgnorable = !ai.IsMapping && (ai.NamespaceURI == IgnorableUri || ai.Prefix == "mc");
                        // an xmlns:my='clr-namespace...' attribute
                        ai.IsNsClr = !ai.IsMapping && ai.NamespaceURI.StartsWith("clr-namespace");
                        if (ai.IsNsXaml && ai.LocalName == "Class")
                            node.Class = ai.Value;

                        if (ai.IsMapping)
                        {
                            if (node.top != node)
                                ai.Index = node.top.Attributes.Count;
                            node.top.Attributes[reader.Name] = ai;
                        }
                        else
                        {
                            if (ai.IsNsXaml)
                            {
                                if (ai.LocalName == "Key")
                                    node.X_Key = ai.Value;
                                else if (ai.LocalName == "Name")
                                    node.X_Name = ai.Value;
                            }
                            node.Attributes[reader.Name] = ai;
                        }
                    } while (reader.MoveToNextAttribute());

                    reader.MoveToElement();
                }

                node.Initialized = true;
            }

            if (evstart != null && (node.NodeType == XmlNodeType.Element || node.NodeType == XmlNodeType.Text))
                evstart(node);

            if (evattr != null)
            {
                foreach (XamlAttribute ai in node.Attributes.Values.Where(x => !x.IsNsIgnorable && !x.IsMapping))
                    evattr(node, ai);
            }

            if (node.Ignore)
            {
                node.outerXml = reader.ReadOuterXml();
                cache[node.outerXml] = node;
                if (evend != null && node.NodeType == XmlNodeType.Element)
                    evend(node);
                return node;
            }

            if (node.NodeType == XmlNodeType.Element && !node.IsEmptyElement)
            {
                XamlNode child = null;
                do
                {
                    child = Parse(reader, node, null, evstart, evend, evattr, child != null ? child.Ignore : false);
                    if (child != null)
                    {
                        child.Parsed = !child.Ignore;
                        node.Children.Add(child);
                    }
                } while (child != null);
            }

            if (evend != null && node.NodeType == XmlNodeType.Element)
                evend(node);

            return node;
        }

        static Dictionary<string, XamlNode> cache;
        public static string IgnorableUri = "http://schemas.openxmlformats.org/markup-compatibility/2006";
        public static string PresentationUri = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";
        public static string XamlUri = "http://schemas.microsoft.com/winfx/2006/xaml";
        public static string LegacyXamlUri = "http://schemas.microsoft.com/client/2007";

        public List<XamlNode> Children;
        public Dictionary<string, XamlAttribute> Attributes;
        List<string> ignorablePrefixes;

        XamlNode parent;
        XamlNode top;

        int attrpos;
        bool isInAttr;

        XmlNodeType type;
        string namespaceURI;
        string prefix;
        string localName;
        string name;
        string val;
        string start;
        string end;
        string outerXml;

        public bool Ignore;
        public bool Parsed;
        public bool Initialized;
        public bool IsEmptyElement;
        public bool HasValue;
        public XmlSpace XmlSpace;

        public bool IsNsXaml;
        public bool IsNsClr;
        public string ClrNamespace;
        public string Assembly;
        public string Class;
        public string X_Key;
        public string X_Name;

        public Type ManagedType;

        public delegate void NodeEvent(XamlNode node);
        public delegate void AttributeEvent(XamlNode node, XamlAttribute ai);
    }
}
