/*
Serenity - The next evolution of web server technology

Copyright © 2006-2007 Serenity Project (http://serenityproject.net/)

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
    public sealed class DriverInfo
    {
        #region Constructors - Internal
        internal DriverInfo()
        {
            this._Class = "unknown";
            this._SupportedVersion = new Version();
            this._Type = "unknown";
            this._UriSchema = "unknown";
        }
        #endregion
        #region Fields - Private
        private string _Class;
        private string _Type;
        private string _UriSchema;
        private Version _SupportedVersion;
        #endregion
        #region Methods - Internal
        internal DriverInfo(string Class, string Type, string UriSchema, Version SupportedVersion)
        {
            this._Class = Class;
            this._Type = Type;
            this._UriSchema = UriSchema;
            this._SupportedVersion = SupportedVersion;
        }
        #endregion
        #region Properties - Public
        public string Class
        {
            get
            {
                return this._Class;
            }
            internal set
            {
                this._Class = value;
            }
        }
        public string Type
        {
            get
            {
                return this._Type;
            }
            internal set
            {
                this._Type = value;
            }
        }
        public string UriSchema
        {
            get
            {
                return this._UriSchema;
            }
            internal set
            {
                this._UriSchema = value;
            }
        }
        public Version SupportedVersion
        {
            get
            {
                return this._SupportedVersion;
            }
            internal set
            {
                this._SupportedVersion = value;
            }
        }
        #endregion
    }
}
