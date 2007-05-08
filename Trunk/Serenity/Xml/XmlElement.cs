/*
Serenity - The next evolution of web server technology

Copyright © 2006-2007 Serenity Project (http://serenityproject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity.Xml
{
    /// <summary>
    /// Represents an element in an XML document.
    /// </summary>
    public class XmlElement : XmlNode
    {
        #region Constructors - Internal
        /// <summary>
        /// Initializes a new instance of the XmlElement class.
        /// </summary>
        /// <param name="Name"></param>
        internal XmlElement(string Name) : base(Name, null, null)
        {
            this.attributes = new XmlNodeCollection<XmlAttribute>();
            this.children = new XmlNodeCollection<XmlNode>();
        }
        #endregion
        #region Fields - Private
        private XmlNodeCollection<XmlAttribute> attributes;
        private XmlNodeCollection<XmlNode> children;
        private XmlNode nextSibling;
        private XmlNode previousSibling;
        #endregion
        #region Indexers - Public
        /// <summary>
        /// Gets the child XmlNode at the position specified by Index.
        /// </summary>
        /// <param name="Index">The zero-based index to retrieve a child XmlNode at.</param>
        /// <returns>The child XmlNode at Index, or null if no XmlNode was found.</returns>
        public virtual XmlNode this[int Index]
        {
            get
            {
                if (this.children.Count > Index)
                {
                    return this.children[Index];
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// Gets the first child of the current XmlNode whose name matches the specified ChildName.
        /// </summary>
        /// <param name="name">The name to match against.</param>
        /// <returns>The first child XmlNode with a name matching ChildName, or null if no XmlNode was found.</returns>
        public virtual XmlNode this[string name]
        {
            get
            {
                if (this.children.Contains(name) == true)
                {
                    return this.children[name];
                }
                else
                {
                    return null;
                }
            }
        }
                #endregion
        #region Methods - Public
        /// <summary>
        /// Appends an attribute to the current XmlElement.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AppendAttribute(string name, string value)
        {
            this.attributes.Add(new XmlAttribute(name, value));
        }
        /// <summary>
        /// Appends an attribute to the current XmlElement.
        /// </summary>
        /// <param name="attribute"></param>
        public void AppendAttribute(XmlAttribute attribute)
        {
            this.attributes.Add(attribute);
        }
        /// <summary>
        /// Appends an XmlNode to the children of the current XmlNode.
        /// </summary>
        /// <param name="node">The XmlNode to append.</param>
        public virtual void AppendChild(XmlNode node)
        {
            if (node != null)
            {
                //node._Parent = this;
                this.children.Add(node);
            }
        }
        /// <summary>
        /// Appends a range of XmlNodes to the children of the current XmlNode.
        /// </summary>
        /// <param name="Nodes">The XmlNodes to append.</param>
        public virtual void AppendChildren(IEnumerable<XmlNode> Nodes)
        {
            if (Nodes != null)
            {
                foreach (XmlNode Node in Nodes)
                {
                    this.AppendChild(Node);
                }
            }
        }
        public void AppendComment(string comment)
        {
            this.children.Add(new XmlComment(comment));
        }

        /// <summary>
        /// Appends valid XML markup to the current XmlElement.
        /// Nodes contained in the valid XML markup string are added as
        /// children to the current XmlElement, in the order that they are read.
        /// </summary>
        /// <param name="markup">The XML markup to append to the current XmlElement.</param>
        public virtual void AppendMarkup(string markup)
        {
            TryResult<XmlNodeCollection<XmlNode>> Result = XmlReader.TryReadMarkup(markup);
            if (Result.IsSuccessful == true)
            {
                this.AppendChildren(Result.Value);
            }
        }
        /// <summary>
        /// Appends an XML Preprocessor Directive to the current XmlElement.
        /// </summary>
        /// <param name="name">The name of the new XML Preprocessor Directive.</param>
        /// <param name="value">The value of the new XmlPreprocessorDirective.</param>
        public virtual void AppendPreprocessorDirective(string name, string value)
        {
            this.children.Add(new XmlPreprocessorDirective(name, value));
        }
        /// <summary>
        /// Appends text to the current XmlElement.
        /// </summary>
        /// <param name="Text">The text to append to the current XmlElement.</param>
        public void AppendText(string Text)
        {
            if ((this.children.IsEmpty == false) && (this.children.LastNode is XmlTextNode))
            {
                this.children.LastNode.Value += Text;
            }
            else
            {
                this.AppendChild(new XmlTextNode(Text));
            }
        }
        public string GetAttributeValue(string Name)
        {
            if (this.attributes.Contains(Name) == true)
            {
                return this.attributes[Name].Value;
            }
            else
            {
                return "";
            }
        }
        public IEnumerable<XmlElement> GetElementsByName(string Name)
        {
            foreach (XmlNode Node in this.Children)
            {
                if (Node is XmlElement)
                {
                    if (Node.Name == Name)
                    {
                        yield return (XmlElement)Node;
                    }
                }
            }
        }

        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets the XmlNodeCollection containing the attributes of the current XmlElement.
        /// </summary>
        public XmlNodeCollection<XmlAttribute> Attributes
        {
            get
            {
                return this.attributes;
            }
        }
        /// <summary>
        /// Gets the XmlNodeCollection&lt;XmlNode&gt; containing the children of the current XmlNode.
        /// </summary>
        public XmlNodeCollection<XmlNode> Children
        {
            get
            {
                return this.children;
            }
        }
        /// <summary>
        /// Gets a boolean value which indicates if the current XmlElement
        /// contains any attributes (true), or not (false).
        /// </summary>
        public bool HasAttributes
        {
            get
            {
                return ((this.attributes == null) || (this.attributes.Count == 0)) ? false : true;
            }
        }
        /// <summary>
        /// Gets a value which indicates whether the current XmlNode actually contains any child nodes.
        /// </summary>
        public bool HasChildren
        {
            get
            {
                return (this.children.Count == 0) ? false : true;
            }
        }
        /// <summary>
        /// Gets a boolean value which indicates if the current XmlNode
        /// posesses a next sibling (true), or not (false).
        /// </summary>
        public bool HasNextSibling
        {
            get
            {
                return (this.nextSibling == null) ? false : true;
            }
        }
        /// <summary>
        /// Gets a boolean value which indicates if the current XmlNode
        /// posesses a previous sibling (true), or not (false).
        /// </summary>
        public bool HasPreviousSibling
        {
            get
            {
                return (this.previousSibling == null) ? false : true;
            }
        }
        /// <summary>
        /// Gets the complete markup of all child nodes.
        /// </summary>
        public virtual string InnerMarkup
        {
            get
            {
                StringBuilder Output = new StringBuilder();
                foreach (XmlNode Node in this.Children)
                {
                    Output.Append(Node.OuterMarkup);
                }
                return Output.ToString();
            }
            set
            {
                TryResult<XmlNodeCollection<XmlNode>> Result = XmlReader.TryReadMarkup(value);
                if (Result.IsSuccessful == true)
                {
                    this.Children.Clear();
                    this.AppendChildren(Result.Value);
                }
            }
        }
        /// <summary>
        /// Gets the textual content of the current XmlElement and all child elements.
        /// </summary>
        public virtual string InnerText
        {
            get
            {
                StringBuilder Result = new StringBuilder();
                foreach (XmlNode Node in this.Children)
                {
                    Result.Append(Node.Value);
                }
                return Result.ToString();
            }
            set
            {
                this.Children.Clear();
                this.AppendText(value);
            }
        }
        /// <summary>
        /// Gets a boolean value which indicates if the current XmlElement is an empty element
        /// (has no child nodes, but may have attributes).
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                if (this.Children.Count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets (and sets for derived classes only) the XmlNode which
        /// is the next sibling of the current XmlNode.
        /// </summary>
        public XmlNode NextSibling
        {
            get
            {
                return this.nextSibling;
            }
            internal set
            {
                this.nextSibling = value;
            }
        }
        /// <summary>
        /// Gets the complete markup of the current XmlElement and all child nodes.
        /// </summary>
        public override string OuterMarkup
        {
            get
            {
                StringBuilder Output = new StringBuilder(string.Format("<{0}", this.Name));
                if (this.HasAttributes == true)
                {
                    foreach (XmlAttribute Attribute in this.Attributes)
                    {
                        if (Attribute.HasValue == true)
                        {
                            Output.AppendFormat(" {0}", Attribute.OuterMarkup);
                        }
                    }
                }
                if (this.HasChildren == true)
                {
                    Output.AppendFormat(">{1}</{0}>", this.Name, this.InnerMarkup);
                }
                else
                {
                    Output.Append(" />");
                }
                return Output.ToString();
            }
        }
        /// <summary>
        /// Gets (and sets for derived classes only) the XmlNode which
        /// is the previous sibling of the current XmlNode.
        /// </summary>
        public XmlNode PreviousSibling
        {
            get
            {
                return this.previousSibling;
            }
            protected set
            {
                this.previousSibling = value;
            }
        }
        #endregion
    }
}
