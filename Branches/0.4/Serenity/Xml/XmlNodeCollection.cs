/*
Serenity - The next evolution of web server technology

Copyright © 2006-2007 Serenity Project (http://SerenityProject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Serenity.Xml
{
    /// <summary>
    /// Represents a collection of XmlNodes, backed by a doubly linked list. This class cannot be inherited.
    /// </summary>
    public sealed class XmlNodeCollection<T> : ICollection<T> where T : XmlNode
    {
        #region Constructors - Internal
        /// <summary>
        /// Initializes a new instance of the XmlNodeCollection class.
        /// </summary>
        internal XmlNodeCollection() : this(null)
        {
        }
        /// <summary>
        /// Initializes a new instance of the XmlNodeCollection class.
        /// </summary>
        /// <param name="initialNodes">An IEnumerable&lt;XmlNode&gt; object
        /// containing XmlNodes to initially populate the new XmlNodeCollection instance with.</param>
        internal XmlNodeCollection(IEnumerable<T> initialNodes)
        {
            if (initialNodes != null)
            {
                this.nodes = new LinkedList<T>(initialNodes);
            }
            else
            {
                this.nodes = new LinkedList<T>();
            }
        }
        #endregion
        #region Fields - Public
        private LinkedList<T> nodes;
        #endregion
        #region Indexers - Public
        /// <summary>
        /// Gets the item at the specified Index in the current XmlNodeCollection.
        /// </summary>
        /// <param name="Index"></param>
        /// <returns></returns>
        public T this[int Index]
        {
            get
            {
                int I = 0;
                foreach (T Node in this.nodes)
                {
                    if (I == Index)
                    {
                        return Node;
                    }
                    else
                    {
                        I++;
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// Gets the first item found with the specified Name in the current XmlNodeCollection.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public T this[string name]
        {
            get
            {
                LinkedListNode<T> Node = this.nodes.First;
                while (Node != null)
                {
                    if (Node.Value.Name == name)
                    {
                        return Node.Value;
                    }
                    else
                    {
                        Node = Node.Next;
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// Gets the first item found with the specified Name and Namespace in the current XmlNodeCollection.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="namespacePrefix"></param>
        /// <returns></returns>
        public T this[string name, string namespacePrefix]
        {
            get
            {
                foreach (T Node in this.nodes)
                {
                    if ((Node.Name == name) && (Node.NamespacePrefix == namespacePrefix))
                    {
                        return Node;
                    }
                }
                return null;
            }
        }
        #endregion
        #region Methods - Public
        /// <summary>
        /// Adds an XmlNode to the current XmlNodeCollection.
        /// </summary>
        /// <param name="node">The XmlNode which will be added.</param>
        public void Add(T node)
        {
            if (node != null)
            {
                this.nodes.AddLast(node);
            }
        }
        /// <summary>
        /// Adds a range of XmlNodes to the current XmlNodeCollection.
        /// </summary>
        /// <param name="nodes">The XmlNodes which will be added.</param>
        public void AddRange(IEnumerable<T> nodes)
        {
            if (nodes != null)
            {
                foreach (T node in nodes)
                {
                    if (node != null)
                    {
                        this.Add(node);
                    }
                }
            }
        }
        /// <summary>
        /// Clears the XmlNodes in the current XmlNodeCollection.
        /// </summary>
        public void Clear()
        {
            this.nodes.Clear();
        }
        /// <summary>
        /// Determines if an XmlNode exists in the current XmlNodeCollection.
        /// </summary>
        /// <param name="name">The name of the XmlNode to check for.</param>
        /// <returns>True if node was found, false otherwise.</returns>
        public bool Contains(string name)
        {
            T node = this[name];
            if (node == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// Determines if an XmlNode exists in the current XmlNodeCollection.
        /// </summary>
        /// <param name="node">The XmlNode to check for.</param>
        /// <returns>True if node was found, false otherwise.</returns>
        public bool Contains(T node)
        {
            if (this.nodes.Contains(node) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Determines if an XmlNode exists in the current XmlNodeCollection.
        /// </summary>
        /// <param name="name">The name of the XmlNode to check for.</param>
        /// <param name="namespacePrefix">The namespace of the XmlNode to check for.</param>
        /// <returns>True if node was found, false otherwise.</returns>
        public bool Contains(string name, string namespacePrefix)
        {
            T Node = this[name, namespacePrefix];
            if (Node == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// Copies the XmlNodes in the current XmlNodeCollection to the specified XmlNode TargetArray.
        /// </summary>
        /// <param name="array">The XmlNode array to copy to.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            int I = arrayIndex;
            foreach (T Node in this.nodes)
            {
                if (I >= array.Length)
                {
                    break;
                }
                else
                {
                    array[I] = Node;
                    I++;
                }
            }
        }
        /// <summary>
        /// Returns a filtered XmlNodeCollection, containing only items of the specified filter type.
        /// </summary>
        /// <typeparam name="TFilter">The type to filter to.</typeparam>
        /// <returns></returns>
        public IEnumerable<TFilter> FilterExclusive<TFilter>() where TFilter : T
        {
            foreach (T node in this.nodes)
            {
                if (node is TFilter)
                {
                    yield return (TFilter)node;
                }
            }
        }
        /// <summary>
        /// Returns a filtered IEnumerable&lt;TFilter&gt; object, containing only items
        /// of type TFilter, and Name matching the specified name.
        /// </summary>
        /// <typeparam name="TFilter">The type to filter to.</typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<TFilter> FilterExclusive<TFilter>(string name) where TFilter : T
        {
            foreach (T node in this.nodes)
            {
                if ((node is TFilter) && (node.Name == name))
                {
                    yield return (TFilter)node;
                }
            }
        }
        /// <summary>
        /// Returns a filtered IEnumerable&lt;T&gt; object containing all items of the current
        /// XmlNodeCollection&lt;T&gt; except items of type TFilter.
        /// </summary>
        /// <typeparam name="TFilter">The type to filter out.</typeparam>
        /// <returns></returns>
        public IEnumerable<T> FilterInclusive<TFilter>() where TFilter : T
        {
            foreach (T node in this.nodes)
            {
                if ((node is TFilter) == false)
                {
                    yield return node;
                }
            }
        }
        /// <summary>
        /// Returns a filtered IEnumerable&lt;T&gt; object, containing all items of the current
        /// XmlNodeCollection&lt;T&gt; except items of type TFilter,
        /// if the item's Name matches the specified name.
        /// </summary>
        /// <typeparam name="TFilter">The type to filter out.</typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<T> FilterInclusive<TFilter>(string name) where TFilter : T
        {
            foreach (T node in this.nodes)
            {
                if (((node is TFilter) == false) && (node.Name == name))
                {
                    yield return node;
                }
            }
        }
        /// <summary>
        /// Gets a generic enumerator for iterating over the XmlNodes
        /// contained in the current XmlNodeCollection.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            foreach (T item in this.nodes)
            {
                yield return item;
            }
        }
        /// <summary>
        /// Gets a nongeneric enumerator for iterating over the XmlNodes
        /// contained in the current XmlNodeCollection.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        /// <summary>
        /// Gets an array containing the nodes in the current collection.
        /// </summary>
        /// <returns></returns>
        public T[] ToArray()
        {
            T[] Result = new T[this.nodes.Count];
            this.nodes.CopyTo(Result, 0);
            return Result;
        }
        /// <summary>
        /// Removes the first XmlNode encountered whose Name matches name.
        /// </summary>
        /// <param name="name">The Name of the XmlNode to remove from from
        /// the current XmlNodeCollection.</param>
        /// <returns>True if the XmlNode was sucessfully removed,
        /// false if it was not removed or was not found.</returns>
        public bool Remove(string name)
        {
            foreach (T Node in this.nodes)
            {
                if (Node.Name == name)
                {
                    this.nodes.Remove(Node);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Removes the first XmlNode encountered which matches the specified node.
        /// </summary>
        /// <param name="node">The XmlNode to remove from the current XmlNodeCollection.</param>
        /// <returns>True if the XmlNode was sucessfully removed,
        /// false if it was not removed or was not found.</returns>
        public bool Remove(T node)
        {
            if (this.nodes.Contains(node) == true)
            {
                this.nodes.Remove(node);
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Removes the first XmlNode found which has the specified Name and Namespace.
        /// </summary>
        /// <param name="name">The name of the XmlNode to remove.</param>
        /// <param name="namespacePrefix">The namespace of the XmlNode to remove.</param>
        /// <returns>True if the node was removed successfully, or false if it was not found.</returns>
        public bool Remove(string name, string namespacePrefix)
        {
            T Node = this[name, namespacePrefix];
            if (Node == null)
            {
                return false;
            }
            else
            {
                this.nodes.Remove(Node);
                return true;
            }
        }
        /// <summary>
        /// Removes the XmlNode found at the specified index.
        /// </summary>
        /// <param name="index">A zero-based index in the current XmlNodeCollection&lt;T&gt;.</param>
        /// <returns>True if the node was removed successfully, or false if it was not found.</returns>
        public bool Remove(int index)
        {
            T node = this[index];
            if (node == null)
            {
                return false;
            }
            else
            {
                this.nodes.Remove(node);
                return true;
            }
        }
        #endregion Public Methods
        #region Properties - Public
        /// <summary>
        /// Gets the total number of XmlNodes actually contained in the current XmlNodeCollection.
        /// </summary>
        public int Count
        {
            get
            {
                return this.nodes.Count;
            }
        }
        /// <summary>
        /// Gets the first node contained in the XmlNodeCollection.
        /// </summary>
        public T FirstNode
        {
            get
            {
                return this.nodes.First.Value;
            }
        }
        /// <summary>
        /// Gets a boolean value which indicates if the current XmlNodeCollection contains any items.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return (this.nodes.Count > 0) ? false : true;
            }
        }
        /// <summary>
        /// Gets a boolean value which indicates whether the current
        /// XmlNodeCollection is read-only (true), or not (false).
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// Gets the last node contained in the XmlNodeCollection.
        /// </summary>
        public T LastNode
        {
            get
            {
                return this.nodes.Last.Value;
            }
        }
        #endregion
    }
}