/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

using Serenity.Web;

namespace Serenity.Web.Resources
{
    /// <summary>
    /// Represents a requestable file embedded in a module assembly file.
    /// </summary>
    public sealed class ResourceResource : Resource
    {
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceResource"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public ResourceResource(string name, byte[] data)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            this.Name = name;
            this.data = data;
            this.ContentType = ResourceResource.GetMimeType(name);
        }
        #endregion
        #region Fields - Private
        private readonly byte[] data;
        #endregion
        #region Methods - Public
        private static MimeType GetMimeType(string name)
        {
            string ext = System.IO.Path.GetExtension(name);
            switch (ext)
            {
                case ".png":
                    return MimeType.ImagePng;

                default:
                    return MimeType.Default;
            }
        }
        public override void OnRequest(Request request, Response response)
        {
            response.Write(this.data);
            response.Status = StatusCode.Http200Ok;
            response.ContentType = this.ContentType;
        }
        #endregion
        #region Properties - Public
        public override ResourceGrouping Grouping
        {
            get
            {
                return ResourceGrouping.Resources;
            }
        }
        public override bool IsSizeKnown
        {
            get
            {
                return true;
            }
        }
        public override int Size
        {
            get
            {
                return this.data.Length;
            }
        }
        #endregion
    }
}
