/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Permissive License (Ms-PL), a copy of which should have been included with *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Serenity.Web
{
	/// <summary>
	/// Represents a collection of RequestDataStreams that are associated with an incoming or outgoing request.
	/// </summary>
	public sealed class RequestDataCollection : KeyedCollection<string, RequestDataStream>
	{
		#region Methods - Public
		/// <summary>
		/// Creates and adds a new RequestDataStream to the current RequestDataCollection.
		/// </summary>
		/// <param name="name">The name of the new stream.</param>
		/// <param name="data">The data to populate the new stream with.</param>
		/// <returns>The created RequestDataStream.</returns>
		public RequestDataStream AddDataStream(string name, byte[] data)
		{
			if (!this.Contains(name))
			{
				RequestDataStream stream = new RequestDataStream(name, data);
				this.Add(stream);
				return stream;
			}
			else
			{
				return null;
			}
		}
		#endregion
		#region Methods - Protected
		protected override string GetKeyForItem(RequestDataStream item)
		{
			return item.Name;
		}
		#endregion
	}
}