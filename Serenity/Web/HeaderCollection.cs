/******************************************************************************
 * Serenity - Managed Web Application Server. ( http://gearedstudios.com/ )   *
 * Copyright © 2006-2011 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace Serenity.Web
{
	/// <summary>
	/// Represents a collection of Header objects.
	/// </summary>
	public sealed class HeaderCollection : KeyedCollection<string, Header>
	{
		#region Methods
		/// <summary>
		/// Creates and adds a new Header to the current HeaderCollection.
		/// </summary>
		/// <param name="name">The name of the new Header.</param>
		/// <param name="value">The value of the new Header.</param>
		/// <returns>The newly created Header.</returns>
		public Header Add(string name, string value)
		{
            Contract.Requires(!string.IsNullOrEmpty(name));

			Header header = new Header(name, value);
			this.Add(header);
			return header;
		}
        protected override string GetKeyForItem(Header item)
        {
            return item.Name;
        }
		#endregion
	}
}