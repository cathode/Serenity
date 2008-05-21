using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity.Web.Forms
{
    public sealed class ControlAttributeCollection : Collection<ControlAttribute>
    {
        public void EnableAttribute(string name)
        {
            if (!this.Contains(name))
            {
                return;
            }

            var attribute = (from a in this
                             where a.Name == name
                             select a).First();
            attribute.Include = true;
                           

        }
        public void DisableAttribute(string name)
        {
        }
        public bool Contains(string attributeName)
        {
            return (from a in this
                    where a.Name == attributeName
                    select a).Count() > 0;
        }
    }
}
