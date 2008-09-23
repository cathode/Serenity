using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity.Web.Forms
{
    /// <summary>
    /// Event data for the <see cref="Control.PreRender"/> event.
    /// </summary>
    public sealed class RenderEventArgs : EventArgs
    {
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the <see cref="PreRenderEventArgs"/> class.
        /// </summary>
        /// <param name="context"></param>
        public RenderEventArgs(RenderingContext context)
        {
            this.context = context;
        }
        #endregion
        #region Fields - Private
        private readonly RenderingContext context;
        #endregion
        #region Properties - Public
        public RenderingContext Context
        {
            get
            {
                return this.context;
            }
        }
        #endregion
    }
}
