using System;
using System.Collections.Generic;
using System.Text;

using Serenity.Xml.Html;

namespace Serenity.Web.Controls
{
    public abstract class Control
    {
        public abstract HtmlElement Render();
    }
}
