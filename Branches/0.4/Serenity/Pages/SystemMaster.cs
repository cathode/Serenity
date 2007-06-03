using System;
using System.Collections.Generic;
using System.Text;

using Serenity.Web;

namespace Serenity.Pages
{
    internal sealed class SystemMaster : MasterPage
    {
        public override void PostRequest(CommonContext context)
        {
            
        }
        public override void PreRequest(CommonContext context)
        {
            //WS: Insert auth/credential checking code here
            //    (to prevent unauthorized access to administrative stuff)
        }
    }
}
