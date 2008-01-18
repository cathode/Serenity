using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Serenity.Resources;
using Serenity.Web;
using Serenity.Web.Drivers;

namespace Serenity.Pages.Admin
{
    /// <summary>
    /// Provides a dynamic page for configuring general server options.
    /// </summary>
    public sealed class MainConfigPage : DynamicResource
    {
        #region Constructors - Public
        public MainConfigPage()
        {
            this.Name = "MainConfig";
            this.ContentType = MimeType.ApplicationXml;
        }
        #endregion
        #region Methods - Public
        public override void OnRequest(CommonContext context)
        {

        }
        #endregion
    }
}
