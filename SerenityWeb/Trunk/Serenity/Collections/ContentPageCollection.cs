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
using System.Collections.ObjectModel;
using System.Text;

namespace Serenity.Collections
{
	public sealed class ContentPageCollection : KeyedCollection<string, ContentPage>
	{
		#region Methods - Protected
		protected override string GetKeyForItem(ContentPage item)
		{
			return item.SystemName;
		}
		#endregion
	}
}
