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

namespace Serenity.Web.Drivers
{
    /// <summary>
    /// Provides an type that WebDrivers can use to expose information about themselves.
    /// </summary>
    public sealed class DriverInfo
    {
        #region Constructors - Internal
        internal DriverInfo()
        {
            this.className = "unknown";
            this.version = new Version();
            this.type = "unknown";
            this.uriSchema = "unknown";
        }
        internal DriverInfo(string className, string typeName, string uriSchema, Version version)
        {
            this.className = className;
            this.type = typeName;
            this.uriSchema = uriSchema;
            this.version = version;
        }
        #endregion
        #region Fields - Private
        private string className;
        private string type;
        private string uriSchema;
        private Version version;
        #endregion
        #region Properties - Public
        public string ClassName
        {
            get
            {
                return this.className;
            }
            internal set
            {
                this.className = value;
            }
        }
        public string TypeName
        {
            get
            {
                return this.type;
            }
            internal set
            {
                this.type = value;
            }
        }
        public string UriSchema
        {
            get
            {
                return this.uriSchema;
            }
            internal set
            {
                this.uriSchema = value;
            }
        }
        public Version Version
        {
            get
            {
                return this.version;
            }
            internal set
            {
                this.version = value;
            }
        }
        #endregion
    }
}
