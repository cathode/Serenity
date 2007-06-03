using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity.Web
{
    public class MimeType
    {
        public static string Format(string primary, string secondary)
        {
            return primary + "/" + secondary;
        }
    }
}
