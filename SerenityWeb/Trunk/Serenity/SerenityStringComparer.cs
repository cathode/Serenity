using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity
{
    public sealed class SerenityStringComparer : IEqualityComparer<string>
    {
        #region Constructors - Private
        private SerenityStringComparer()
        {
        }
        #endregion
        #region Fields - Private
        private static readonly SerenityStringComparer instance = new SerenityStringComparer();
        #endregion
        #region Methods - Public
        public bool Equals(string x, string y)
        {
            return string.Equals(x, y, SerenityServer.StringComparison);
        }
        public int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
        #endregion
        #region Properties - Public
        public static SerenityStringComparer Instance
        {
            get
            {
                return SerenityStringComparer.instance;
            }
        }
        #endregion
    }
}
