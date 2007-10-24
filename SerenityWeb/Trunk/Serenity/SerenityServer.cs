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
using System.Text;

using Serenity.Web;
using Serenity.Web.Drivers;

namespace Serenity
{
	public sealed class SerenityServer
	{
		#region Fields - Private
		private ContextHandler contextHandler;
		private DriverPool driverPool;
		#endregion
		#region Properties - Public
		public ContextHandler ContextHandler
		{
			get
			{
				return this.contextHandler;
			}
			set
			{
				this.contextHandler = value;
			}
		}
		public DriverPool DriverPool
		{
			get
			{
				return this.driverPool;
			}
		}
		#endregion
	}
}