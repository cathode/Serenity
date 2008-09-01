/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Permissive License (Ms-PL), a copy of which should have been included with *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
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
