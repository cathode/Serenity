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
using System.IO;
using System.Text;

namespace Serenity.Xml
{
    /// <summary>
    /// Represents a parser for XML markup. This class cannot be inherited.
    /// </summary>
    public static class XmlReader
    {
        /// <summary>
        /// Reads the XML data located in a file.
        /// </summary>
        /// <param name="FilePath">The path to the file which will be read.</param>
        /// <returns></returns>
        public static XmlNodeCollection<XmlNode> ReadFile(string FilePath)
        {
            if (File.Exists(FilePath) == true)
            {
                return XmlReader.ReadMarkup(File.ReadAllText(FilePath));
            }
            else
            {
                throw new FileNotFoundException("The specified XML file was not found", FilePath);
            }
        }
        /// <summary>
        /// Attempts to read the markup contained in the specified file.
        /// </summary>
        /// <param name="path">The location of the file to read from.</param>
        /// <returns></returns>
        public static TryResult<XmlNodeCollection<XmlNode>> TryReadFile(string path)
        {
            try
            {
                return TryResult<XmlNodeCollection<XmlNode>>.SuccessResult(XmlReader.ReadFile(path));
            }
            catch (Exception E)
            {
                return TryResult<XmlNodeCollection<XmlNode>>.FailResult(E);
            }
        }
        /// <summary>
        /// Processes a string of XML markup and returns an XmlNodeCollection&lt;XmlNode&gt;
        /// containing XML nodes present in the markup.
        /// </summary>
        /// <param name="markup">The XML markup to use as input.</param>
        /// <returns>An XmlNodeCollection&lt;XmlNode&gt; containing nodes present in the markup.</returns>
        public static XmlNodeCollection<XmlNode> ReadMarkup(string markup)
        {
            XmlNodeCollection<XmlNode> Nodes = new XmlNodeCollection<XmlNode>();
            string Remaining = markup;
            while (Remaining.Length > 0)
            {
                Nodes.Add(XmlReader.ReadNextNode(Remaining, out Remaining));
            }
            return Nodes;
        }
        /// <summary>
        /// Attempts to parse the markup in the specified string.
        /// </summary>
        /// <param name="markup">A string containing the markup to parse.</param>
        /// <returns></returns>
        public static TryResult<XmlNodeCollection<XmlNode>> TryReadMarkup(string markup)
        {
            try
            {
                return TryResult<XmlNodeCollection<XmlNode>>.SuccessResult(XmlReader.ReadMarkup(markup));
            }
            catch (Exception e)
            {
                return TryResult<XmlNodeCollection<XmlNode>>.FailResult(e);
            }
        }
        /// <summary>
        /// Parses and returns the next available XmlNode described in outerMarkup.
        /// When this method returns, remainingMarkup contains a string containing the markup which was not part
        /// of the next available node.
        /// </summary>
        /// <param name="markup"></param>
        /// <param name="remaining">When this method returns, remainingMarkup contains
        /// any markup that was not part of the returned "next node".</param>
        /// <returns></returns>
        public static XmlNode ReadNextNode(string markup, out string remaining)
        {
            markup = markup.Trim();
            int i = -1;
            int n = -1;
            if (string.IsNullOrEmpty(markup) == true)
            {
                remaining = "";
                return null;
            }
            else
            {
                XmlNode Result = null;
                remaining = "";

                if (markup[0] == '<')
                {
                    //Start of an element
                    if (markup[1] == '!')
                    {
                        //Next node will be a comment, CDATA, or doctype.
                        if (markup[2] == '-')
                        {
                            //Comment.
                            if (markup[3] == '-')
                            {
                                i = markup.IndexOf("-->");
                                if (i != -1)
                                {
                                    Result = new XmlComment(markup.Substring(4, i - 4));
                                    remaining = markup.Substring(i + 3);
                                }
                                else
                                {
                                }
                            }
                            else
                            {
                            }
                        }
                        else if (markup[2] == '[')
                        {
                            //CDATA.
                            if (markup.StartsWith("<![CDATA[") == true)
                            {
                                i = markup.IndexOf("]]>");
                                if (i != -1)
                                {
                                    Result = new XmlCharacterData(markup.Substring(9, i - 9));
                                    remaining = markup.Substring(i + 3);
                                }
                            }
                            else
                            {
                            }
                        }
                        else
                        {
                            //Must be a doctype (or is bad markup).
                        }
                    }
                    else if (markup[1] == '?')
                    {
                        //Next node will be a preprocessor directive.
                        if (markup[2] != ' ')
                        {
                            i = markup.IndexOf(' ');
                            n = markup.IndexOf("?>");
                            string Name = markup.Substring(2, i - 2);
                            string Content = markup.Substring(i, n - i);
                            Result = new XmlPreprocessorDirective(Name, Content);
                            remaining = markup.Substring(n + 2);
                        }
                        else
                        {
                        }
                    }
                    else
                    {
                        //Next node will be a plain element.
                        n = markup.IndexOf('>');
                        if (n != -1)
                        {
                            XmlElement result = null;
                            string elementName = "";
                            string InnerMarkup = "";
                            i = markup.IndexOf(' ');
                            if ((i != -1) && (i < n))
                            {
                                //Has attributes
                                elementName = markup.Substring(1, markup.IndexOf(' ') - 1);
                                result = new XmlElement(elementName);
                                string attributeContent = "";
                                i = markup.IndexOf("/>");
                                if ((i != -1) && (i < n))
                                {
                                    remaining = markup.Substring(i + 2);
                                    attributeContent = markup.Substring(elementName.Length + 2,
                                        i - (elementName.Length + 2));
                                }
                                else
                                {
                                    string EndTag = string.Format("</{0}>", elementName);
                                    i = markup.IndexOf(EndTag);
                                    if (i != -1)
                                    {
                                        remaining = markup.Substring(i + EndTag.Length);
                                        attributeContent = markup.Substring(elementName.Length + 2,
                                            n - (elementName.Length + 2));
                                        InnerMarkup = markup.Substring(n + 1, i - (n + 1));
                                    }
                                    else
                                    {
                                        throw new Exception("Invalid markup encountered");
                                    }

                                }
                                while (string.IsNullOrEmpty(attributeContent) == false)
                                {
                                    attributeContent = attributeContent.Trim();
                                    string Name = "";
                                    string Value = "";
                                    i = attributeContent.IndexOf("=\"");
                                    if (i != -1)
                                    {
                                        Name = attributeContent.Substring(0, i);
                                        attributeContent = attributeContent.Substring(i + 2);
                                        i = attributeContent.IndexOf("\"");
                                        if (i != -1)
                                        {
                                            Value = attributeContent.Substring(0, i);
                                            attributeContent = attributeContent.Substring(i + 1);
                                            result.AppendAttribute(Name, Value);
                                        }
                                        else
                                        {
                                            throw new Exception("Invalid input when attempting to parse attributes");
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("Invalid input when attempting to parse attributes");
                                    }
                                }
                            }
                            else
                            {
                                //No attributes, but not an empty element.
                                elementName = markup.Substring(1, n - 1);

                                string EndTag = string.Format("</{0}>", elementName);
                                i = markup.IndexOf(EndTag);
                                if (i != -1)
                                {
                                    result = new XmlElement(elementName);
                                    InnerMarkup = markup.Substring(elementName.Length + 2,
                                        i - (elementName.Length + 2));
                                    
                                    remaining = markup.Substring(i + EndTag.Length);
                                }
                            }
                            result.AppendChildren(XmlReader.ReadMarkup(InnerMarkup));
                            return result;
                        }
                        else
                        {
                            throw new Exception("Markup error.");
                        }
                    }
                }
                else
                {
                    //Next node will be a TextNode.
                    i = markup.IndexOf('<');
                    if (i != -1)
                    {
                        Result = new XmlTextNode(markup.Substring(0, i));
                        remaining = markup.Substring(i);
                    }
                    else
                    {
                        Result = new XmlTextNode(markup);
                        remaining = "";
                    }
                }
                return Result;
            }
        }
        /// <summary>
        /// Attempts to read the next XmlNode contained in the specified string.
        /// </summary>
        /// <param name="markup"></param>
        /// <param name="remaining"></param>
        /// <returns></returns>
        public static TryResult<XmlNode> TryReadNextNode(string markup, out string remaining)
        {
            try
            {
                return TryResult<XmlNode>.SuccessResult(XmlReader.ReadNextNode(markup, out remaining));
            }
            catch (Exception e)
            {
                remaining = "";
                return TryResult<XmlNode>.FailResult(e);
            }
        }
        private static bool IsValidName(string Name)
        {
            for (int I = 0; I < Name.Length; I++)
            {
                if (char.IsLetterOrDigit(Name[I]) == false)
                {
                    return false;
                }
            }
            return true;
        }
    }
}