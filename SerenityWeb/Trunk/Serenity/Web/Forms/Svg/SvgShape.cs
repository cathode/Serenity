using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Serenity.Web.Forms.Svg
{
    public class SvgShape
    {
        private Point location;

        /// <summary>
        /// Gets or sets the location of the current <see cref="SvgShape"/>.
        /// </summary>
        public Point Location
        {
            get
            {
                return this.location;
            }
            set
            {
                this.location = value;
            }
        }
    }
}
