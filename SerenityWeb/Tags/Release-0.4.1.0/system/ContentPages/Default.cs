/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Permissive License (Ms-PL), a copy of which should have been included with *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Text;

using Serenity;
using Serenity.Web;

namespace Serenity.System.ContentPages
{
	public partial class Default : ContentPage
	{
		public override ContentPage CreateInstance()
		{
			return new Default();
		}

		public override string Name
		{
			get
			{
				return "Default";
			}
		}
	}
}