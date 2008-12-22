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
        public override Resource GetChild(Uri uri)
        {
            return this.GetChild(uri.Host) ?? base.GetChild(uri);
        }
        public override void OnRequest(Request request, Response response)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            else if (response == null)
            {
                throw new ArgumentNullException("response");
            }
            else if (response.IsComplete)
            {
                return;
            }
            Resource res = this.GetChild(request.Url);

            if (res != null)
            {
                res.OnRequest(request, response);
            }
            else
            {
                //TODO: Serve up a 404 or other error page.
                base.OnRequest(request, response);
            }
        }
    }
}
