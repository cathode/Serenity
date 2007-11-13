/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity
{
    /// <summary>
    /// Represents a requestable file embedded in a module assembly file.
    /// </summary>
    public class ResourceResource : Resource
    {
        #region Constructors - Public
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
        }
        #endregion
        #region Fields - Private
        private readonly byte[] data;
        #endregion
        #region Methods - Public
        public override void OnRequest(Serenity.Web.CommonContext context)
        {
            context.Response.Write(this.data);
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
        #endregion
    }
}
