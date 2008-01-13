using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace Serenity.Web.Controls
{
    public abstract class Control
    {
        private string name = Control.DefaultName;

        public const string DefaultName = "control";

        public string Render()
        {

        }
        public void Render(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
        }

        public virtual string Name
        {
            get
            {
                return this.name;
            }
            protected set
            {
                this.name = value;
            }
        }
    }
}
