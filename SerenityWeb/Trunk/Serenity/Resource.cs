using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity
{
	public abstract class Resource
	{
		#region Fields - Protected
		protected string name;
		#endregion
		#region Properties - Public
		public virtual string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}
		public string SystemName
		{
			get
			{
				return this.Name.ToLower();
			}
		}
		#endregion
	}
}
