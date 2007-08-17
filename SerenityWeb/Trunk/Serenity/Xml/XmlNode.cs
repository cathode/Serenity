/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Permissive License (Ms-PL), a copy of which should have been included with *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity.Xml
{
    /// <summary>
    /// Represents an XML node. This class is abstract.
    /// </summary>
    public abstract class XmlNode
    {
        #region Constructors - Internal
        /// <summary>
        /// Initializes a new instance of the XmlNode class.
        /// </summary>
        /// <param name="name">The name of the new XmlNode instance.</param>
        internal XmlNode(string name) : this(name, null, null)
        {
        }
        /// <summary>
        /// Initializes a new instance of the XmlNode class.
        /// </summary>
        /// <param name="name">The name of the new XmlNode instance.</param>
        /// <param name="namespacePrefix">The XML namespace of the new XmlNode instance.</param>
        internal XmlNode(string name, string namespacePrefix) : this(name, namespacePrefix, null)
        {
        }
        /// <summary>
        /// Initializes a new instance of the XmlNode class.
        /// </summary>
        /// <param name="name">The name of the new XmlNode instance.</param>
        /// <param name="namespacePrefix">The XML namespace of the new XmlNode instance.</param>
        /// <param name="parent">The parent XmlNode of the new XmlNode instance.</param>
        internal XmlNode(string name, string namespacePrefix, XmlNode parent)
        {
            this.name = name;
            this.namespacePrefix = namespacePrefix;
            this.parent = parent;
        }
        #endregion
        #region Fields - Private
        private bool hidden;
        private string namespacePrefix;
        private string value;
        #endregion
        #region Fields - Protected
        /// <summary>
        /// Holds the name of the current XmlNode.
        /// </summary>
        protected string name;
        /// <summary>
        /// Holds the parent XmlNode.
        /// </summary>
        protected XmlNode parent;
        #endregion
        #region Properties - Public

        /// <summary>
        /// Gets a boolean value which indicates if the current XmlNode
        /// posesses a parent XmlNode (true), or not (false).
        /// </summary>
        public bool HasParent
        {
            get
            {
                return (this.parent == null) ? false : true;
            }
        }

        /// <summary>
        /// Gets a boolean value which indicates if the current XmlNode has a value.
        /// </summary>
        public bool HasValue
        {
            get
            {
                return (string.IsNullOrEmpty(this.value) == true) ? false : true;
            }
        }
        /// <summary>
        /// Gets or sets a value that determines if the current XmlNode will be used.
        /// </summary>
        public bool Hidden
        {
            get
            {
                return this.hidden;
            }
            set
            {
                this.hidden = value;
            }
        }
        /// <summary>
        /// Gets (and sets for derived classes) the name of the current XmlNode.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }
        /// <summary>
        /// Gets (and sets for derived classes) the XML namespace of the current XmlNode.
        /// </summary>
        public string NamespacePrefix
        {
            get
            {
                return this.namespacePrefix;
            }
            internal set
            {
                this.namespacePrefix = value;
            }
        }
        /// <summary>
        /// When overridden in a derived class, gets the entire XML markup for the current node and all child nodes.
        /// </summary>
        public abstract string OuterMarkup
        {
            get;
        }
        /// <summary>
        /// Gets the XmlNode which is the parent of the current XmlNode.
        /// </summary>
        public XmlNode Parent
        {
            get
            {
                return this.parent;
            }
        }

        /// <summary>
        /// Gets or sets a string value associated with the current XmlNode.
        /// </summary>
        public virtual string Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }
        #endregion
    }
}