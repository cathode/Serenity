/*
Serenity - The next evolution of web server technology
Serenity/Hdf/HdfDataset.cs
Copyright � 2006-2007 Serenity Project (http://SerenityProject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
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
