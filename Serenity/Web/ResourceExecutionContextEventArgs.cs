using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity.Web
{
    public sealed class ResourceExecutionContextEventArgs : EventArgs
    {
        public ResourceExecutionContext Context
        {
            get;
            set;
        }
    }
}
