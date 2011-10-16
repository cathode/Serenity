using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Serenity.Web;

namespace Serenity.WebApps.ServerInfo
{
    /// <summary>
    /// Implements a web application that provides simple server information upon request.
    /// </summary>
    public class ServerInfoWebApp : WebApplication
    {
        #region Fields
        #endregion
        #region Constructors
        internal ServerInfoWebApp()
        {
            this.Name = "ServerInfo";
            this.UniqueID = new Guid("{39DD1DF1-1F04-47F5-B759-E4A41B7D6651}");
            this.Version = new Version(1, 0, 0, 0);
        }
        #endregion
        #region Properties
      
        #endregion
        #region Methods
        public override void InitializeResources()
        {
            this.ApplicationRoot.Name = this.Name;
            this.ApplicationRoot.AttachResource(new DirectoryResource(this.Name));
            this.ApplicationRoot.AddChild(new ResourceGraphNode(new ListApps()));
            this.ApplicationRoot.AddChild(new ResourceGraphNode(new DetailInfo()));
        }
        public override void ProcessRequest(Web.Request request, Web.Response response)
        {
           
        }
        #endregion
    }
}
