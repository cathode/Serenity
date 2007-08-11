using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Serenity.IO;

namespace Serenity.Web.Drivers
{
    public sealed class AsyncHttpWriter : Writer<CommonContext> 
    {
        public override bool Write(System.IO.Stream stream, CommonContext value)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
