using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Serenity.Web.Forms
{
    /// <summary>
    /// Provides a control that renders a target URL and creates links to each segment of the target URL.
    /// </summary>
    public class PathLink : Control
    {
        #region Constructors - Public
        public PathLink()
        {
        }
        #endregion
        #region Fields - Private
        private Uri target;
        #endregion
        #region Methods - Protected
        protected override void RenderBegin(RenderingContext context)
        {
            StreamWriter writer = new StreamWriter(context.OutputStream, context.OutputEncoding);

            writer.Write("<span>");

            if (this.ShowTargetHostname)
            {
                if (this.ShowTargetSchema)
                {
                    writer.Write(this.Target.Scheme + "://");
                }
                writer.Write(this.Target.Host);
            }
            string s = "/";

            writer.Write("/ ");
            var segments = (this.ShowTargetHostname) ? this.Target.Segments : this.Target.Segments.Skip(1);
            foreach (string seg in segments)
            {
                s += seg;

                Anchor anchor = new Anchor(new Uri(this.Target.Scheme + "://" + this.Target.Host + s),
                    seg.Trim('/'));
                writer.Flush();
                anchor.Render(context);

                writer.Write(" / ");
            }

            writer.Flush();
        }
        protected override void RenderEnd(RenderingContext context)
        {
            StreamWriter writer = new StreamWriter(context.OutputStream, context.OutputEncoding);

            writer.Write("</span>");
            writer.Flush();
        }
        #endregion
        #region Properties - Protected
        protected override bool CanContainControls
        {
            get
            {
                return false;
            }
        }
        #endregion
        #region Properties - Public
        public Uri Target
        {
            get
            {
                return this.target;
            }
            set
            {
                this.target = value;
            }
        }
        public bool ShowTargetSchema
        {
            get
            {
                return false;
            }
        }
        public bool ShowTargetHostname
        {
            get
            {
                return false;
            }
        }
        #endregion
    }
}
