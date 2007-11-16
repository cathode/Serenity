/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity.Resources
{
    /// <summary>
    /// Represents a path to a resource. This class is immutable.
    /// </summary>
    public sealed class ResourcePath : ICloneable<ResourcePath>, IEnumerable<string>, IEquatable<ResourcePath>
    {
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the ResourcePath class.
        /// </summary>
        /// <param name="path">The path string to create the new ResourcePath from.</param>
        /// <exception cref="System.ArgumentException">Thrown if 'path' is empty.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown if 'path' is null.</exception>
        public ResourcePath(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            this.path = path;
            this.segments = this.path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            //if (this.segments.Length == 0)
            //{
            //    this.segments = new string[1] { "" };
            //}
        }
        #endregion
        #region Fields - Private
        private readonly string path;
        private readonly string[] segments;
        #endregion
        #region Operators
        public static ResourcePath operator +(ResourcePath a, string b)
        {
            if (a == null)
            {
                throw new ArgumentNullException("a");
            }
            else if (b == null)
            {
                throw new ArgumentNullException("b");
            }
            else if (b == string.Empty)
            {
                throw new ArgumentException("Argument 'b' cannot be empty.");
            }

            if (a.IsDirectory)
            {
                return new ResourcePath(a.ToString() + b);
            }
            else
            {
                return new ResourcePath(a.ToString() + "/" + b);
            }
        }
        public static bool operator !=(ResourcePath a, ResourcePath b)
        {
            return !ResourcePath.Equals(a, b);
        }
        public static bool operator ==(ResourcePath a, ResourcePath b)
        {
            return ResourcePath.Equals(a, b);
        }
        #endregion
        #region Methods - Public
        public ResourcePath Clone()
        {
            return new ResourcePath(this.path);
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            else if (obj.GetType().TypeHandle.Equals(typeof(ResourcePath).TypeHandle))
            {
                return ResourcePath.Equals(this, (ResourcePath)obj);
            }
            else
            {
                return false;
            }
        }
        public bool Equals(ResourcePath other)
        {
            return ResourcePath.Equals(this, other);
        }
        public static bool Equals(ResourcePath a, ResourcePath b)
        {
            if (object.Equals(a, null) && object.Equals(b, null))
            {
                return true;
            }
            if (object.Equals(a, null) || object.Equals(b, null))
            {
                return false;
            }
            return a.path.Equals(b.path);
        }
        public IEnumerator<string> GetEnumerator()
        {
            foreach (string s in this.segments)
            {
                yield return s;
            }
        }
        public override int GetHashCode()
        {
            return this.path.GetHashCode() ^ 0x010CE52A;
        }
        public ResourcePath MakeDirectoryPath()
        {
            if (this.IsDirectory)
            {
                throw new InvalidOperationException();
            }
            return new ResourcePath(this.path + "/");
        }
        public override string ToString()
        {
            return this.path;
        }
        #endregion
        #region Properties - Public
        public int Depth
        {
            get
            {
                return this.segments.Length;
            }
        }
        public bool IsDirectory
        {
            get
            {
                return this.path.EndsWith("/");
            }
        }
        public string[] Segments
        {
            get
            {
                return this.segments.Clone() as string[];
            }
        }
        #endregion
        #region ICloneable Members
        object ICloneable.Clone()
        {
            return this.Clone();
        }
        #endregion
        #region IEnumerable Members
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion
    }
}
