/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Serenity
{
	/// <summary>
	/// Represents a file on the local filesystem that is exposed as a requestable resource.
	/// </summary>
	public sealed class StaticResource : Resource
	{
		#region Fields - Private
		private string location;
		#endregion
		#region Methods - Public
		public Stream GetStream()
		{
			if (File.Exists(this.location))
			{
				try
				{
					return File.Open(location, FileMode.Open);
				}
				catch
				{
					return null;
				}
			}
			else
			{
				return null;
			}
		}
		#endregion
		#region Properties - Public
		/// <summary>
		/// Gets the local filesystem location which the current StaticResource represents.
		/// </summary>
		public string Location
		{
			get
			{
				return this.location;
			}
			internal set
			{
				this.location = value;
			}
		}
		#endregion
	}
}
