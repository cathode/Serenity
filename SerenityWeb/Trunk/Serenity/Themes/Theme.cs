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
using System.Text;

namespace Serenity.Themes
{
	/// <summary>
	/// Represents a theme; a collection of styling properties and display information
	/// that can be applied universally to any themeable resource.
	/// </summary>
	public sealed class Theme
	{
		private string author;
		private Uri url;
		private Version version;
	}
}
