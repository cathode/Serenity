using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity.Web.Forms
{
    /// <summary>
    /// Represents a web control that only contains text.
    /// </summary>
    public sealed class TextControl : Control
    {
        #region Constructors - Public
        public TextControl()
        {
        }
        public TextControl(string content)
        {
            this.content = content;
        }
        #endregion
        #region Fields - Private
        private string content;
        #endregion
        #region Methods - Protected
        protected override void RenderBegin(RenderingContext context)
        {
            if (string.IsNullOrEmpty(this.content))
            {
                return;
            }

            byte[] buf = context.OutputEncoding.GetBytes(this.content);
            context.OutputStream.Write(buf, 0, buf.Length);
        }
        protected override void RenderEnd(RenderingContext context)
        {
        }
        #endregion
        #region Properties - Protected
        protected override bool CanContainChildren
        {
            get
            {
                return false;
            }
        }
        #endregion
        #region Properties - Public
        public string Content
        {
            get
            {
                return this.content;
            }
            set
            {
                this.content = value;
            }
        }
        #endregion
    }
}
