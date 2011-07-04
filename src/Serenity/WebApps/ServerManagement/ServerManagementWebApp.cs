using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity.WebApps.ServerManagement
{
    public sealed class ServerManagementWebApp : WebApplication
    {
        public override string Name
        {
            get
            {
                return "Server Management";
            }
        }

        public override Guid UniqueID
        {
            get
            {
                return new Guid("{9D67106C-6081-41EC-A6DA-93B7E3A2AD21}");
            }
        }

        public override Version Version
        {
            get
            {
                return new Version(0, 1);
            }
        }

        public override void InitializeResources()
        {
            throw new NotImplementedException();
        }

        public override string DefaultBinding
        {
            get
            {
                return "/ServerAdmin";
            }
        }

        public override void ProcessRequest(Web.Request request, Web.Response response)
        {
            throw new NotImplementedException();
        }
    }
}
