using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity.Web.Forms.Svg
{
    /// <summary>
    /// Represents a rectangular shape.
    /// </summary>
    public class SvgRectangle : SvgShape
    {
        #region Fields - Private
        private int x;
        private int y;
        private int rx;
        private int ry;
        #endregion
        #region Properties - Protected
        protected override string DefaultName
        {
            get
            {
                return "rect";
            }
        }
        #endregion
        #region Properties - Public
        public int X
        {
            get
            {
                
            }
        }
        #endregion
    }
}
