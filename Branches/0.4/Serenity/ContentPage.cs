using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity
{
	/// <summary>
	/// Represents a page that generates dynamic content.
	/// </summary>
	public abstract class ContentPage : Page
	{
		public abstract ContentPage CreateInstance();
		public virtual MasterPage CreateMasterPageInstance()
		{
			return null;
		}
		#region Properties - Public
		/// <summary>
		/// Gets a boolean value which indicates if the current Page uses a MasterPage.
		/// </summary>
		public virtual bool HasMasterPage
		{
			get
			{
				return (this.CreateMasterPageInstance() == null) ? false : true;
			}
		}
		#endregion
	}
}
