using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Serenity.Web.Forms;

namespace Serenity.Web.Resources
{
    public class DirectoryControl : Division
    {
        public DirectoryControl()
        {
            this.location = new TextControl();
        }
        #region Fields
        private TextControl location;
        #endregion
        #region Methods
        public override void OnPreRender(RenderingContext context)
        {
            base.OnPreRender(context);

            
        }
        #endregion
        #region Properties
        protected override string DefaultName
        {
            get
            {
                return "directory";
            }
        }
        public string Location
        {
            get
            {
                return this.location.Content;
            }
            set
            {
                this.location.Content = value;
            }
        }
        #endregion
    }
}
