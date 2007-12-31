using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity.Resources
{
    /// <summary>
    /// Represents a link or redirection to another resource on the server.
    /// </summary>
    public sealed class RewriteResource : Resource
    {
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the RewriteResource class.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="target"></param>
        public RewriteResource(ResourcePath path, ResourcePath target)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            else if (target == null)
            {
                throw new ArgumentNullException("target");
            }
            this.Path = path;
            this.targetPath = target;
        }
        #endregion
        #region Fields - Private
        private ResourcePath targetPath;
        private bool isHardRewrite = true;
        private Resource targetResource = null;
        #endregion
        #region Methods - Private
        private void RebuildCache()
        {
            if (targetResource != null)
            {
                return;
            }

        }
        private void InvalidateCache()
        {
            this.targetResource = null;
        }
        #endregion
        #region Methods - Public
        public override void OnRequest(Serenity.Web.CommonContext context)
        {
            if (this.IsHardRewrite)
            {
                this.TargetResource.OnRequest(context);
            }
            else
            {
                context.Response.Headers.Add("Location", this.TargetPath.ToUriString());
            }
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets or sets a value which determines if the rewriting is a "hard" rewrite.
        /// where the rewrite is silently handled on the server side, or a "soft" rewrite,
        /// where a redirect response is sent back to the client.
        /// </summary>
        public bool IsHardRewrite
        {
            get
            {
                return this.isHardRewrite;
            }
            set
            {
                this.isHardRewrite = value;
            }
        }
        /// <summary>
        /// Gets or sets the path of the target Resource.
        /// </summary>
        public ResourcePath TargetPath
        {
            get
            {
                return this.targetPath;
            }
            set
            {
                this.InvalidateCache();

                this.targetPath = value;
            }
        }
        /// <summary>
        /// Gets the target resource.
        /// </summary>
        public Resource TargetResource
        {
            get
            {
                this.RebuildCache();

                return this.targetResource;
            }
        }
        #endregion
    }
}
