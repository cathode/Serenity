using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Serenity.IO;

namespace Serenity.Web.Drivers
{
    public sealed class AsyncHttpReader : Reader<CommonContext>
    {
        public override CommonContext Read(System.IO.Stream stream, out bool result)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
