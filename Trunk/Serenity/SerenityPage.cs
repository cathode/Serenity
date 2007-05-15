/*
Serenity - The next evolution of web server technology

Copyright © 2006-2007 Serenity Project (http://SerenityProject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
using System;
using System.Collections.Generic;
using System.Text;

using Serenity.Web;

namespace Serenity
{
    /// <summary>
    /// Represents a dynamic page which generates content on the fly.
    /// </summary>
    public abstract class SerenityPage 
    {
        #region Methods - Public
        public abstract SerenityPage CreateSafeInstanceHelper();
        public abstract void OnInitialization();
        public abstract void OnRequest(CommonContext context);
        public abstract void OnShutdown();
        #endregion
        #region Properties - Protected
        [Obsolete]
        protected abstract string NameHelper
        {
            get;
        }
        #endregion
        #region Properties - Public
        public string Name
        {
            get
            {
                return this.NameHelper.ToLower();
            }
        }
        
        public virtual string Title
        {
            get
            {
                return this.NameHelper;
            }
        }
        #endregion
    }
}
