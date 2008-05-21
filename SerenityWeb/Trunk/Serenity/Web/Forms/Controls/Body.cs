using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity.Web.Forms.Controls
{
    public class Body : Control
    {
        #region Constructors - Public
        public Body()
        {
        }
        public Body(params Control[] controls)
            : base(controls)
        {
        }
        #endregion
        #region Properties - Protected
        protected override string DefaultName
        {
            get
            {
                return "body";
            }
        }
        #endregion
    }
}
