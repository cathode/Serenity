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
using System.Collections.ObjectModel;
using System.Text;

namespace Serenity
{
    /// <summary>
    /// Represents a collection of DomainSettings objects.
    /// </summary>
    public sealed class DomainCollection : KeyedCollection<string, Domain>
    {
        #region Methods - Protected
        protected override void ClearItems()
        {
            if (SerenityServer.Status == OperationStatus.Started)
            {
                throw new InvalidOperationException("Cannot modify server-wide objects while server is running.");
            }
            base.ClearItems();
        }
        protected override string GetKeyForItem(Domain item)
        {
            return item.HostName;
        }
        protected override void InsertItem(int index, Domain item)
        {
            if (SerenityServer.Status == OperationStatus.Started)
            {
                throw new InvalidOperationException("Cannot modify server-wide objects while server is running.");
            }
            base.InsertItem(index, item);
        }
        protected override void RemoveItem(int index)
        {
            if (SerenityServer.Status == OperationStatus.Started)
            {
                throw new InvalidOperationException("Cannot modify server-wide objects while server is running.");
            }
            base.RemoveItem(index);
        }
        protected override void SetItem(int index, Domain item)
        {
            if (SerenityServer.Status == OperationStatus.Started)
            {
                throw new InvalidOperationException("Cannot modify server-wide objects while server is running.");
            }
            base.SetItem(index, item);
        }
        #endregion
        #region Methods - Public
        public Domain GetBestMatch(string hostName)
        {
            if (hostName == null)
            {
                throw new ArgumentNullException("hostName");
            }
            else if (this.Contains(hostName))
            {
                return this[hostName];
            }
            else
            {
                string parentHost = Domain.GetParentHost(hostName);
                if (parentHost == hostName)
                {
                    return null;
                }
                else
                {
                    return this.GetBestMatch(parentHost);
                }
            }
        }
        #endregion
    }
}
