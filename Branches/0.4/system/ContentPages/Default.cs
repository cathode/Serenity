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