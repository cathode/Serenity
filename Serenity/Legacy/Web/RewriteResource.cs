/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright � 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity.Web
{
    /// <summary>
    /// Represents a link or redirection to another resource.
    /// </summary>
    public sealed class RewriteResource : Resource
    {
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the RewriteResource class.
        /// </summary>
        /// <param name="target"></param>
        public RewriteResource(Uri target)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }
            this.target = target;
        }
        #endregion
        #region Fields - Private
        private bool isHardRewrite;
        private Uri target;
        #endregion
        #region Methods - Public
        public override void OnRequest(Request request, Response response)
        {
            if (this.IsHardRewrite)
            {
                this.TargetResource.OnRequest(request, response);
            }
            else
            {
                response.Headers.Add("Location", this.target.ToString());
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
        /// Gets the target resource.
        /// </summary>
        public Resource TargetResource
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        #endregion
    }
}
