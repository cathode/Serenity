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

namespace Serenity.Xml.Html
{
    /// <summary>
    /// Provides object-oriented access to a Document Type Declaration
    /// that should appear in any SGML-based document, e.g. HTML, SVG, MathML, etc.
    /// This class cannot be inherited.
    /// </summary>
    public sealed class Doctype
    {
        /// <summary>
        /// Initializes a new instance of the Doctype class
        /// </summary>
        /// <param name="RootElement">Defines the root element that the current Doctype applies to.</param>
        /// <param name="PublicIdentifier">Defines the public identifier of the current Doctype.</param>
        /// <param name="SystemIdentifier">Defines the system identifier of the current Doctype.</param>
        public Doctype(string RootElement, string PublicIdentifier, string SystemIdentifier)
        {
            this._RootElement = RootElement;
            this._PublicIdentifier = PublicIdentifier;
            this._SystemIdentifier = SystemIdentifier;
        }
        /// <summary>
        /// Returns the string representation of the current Doctype.
        /// </summary>
        /// <returns>The string representation of the current Doctype.</returns>
        public override string ToString()
        {
            return "<!DOCTYPE "
                + this._RootElement
                + " PUBLIC \""
                + _PublicIdentifier
                + "\" \""
                + this._SystemIdentifier
                + "\">";
        }
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
                return this._PublicIdentifier;
            }
            set
            {
                this._PublicIdentifier = value;
            }
        }
        /// <summary>
        /// Gets or sets the Root Element portion of the current Doctype.
        /// </summary>
        public string RootElement
        {
            get
            {
                return this._RootElement;
            }
            set
            {
                this._RootElement = value;
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
                return this._SystemIdentifier;
            }
            set
            {
                this._SystemIdentifier = value;
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
        private string _RootElement;
        private string _PublicIdentifier;
        private string _SystemIdentifier;
    }
}
