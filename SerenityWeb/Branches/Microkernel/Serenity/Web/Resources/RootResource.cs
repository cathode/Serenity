using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Serenity.Net;

namespace Serenity.Web.Resources
{
    public sealed class RootResource : DirectoryResource
    {
        internal RootResource(Server owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }
            this.Owner = owner;
        }
        public override void OnRequest(Request request, Response response)
        {
            base.OnRequest(request, response);
        }
    }
}
