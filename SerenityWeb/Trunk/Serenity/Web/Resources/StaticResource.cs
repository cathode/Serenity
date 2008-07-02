/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using Serenity.Web;

namespace Serenity.Web.Resources
{
    /// <summary>
    /// Represents a file on the local filesystem that is exposed as a requestable resource.
    /// </summary>
    public sealed class StaticResource : Resource
    {
        #region Fields - Private
        private string location;
        #endregion
        #region Methods - Public
        public override void OnRequest(Request request, Response response)
        {
            if (File.Exists(this.location))
            {
                //TODO: Fix this so it works with large files.
                response.Write(File.ReadAllBytes(this.location));
            }
            else
            {
                ErrorHandler.Handle(StatusCode.Http404NotFound);
            }
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Overridden. Returns ResourceGrouping.Files.
        /// </summary>
        public override ResourceGrouping Grouping
        {
            get
            {
                return ResourceGrouping.Files;
            }
        }
        /// <summary>
        /// Gets the local filesystem location which the current StaticResource represents.
        /// </summary>
        public string Location
        {
            get
            {
                return this.location;
            }
            internal set
            {
                this.location = value;
            }
        }
        /// <summary>
        /// Overridden. Returns true, indicating the size of a static file can be known.
        /// </summary>
        public override bool IsSizeKnown
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        /// Overridden. Gets the size of the file represented by the current StaticResource.
        /// </summary>
        public override int Size
        {
            get
            {
                return (int)new FileInfo(this.location).Length;
            }
        }
        #endregion
    }
}
