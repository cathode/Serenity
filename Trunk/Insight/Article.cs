/*
Insight - The Intelligent Wiki Engine

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

using Serenity;

namespace Insight
{
    /// <summary>
    /// Defines a base class for all articles (including special/system articles)
    /// </summary>
    internal abstract class Article
    {
        /// <summary>
        /// Gets or sets the Name of the current Article.
        /// </summary>
        internal string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this._Name = value;
            }
        }
        private string _Name;
    }
}
