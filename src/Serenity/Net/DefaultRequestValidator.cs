using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Serenity.Web;

namespace Serenity.Net
{
    public class DefaultRequestValidator : RequestValidator
    {
        public override bool ValidateRequest(Request request, Response response)
        {
            // TODO: Implement basic request validation.
            return true;
        }
    }
}
