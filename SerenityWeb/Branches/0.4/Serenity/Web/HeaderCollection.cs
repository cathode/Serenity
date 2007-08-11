﻿/*
Serenity - The next evolution of web server technology

Copyright © 2006-2007 Serenity Project (http://SerenityProject.net/)

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

namespace Serenity.Web
{
	/// <summary>
	/// Represents a collection of Header objects.
	/// </summary>
	public sealed class HeaderCollection : KeyedCollection<string, Header>
	{
		#region Methods - Protected
		protected override string GetKeyForItem(Header item)
		{
			return item.Name;
		}
		#endregion
		#region Methods - Public
		/// <summary>
		/// Creates and adds a new Header to the current HeaderCollection.
		/// </summary>
		/// <param name="name">The name of the new Header.</param>
		/// <param name="value">The value of the new Header.</param>
		/// <returns>The newly created Header.</returns>
		public Header Add(string name, string value)
		{
			Header header = new Header(name, value);
			this.Add(header);
			return header;
		}
		#endregion
	}
}