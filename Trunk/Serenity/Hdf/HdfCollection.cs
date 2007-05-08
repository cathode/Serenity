using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Serenity.Hdf
{
    /// <summary>
    /// Represents a collection of HdfElement objects which can be accessed by index or name/path.
    /// </summary>
    public sealed class HdfCollection : KeyedCollection<string, HdfElement>
    {
        #region Constructors - Internal
        internal HdfCollection()
        {
        }
        #endregion
        #region Methods - Public
        protected override string GetKeyForItem(HdfElement item)
        {
            if (item.Dataset.IsCaseSensitive)
            {
                return item.Name;
            }
            else
            {
                return item.Name.ToLower();
            }
        }
        #endregion
    }
}
