using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity.Attributes
{
    /// <summary>
    /// Provides a way to specify alternate paths for a resource.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class AlternateResourcePathAttribute : Attribute
    {
        #region Constructors - Public
        public AlternateResourcePathAttribute(string path) : this(path, true)
        {
        }
        public AlternateResourcePathAttribute(string path, bool isHardRewrite)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            this.path = path;
            this.isHardRewrite = isHardRewrite;
        }
        #endregion
        #region Fields - Private
        private readonly bool isHardRewrite;
        private readonly string path;
        #endregion
        #region Properties - Public
        public string Path
        {
            get
            {
                return this.path;
            }
        }
        #endregion
    }
}
