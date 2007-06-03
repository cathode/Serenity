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

namespace Serenity.Themes
{
    /// <summary>
    /// Represents the measurement types of width or distance in a Style.
    /// </summary>
    public enum Unit
    {
        /// <summary>
        /// Width will be a number of pixels.
        /// </summary>
        Pixels,
        /// <summary>
        /// Width will be a percentage of the parent container's size.
        /// </summary>
        Percentage,
        /// <summary>
        /// Width will be a number of inches.
        /// </summary>
        Inches,
        /// <summary>
        /// Width will be a number of centimeters.
        /// </summary>
        Centimeters,
        /// <summary>
        /// Width will be a number of millimeters.
        /// </summary>
        Millimeters,
        /// <summary>
        /// Width will be a number of points.
        /// </summary>
        Points,
    }
}
