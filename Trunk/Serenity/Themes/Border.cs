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

namespace Serenity.Themes
{
    /// <summary>
    /// Represents the border properties of a Style.
    /// </summary>
    public sealed class Border : Box<BorderSide>
    {
        #region Constructors - Internal
        internal Border()
        {
            this.bottom = new BorderSide();
            this.left = new BorderSide();
            this.right = new BorderSide();
            this.top = new BorderSide();
        }
        #endregion
    }
}
