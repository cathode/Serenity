using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity.Web.Resources
{
    public sealed class DomainResource : DirectoryResource
    {
        private string hostName;
        public override void OnRequest(Request request, Response response)
        {

        }
        public string HostName
        {
            get
            {
                return this.hostName;
            }
            set
            {
                this.hostName = value;
            }
        }
    }
}
