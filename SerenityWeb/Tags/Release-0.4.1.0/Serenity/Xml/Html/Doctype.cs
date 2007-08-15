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

namespace Serenity.Xml.Html
{
    /// <summary>
    /// Provides object-oriented access to a Document Type Declaration
    /// that should appear in any SGML-based document, e.g. HTML, SVG, MathML, etc.
    /// This class cannot be inherited.
    /// </summary>
    public sealed class Doctype
	{
		#region Constructors - Public
		/// <summary>
        /// Initializes a new instance of the Doctype class
        /// </summary>
        /// <param name="RootElement">Defines the root element that the current Doctype applies to.</param>
        /// <param name="PublicIdentifier">Defines the public identifier of the current Doctype.</param>
        /// <param name="SystemIdentifier">Defines the system identifier of the current Doctype.</param>
        public Doctype(string rootElement, string publicIdentifier, string systemIdentifier)
        {
            this.rootElement = rootElement;
            this.publicIdentifier = publicIdentifier;
            this.systemIdentifier = systemIdentifier;
		}
		#endregion
		#region Fields - Private
		private string rootElement;
		private string publicIdentifier;
		private string systemIdentifier;
		#endregion
		#region Methods - Public
		/// <summary>
        /// Returns the string representation of the current Doctype.
        /// </summary>
        /// <returns>The string representation of the current Doctype.</returns>
        public override string ToString()
        {
            return "<!DOCTYPE "
                + this.rootElement
                + " PUBLIC \""
                + publicIdentifier
                + "\" \""
                + this.systemIdentifier
                + "\">";
		}
		#endregion
		#region Properties - Public
		/// <summary>
        /// Gets the HTML 4.01 Frameset Doctype.
        /// </summary>
        public static Doctype HTML401Frameset
        {
            get
            {
                return new Doctype("html",
                    "-//W3C//DTD HTML 4.01 Frameset//EN",
                    "http://www.w3.org/TR/html4/frameset.dtd");
            }
        }
        /// <summary>
        /// Gets the HTML 4.01 Strict Doctype.
        /// </summary>
        public static Doctype HTML401Strict
        {
            get
            {
                return new Doctype("html",
                    "-//W3C//DTD HTML 4.01//EN",
                    "http://www.w3.org/TR/html4/strict.dtd");
            }
        }
        /// <summary>
        /// Gets the HTML 4.01 Transitional Doctype.
        /// </summary>
        public static Doctype HTML401Transitional
        {
            get
            {
                return new Doctype("html",
                    "-//W3C//DTD HTML 4.01 Transitional//EN",
                    "http://www.w3.org/TR/html4/loose.dtd");
            }
        }
        /// <summary>
        /// Gets the MathML 2.0 Doctype.
        /// </summary>
        public static Doctype MathML20
        {
            get
            {
                return new Doctype("math",
                    "-//W3C//DTD MathML 2.0//EN",
                    "http://www.w3.org/TR/MathML2/dtd/mathml2.dtd");
            }
        }
        /// <summary>
        /// Gets or sets the Public Identifier portion of the current Doctype.
        /// </summary>
        public string PublicIdentifier
        {
            get
            {
                return this.publicIdentifier;
            }
        }
        /// <summary>
        /// Gets or sets the Root Element portion of the current Doctype.
        /// </summary>
        public string RootElement
        {
            get
            {
                return this.rootElement;
            }
            set
            {
                this.rootElement = value;
            }
        }
        /// <summary>
        /// Gets the SVG (Scalable Vector Graphics) 1.0 Doctype.
        /// </summary>
        public static Doctype SVG10
        {
            get
            {
                return new Doctype("svg",
                    "-//W3C//DTD SVG 1.0//EN",
                    "http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd");
            }
        }
        /// <summary>
        /// Gets the SVG (Scalable Vector Graphics) 1.1 Basic Doctype.
        /// </summary>
        public static Doctype SVG11Basic
        {
            get
            {
                return new Doctype("svg",
                    "-//W3C//DTD SVG 1.1 Basic//EN",
                    "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11-basic.dtd");
            }
        }
        /// <summary>
        /// Gets the SVG (Scalable Vector Graphics) 1.1 Full Doctype.
        /// </summary>
        public static Doctype SVG11Full
        {
            get
            {
                return new Doctype("svg",
                    "-//W3C//DTD SVG 1.1//EN",
                    "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd");
            }
        }
        /// <summary>
        /// Gets the SVG (Scalable Vector Graphics) 1.1 Tiny Doctype.
        /// </summary>
        public static Doctype SVG11Tiny
        {
            get
            {
                return new Doctype("svg",
                    "-//W3C//DTD SVG 1.1 Tiny//EN",
                    "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11-tiny.dtd");
            }
        }
        /// <summary>
        /// Gets or sets the System Identifier portion of the current Doctype.
        /// </summary>
        public string SystemIdentifier
        {
            get
            {
                return this.systemIdentifier;
            }
        }
        /// <summary>
        /// Gets the XHTML 1.0 Frameset Doctype.
        /// </summary>
        public static Doctype XHTML10Frameset
        {
            get
            {
                return new Doctype("html",
                    "-//W3C//DTD XHTML 1.0 Frameset//EN",
                    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-frameset.dtd");
            }
        }
        /// <summary>
        /// Gets the XHTML 1.0 Strict Doctype.
        /// </summary>
        public static Doctype XHTML10Strict
        {
            get
            {
                return new Doctype("html",
                    "-//W3C//DTD XHTML 1.0 Strict//EN",
                    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd");
            }
        }
        /// <summary>
        /// Gets the XHTML 1.0 Transitional Doctype.
        /// </summary>
        public static Doctype XHTML10Transitional
        {
            get
            {
                return new Doctype("html",
                    "-//W3C//DTD XHTML 1.0 Transitional//EN",
                    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd");
            }
        }
        /// <summary>
        /// Gets the XHTML 1.1 Doctype. This is the reccomended doctype.
        /// </summary>
        public static Doctype XHTML11
        {
            get
            {
                return new Doctype("html",
                    "-//W3C//DTD XHTML 1.1//EN",
                    "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd");
            }
        }
        /// <summary>
        /// Gets the XHTML + MathML + SVG Doctype.
        /// </summary>
        public static Doctype XHTMLMathMLSVG
        {
            get
            {
                return new Doctype("html",
                    "-//W3C//DTD XHTML 1.1 plus MathML 2.0 plus SVG 1.1//EN",
                    "http://www.w3.org/2002/04/xhtml-math-svg/xhtml-math-svg.dtd");
            }
		}
		#endregion
    }
}
