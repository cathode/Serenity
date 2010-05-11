using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity.Web.Forms.Svg
{
    /// <summary>
    /// Represents a control that renders SVG (Scalable Vector Graphics) markup.
    /// </summary>
    public class SvgDocument : Document
    {
        #region Constructors - Public
        public SvgDocument()
        {
        }
        #endregion
        #region Properties - Protected
        protected override string DefaultName
        {
            get
            {
                return "svg";
            }
        }
        #endregion
    }
}
