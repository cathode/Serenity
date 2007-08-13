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
using System.IO;
using System.Text;

using Serenity.Xml;
using Serenity.Xml.Html;

namespace Serenity.Themes
{
    /// <summary>
    /// Represents a universal theme which can be used to give dynamic content
    /// a different appearance based on the current environment.
    /// </summary>
    public sealed class Theme : Multiton<string, Theme>
    {
        #region Constructors - Public
        public Theme(string name) : base(name.ToLower())
        {
            this.stylesheetContent = null;
            this.styles = new Dictionary<StyleType, Style>();
            this.styles[StyleType.AccentA] = new Style("AccentA");
            this.styles[StyleType.AccentB] = new Style("AccentB");
            this.styles[StyleType.AccentC] = new Style("AccentC");
            this.styles[StyleType.AccentD] = new Style("AccentD");
            this.styles[StyleType.AccentE] = new Style("AccentE");
            this.styles[StyleType.AccentF] = new Style("AccentF");
            this.styles[StyleType.BlockText] = new Style("BlockText");
            this.styles[StyleType.ContentA] = new Style("ContentA");
            this.styles[StyleType.ContentB] = new Style("ContentB");
            this.styles[StyleType.HeadingA] = new Style("HeadingA");
            this.styles[StyleType.HeadingB] = new Style("HeadingB");
            this.styles[StyleType.HeadingC] = new Style("HeadingC");
            this.styles[StyleType.HeadingD] = new Style("HeadingD");
            this.styles[StyleType.HeadingE] = new Style("HeadingE");
            this.styles[StyleType.HeadingF] = new Style("HeadingF");
            this.styles[StyleType.Link] = new Style("Link");
            this.styles[StyleType.LinkExternal] = new Style("LinkExternal");

            this.AccentA.BaseStyle = this.ContentA;
            this.AccentB.BaseStyle = this.AccentA;
            this.AccentC.BaseStyle = this.AccentB;
            this.AccentD.BaseStyle = this.AccentC;
            this.AccentE.BaseStyle = this.AccentD;
            this.AccentF.BaseStyle = this.AccentE;
            this.BlockText.BaseStyle = this.ContentA;
            this.ContentB.BaseStyle = this.ContentA;
            this.HeadingA.BaseStyle = this.ContentA;
            this.HeadingB.BaseStyle = this.HeadingA;
            this.HeadingC.BaseStyle = this.HeadingB;
            this.HeadingD.BaseStyle = this.HeadingC;
            this.HeadingE.BaseStyle = this.HeadingD;
            this.HeadingF.BaseStyle = this.HeadingE;
            this.Link.BaseStyle = this.ContentA;
            this.LinkExternal.BaseStyle = this.Link;
        }
        #endregion
        #region Fields - Private
        private Dictionary<StyleType, Style> styles;
        private byte[] stylesheetContent;
        #endregion
        #region Methods - Public
        public void Clear()
        {
            foreach (Style style in this.styles.Values)
            {
                style.Undefine();
            }
        }
        /// <summary>
        /// Loads a theme.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Theme LoadTheme(string name)
        {
            if (Theme.ContainsInstance(name) == true)
            {
                return Theme.GetInstance(name);
            }
            else
            {
                XmlDocument Doc = new XmlDocument();
                Doc.LoadFile(SPath.GetThemeFile(name));

                Theme theme = new Theme(Doc.DocumentElement.GetAttributeValue("Name"));
                IEnumerable<XmlElement> Styles = ((XmlElement)Doc.DocumentElement["Styles"]).GetElementsByName("Style");
                foreach (XmlElement Style in Styles)
                {
                    Style style = theme.styles[(StyleType)Enum.Parse(typeof(StyleType), Style.GetAttributeValue("Class"))];

                    XmlElement ColorElement = (XmlElement)(Style["Color"]);
                    if (ColorElement != null)
                    {
                        style.TextColor.Value = ColorElement.GetAttributeValue("Text");
                        string backgroundColor = ColorElement.GetAttributeValue("Background");
                        if (string.IsNullOrEmpty(backgroundColor) == false)
                        {
                            style.BackgroundColor.Value = backgroundColor;
                        }
                    }
                    XmlElement custom = (XmlElement)(Style["Custom"]);
                    if (custom != null)
                    {
                        style.Custom = custom.InnerText;
                    }
                }
                return theme;
            }
        }
        /// <summary>
        /// Generates and caches the stylesheet output of the current Theme.
        /// </summary>
        public void GenerateStylesheet()
        {
            StringBuilder output = new StringBuilder();
            foreach (Style style in this.styles.Values)
            {
                if (style.QualifiedIsDefined == true)
                {
                    output.Append("." + style.Class + "\r\n{\r\n");
                    if (style.QualifiedTextColor.IsDefined == true)
                    {
                        output.Append("\tcolor: #" + style.QualifiedTextColor.Value + ";\r\n");
                    }
                    if (style.QualifiedBackgroundColor.IsDefined == true)
                    {
                        output.Append("\tbackground-color: #" + style.QualifiedBackgroundColor.Value + ";\r\n");
                    }
                    if (style.QualifiedMargin.IsDefined == true)
                    {
                        Margin margin = style.QualifiedMargin;
                        if (margin.Top.IsDefined == true)
                        {
                            if (margin.Top.Auto == true)
                            {
                                output.Append("\tmargin-top: auto;\r\n");
                            }
                            else
                            {
                                output.Append("\tmargin-top: "
                                    + margin.Top.Value.ToString()
                                    + Theme.ConvertUnit(margin.Top.Unit)
                                    + ";\r\n");
                            }
                        }
                        if (margin.Bottom.IsDefined == true)
                        {
                            if (margin.Bottom.Auto == true)
                            {
                                output.Append("\tmargin-bottom: auto;\r\n");
                            }
                            else
                            {
                                output.Append("\tmargin-bottom: "
                                    + margin.Bottom.Value.ToString()
                                    + Theme.ConvertUnit(margin.Bottom.Unit)
                                    + ";\r\n");
                            }
                        }
                        if (margin.Left.IsDefined == true)
                        {
                            if (margin.Left.Auto == true)
                            {
                                output.Append("\tmargin-left: auto;\r\n");
                            }
                            else
                            {
                                output.Append("\tmargin-left: "
                                    + margin.Left.Value.ToString()
                                    + Theme.ConvertUnit(margin.Left.Unit)
                                    + ";\r\n");
                            }
                        }
                        if (margin.Right.IsDefined == true)
                        {
                            if (margin.Right.Auto == true)
                            {
                                output.Append("\tmargin-right: auto;\r\n");
                            }
                            else
                            {
                                output.Append("\tmargin-right: "
                                    + margin.Right.Value.ToString()
                                    + Theme.ConvertUnit(margin.Right.Unit)
                                    + ";\r\n");
                            }
                        }
                    }
                    if (style.QualifiedPadding.IsDefined == true)
                    {
                        Padding padding = style.QualifiedPadding;
                        if (padding.Top.IsDefined == true)
                        {
                            if (padding.Top.Auto == true)
                            {
                                output.Append("\tpadding-top: auto;\r\n");
                            }
                            else
                            {
                                output.Append("\tpadding-top: ");
                                output.Append(padding.Top.Value.ToString());
                                output.Append(Theme.ConvertUnit(padding.Top.Unit));
                                output.Append(";\r\n");
                            }
                        }
                        if (padding.Bottom.IsDefined == true)
                        {
                            if (padding.Bottom.Auto == true)
                            {
                                output.Append("\tpadding-bottom: auto;\r\n");
                            }
                            else
                            {
                                output.Append("\tpadding-bottom: ");
                                output.Append(padding.Bottom.Value.ToString());
                                output.Append(Theme.ConvertUnit(padding.Bottom.Unit));
                                output.Append(";\r\n");
                            }
                        }
                        if (padding.Left.IsDefined == true)
                        {
                            if (padding.Left.Auto == true)
                            {
                                output.Append("\tpadding-left: auto;\r\n");
                            }
                            else
                            {
                                output.Append("\tpadding-left: ");
                                output.Append(padding.Left.Value.ToString());
                                output.Append(Theme.ConvertUnit(padding.Left.Unit));
                                output.Append(";\r\n");
                            }
                        }
                        if (padding.Right.IsDefined == true)
                        {
                            if (padding.Right.Auto == true)
                            {
                                output.Append("\tpadding-right: auto;\r\n");
                            }
                            else
                            {
                                output.Append("\tpadding-right: ");
                                output.Append(padding.Right.Value.ToString());
                                output.Append(Theme.ConvertUnit(padding.Right.Unit));
                                output.Append(";\r\n");
                            }
                        }
                    }
                    if (style.QualifiedBorder.IsDefined)
                    {
                        Border border = style.QualifiedBorder;
                        if (border.Top.IsDefined == true)
                        {
                            BorderSide side = border.Top;
                            output.Append("\tborder-top-style: "
                                + Theme.ConvertBorderStyle(side.BorderType) + ";\r\n");
                            if (side.Color.IsDefined == true)
                            {
                                output.Append("\tborder-top-color: #" + side.Color.Value + ";\r\n");
                            }
                            if (side.Width.IsDefined == true)
                            {
                                output.Append("\tborder-top-width: "
                                    + side.Width.Value
                                    + Theme.ConvertUnit(side.Width.Unit)
                                    + ";\r\n");
                            }
                        }
                        if (border.Bottom.IsDefined == true)
                        {
                            BorderSide side = border.Bottom;
                            output.Append("\tborder-bottom-style: "
                                + Theme.ConvertBorderStyle(side.BorderType) + ";\r\n");
                            if (side.Color.IsDefined == true)
                            {
                                output.Append("\tborder-bottom-color: #" + side.Color.Value + ";\r\n");
                            }
                            if (side.Width.IsDefined == true)
                            {
                                output.Append("\tborder-bottom-width: "
                                    + side.Width.Value
                                    + Theme.ConvertUnit(side.Width.Unit)
                                    + ";\r\n");
                            }
                        }
                        if (border.Left.IsDefined == true)
                        {
                            BorderSide side = border.Left;
                            output.Append("\tborder-left-style: "
                                + Theme.ConvertBorderStyle(side.BorderType) + ";\r\n");
                            if (side.Color.IsDefined == true)
                            {
                                output.Append("\tborder-left-color: #" + side.Color.Value + ";\r\n");
                            }
                            if (side.Width.IsDefined == true)
                            {
                                output.Append("\tborder-left-width: "
                                    + side.Width.Value
                                    + Theme.ConvertUnit(side.Width.Unit)
                                    + ";\r\n");
                            }
                        }
                        if (border.Right.IsDefined == true)
                        {
                            BorderSide side = border.Right;
                            output.Append("\tborder-right-style: "
                                + Theme.ConvertBorderStyle(side.BorderType) + ";\r\n");
                            if (side.Color.IsDefined == true)
                            {
                                output.Append("\tborder-right-color: #" + side.Color.Value + ";\r\n");
                            }
                            if (side.Width.IsDefined == true)
                            {
                                output.Append("\tborder-right-width: "
                                    + side.Width.Value
                                    + Theme.ConvertUnit(side.Width.Unit)
                                    + ";\r\n");
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(style.Custom) == false)
                    {
                        output.Append("\t" + style.Custom.Trim() + "\r\n");
                    }
                    output.Append("}\r\n");
                }
            }
            this.stylesheetContent = Encoding.UTF8.GetBytes(output.ToString());
        }
        public static string ConvertBorderStyle(BorderType type)
        {
            switch (type)
            {
                default:
                    return type.ToString().ToLower();
            }
        }
        public static string ConvertUnit(Unit unit)
        {
            switch (unit)
            {
                case Unit.Centimeters:
                    return "cm";
                case Unit.Inches:
                    return "in";
                case Unit.Millimeters:
                    return "mm";
                case Unit.Percentage:
                    return "%";
                case Unit.Pixels:
                    return "px";
                case Unit.Points:
                    return "pt";
                default:
                    return "px";
            }
        }
        #endregion
        #region Properties - Public
        public Style AccentA
        {
            get
            {
                return this.styles[StyleType.AccentA];
            }
        }
        public Style AccentB
        {
            get
            {
                return this.styles[StyleType.AccentB];
            }
        }
        public Style AccentC
        {
            get
            {
                return this.styles[StyleType.AccentC];
            }
        }
        public Style AccentD
        {
            get
            {
                return this.styles[StyleType.AccentD];
            }
        }
        public Style AccentE
        {
            get
            {
                return this.styles[StyleType.AccentE];
            }
        }
        public Style AccentF
        {
            get
            {
                return this.styles[StyleType.AccentF];
            }
        }
        public Style[] AllStyles
        {
            get
            {
                Style[] allStyles = new Style[this.styles.Count];
                this.styles.Values.CopyTo(allStyles, 0);
                return allStyles;
            }
        }
        public Style BlockText
        {
            get
            {
                return this.styles[StyleType.BlockText];
            }
        }
        public Style ContentA
        {
            get
            {
                return this.styles[StyleType.ContentA];
            }
        }
        public Style ContentB
        {
            get
            {
                return this.styles[StyleType.ContentB];
            }
        }
        public Style HeadingA
        {
            get
            {
                return this.styles[StyleType.HeadingA];
            }
        }
        public Style HeadingB
        {
            get
            {
                return this.styles[StyleType.HeadingB];
            }
        }
        public Style HeadingC
        {
            get
            {
                return this.styles[StyleType.HeadingC];
            }
        }
        public Style HeadingD
        {
            get
            {
                return this.styles[StyleType.HeadingD];
            }
        }
        public Style HeadingE
        {
            get
            {
                return this.styles[StyleType.HeadingE];
            }
        }
        public Style HeadingF
        {
            get
            {
                return this.styles[StyleType.HeadingF];
            }
        }
        public Style Link
        {
            get
            {
                return this.styles[StyleType.Link];
            }
        }
        public Style LinkExternal
        {
            get
            {
                return this.styles[StyleType.LinkExternal];
            }
        }
        public byte[] StylesheetContent
        {
            get
            {
                if (this.stylesheetContent == null)
                {
                    this.GenerateStylesheet();
                }
                return this.stylesheetContent;
            }
        }
        public string StylesheetUrl
        {
            get
            {
                return  "/Theme/Stylesheet";
            }
        }
        /// <summary>
        /// Gets a specific style of the current Theme.
        /// </summary>
        /// <param name="type">The StyleType specifying what Style to get.</param>
        /// <returns>The requested Style.</returns>
        public Style GetStyle(StyleType type)
        {
            return this.styles[type];
        }
        #endregion
    }
}