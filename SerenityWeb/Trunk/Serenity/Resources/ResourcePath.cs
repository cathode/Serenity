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
    /// Represents a information that can be used to locate a resource.
    /// </summary>
    public sealed class ResourcePath : ICloneable<ResourcePath>, IComparable<ResourcePath>, IEquatable<ResourcePath>
    {
        #region Constructors - Public
        public ResourcePath(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            Uri u = new Uri(path, UriKind.RelativeOrAbsolute);

            if (u.IsAbsoluteUri)
            {
                this.Domain = u.Host;
                this.IsDomainUsed = true;
                this.Path = u.AbsolutePath;
                this.scheme = u.Scheme;
                this.IsSchemeUsed = true;
            }
            else
            {
                this.path = path;
            }
        }
        public ResourcePath(Uri pathUri)
        {

        }
        #endregion
        #region Fields - Private
        private string domain;
        private bool isDomainUsed;
        private bool isSchemeUsed;
        private string scheme;
        private string path;
        #endregion
        #region Fields - Public
        public const string DefaultDomain = "localhost";
        public const string DefaultScheme = "http";
        #endregion
        #region Methods - Public
        public ResourcePath Clone()
        {
            return new ResourcePath(this.path);
        }
        public int CompareTo(ResourcePath other)
        {
            if (other == null)
            {
                return 1;
            }
            else if (ResourcePath.ReferenceEquals(this, other))
            {
                return 0;
            }
            else if (!this.IsSchemeUsed && other.IsSchemeUsed)
            {
                return 1;
            }
            else if (!this.IsDomainUsed && other.IsDomainUsed)
            {
                return 1;
            }
            return this.ToString().CompareTo(other.ToString());
        }
        public static ResourcePath Create(string uriString)
        {
            if (uriString == null)
            {
                throw new ArgumentNullException("uriString");
            }

            ResourcePath uri = new ResourcePath(uriString);
            return uri;
        }
        public override bool Equals(object obj)
        {
            if (object.Equals(obj, null))
            {
                return false;
            }
            else if (!obj.GetType().TypeHandle.Equals(typeof(ResourcePath).TypeHandle))
            {
                return false;
            }
            else
            {
                return ResourcePath.Equals(this, (ResourcePath)obj);
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
            else if (object.Equals(a, null) || object.Equals(b, null))
            {
                return false;
            }

            return (a.GetHashCode() == b.GetHashCode());
        }
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode() ^ 0x5D13A04C;
        }
        public override string ToString()
        {
            return this.ToString(true, true);
        }
        public string ToString(bool includeDomain)
        {
            return this.ToString(true, true);
        }
        public string ToString(bool includeDomain, bool includeProtocolScheme)
        {
            return this.path;
        }
        #endregion
        #region Operators
        public static bool operator ==(ResourcePath a, ResourcePath b)
        {
            return ResourcePath.Equals(a, b);
        }
        public static bool operator !=(ResourcePath a, ResourcePath b)
        {
            return !ResourcePath.Equals(a, b);
        }
        #endregion
        #region Properties - Public
        public int Depth
        {
            get
            {
                return 0;
            }
        }
        public string Domain
        {
            get
            {
                return this.domain;
            }
            set
            {
                this.domain = value;
            }
        }
        public bool IsDirectory
        {
            get
            {
                return this.path.EndsWith("/");
            }
        }
        public bool IsDomainUsed
        {
            get
            {
                return this.isDomainUsed;
            }
            set
            {
                this.isDomainUsed = value;
            }
        }
        public bool IsSchemeUsed
        {
            get
            {
                return this.isSchemeUsed;
            }
            set
            {
                this.isSchemeUsed = value;
            }
        }
        public string Path
        {
            get
            {
                return this.path;
            }
            set
            {
                this.path = value;
            }
        }
        #endregion
        #region ICloneable Members
        object ICloneable.Clone()
        {
            return this.Clone();
        }
        #endregion
    }
}
