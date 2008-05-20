using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Serenity.Web.Resources;

namespace Serenity.Web.Controls
{
    public abstract class DocumentResource : DynamicResource
    {
        #region Methods - Public
        public sealed override void OnRequest(CommonContext context)
        {
            base.OnRequest(context);


        }
        #endregion
        #region Properties - Protected
        protected abstract Document Document
        {
            get;
        }
        #endregion
    }
}
