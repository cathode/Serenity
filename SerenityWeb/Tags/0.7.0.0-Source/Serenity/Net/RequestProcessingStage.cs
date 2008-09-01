using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity.Net
{
    public enum RequestProcessingStage
    {
        None = 0x0,
        MethodProcessed = 0x1,
        UriProcessed = 0x2,
        VersionProcessed = 0x3,
        RequestLineProcessed = 0x4,
        HeaderProcessed = 0x5,
        AllHeadersProcessed = 0x6,
        ProcessingComplete = 0x7,
    }
}
