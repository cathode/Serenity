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

namespace Serenity.Attributes
{
	[AttributeUsage(AttributeTargets.Assembly)]
	public sealed class ModuleDefaultPageAttribute : Attribute
	{
		#region Constructors - Public
		public ModuleDefaultPageAttribute(string typeName)
		{
			this.TypeName = typeName;
		}
		#endregion
		#region Fields - Public
		public readonly string TypeName;
		#endregion
	}
}
