/*
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
			RequestDataStream stream = new RequestDataStream(name, data);
			this.Add(stream);
			return stream;
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