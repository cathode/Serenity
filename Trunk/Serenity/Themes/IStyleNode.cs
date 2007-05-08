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
    /// Defines method signatures shared by different parts of the Style DOM.
    /// </summary>
    public interface IStyleNode
    {
        /// <summary>
        /// When implemented in an inheriting class, restores the style node to it's default state.
        /// </summary>
        void Undefine();
        /// <summary>
        /// When implemented in an inheriting class, gets a boolean value that indicates
        /// if the current style node has had any part of it be defined yet.
        /// </summary>
        bool IsDefined
        {
            get;
        }
    }
}
