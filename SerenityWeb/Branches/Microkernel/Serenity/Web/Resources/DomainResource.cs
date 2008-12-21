using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity.Web.Resources
{
    public sealed class DomainResource : DynamicResource
    {
        public override void OnRequest(Request request, Response response)
        {
            base.OnRequest(request, response);

        }
    }
}
