/******************************************************************************
 * Serenity - Managed Web Application Server. ( http://gearedstudios.com/ )   *
 * Copyright © 2006-2011 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using Serenity.Web;

namespace Serenity.Web
{
    /// <summary>
    /// Represents a file on the local filesystem that is exposed as a resource.
    /// </summary>
    public sealed class StaticResource : Resource
    {
        #region Fields - Private
        private string location;
        #endregion
        #region Methods - Public
        public static StaticResource[] ScanDirectory(string path)
        {

        }
        public override void OnRequest(Request request, Response response)
        {
            if (File.Exists(this.location))
            {
                //TODO: Fix this so it works with large files.
                response.Write(File.ReadAllBytes(this.location));
            }
            else
            {
                //ErrorHandler.Handle(StatusCode.Http404NotFound);
            }
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets the local filesystem location which the current StaticResource represents.
        /// </summary>
        internal string Location
        {
            get
            {
                return this.location;
            }
            set
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
        public override long Size
        {
            get
            {
                return (int)new FileInfo(this.location).Length;
            }
        }
        #endregion
    }
}
