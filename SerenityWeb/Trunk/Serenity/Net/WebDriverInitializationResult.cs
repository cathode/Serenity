using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity.Net
{
    public enum WebDriverInitializationResult
    {
        Suceeded = 0x0,
        FailedSocketBinding,
        AlreadyInitialized,
    }
}
