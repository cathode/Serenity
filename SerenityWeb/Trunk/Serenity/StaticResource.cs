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

using Serenity.Web;

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
        public override void OnRequest(CommonContext context)
        {
            if (File.Exists(this.location))
            {
                context.Response.Write(File.ReadAllBytes(this.location));
            }
            else
            {
                ErrorHandler.Handle(context, StatusCode.Http404NotFound);
            }
        }
		#endregion
		#region Properties - Public
        public override ResourceGrouping Grouping
        {
            get
            {
                return ResourceGrouping.Files;
            }
        }
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
