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
    /// Represents the type of a border line.
    /// </summary>
    public enum BorderType
    {
        /// <summary>
        /// Defines no border.
        /// </summary>
        None,
        /// <summary>
        /// The same as BorderType.None, except in border conflict resolution for table elements
        /// </summary>
        Hidden,
        /// <summary>
        /// Defines a dotted border.
        /// </summary>
        Dotted,
        /// <summary>
        /// Defines a dashed border.
        /// </summary>
        Dashed,
        /// <summary>
        /// Defines a solid border.
        /// </summary>
        Solid,
        /// <summary>
        /// Defines two borders. The width of the two borders are the same as the border-width value.
        /// </summary>
        Double,
        /// <summary>
        /// Defines a 3D grooved border. The effect depends on the border-color value.
        /// </summary>
        Groove,
        /// <summary>
        /// Defines a 3D ridged border. The effect depends on the border-color value.
        /// </summary>
        Ridge,
        /// <summary>
        /// Defines a 3D inset border. The effect depends on the border-color value.
        /// </summary>
        Inset,
        /// <summary>
        /// Defines a 3D outset border. The effect depends on the border-color value.
        /// </summary>
        Outset,
    }
}
