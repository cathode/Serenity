using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity.WebApps.ServerInfo
{
    /// <summary>
    /// Implements a web application that provides simple server information upon request.
    /// </summary>
    public class ServerInfoWebApp : WebApplication
    {
        #region Fields
        private WebServer server;
        #endregion
        #region Constructors
        public ServerInfoWebApp()
        {

        }
        public ServerInfoWebApp(WebServer server)
        {
            this.server = server;
        }
        #endregion
        #region Properties
        public override string Name
        {
            get
            {
                return "ServerInfo";
            }
        }

        public override Guid UniqueID
        {
            get
            {
                return new Guid("{39DD1DF1-1F04-47F5-B759-E4A41B7D6651}");
            }
        }

        public override Version Version
        {
            get
            {
                return new Version(0, 1, 0, 0);
            }
        }
        public override string DefaultBinding
        {
            get
            {
                return "/ServerInfo";
            }
        }
        #endregion
        #region Methods
        public override void InitializeResources()
        {
        }
        #endregion

        public override void ProcessRequest(Web.Request request, Web.Response response)
        {
            if (this.server == null)
                return;

            foreach (var binding in this.server.Bindings)
                response.WriteLine(string.Format("Binding: {0} ({1}) - {2}", binding.Application.Name, binding.Application.UniqueID, binding.Binding));
        }
    }
}
