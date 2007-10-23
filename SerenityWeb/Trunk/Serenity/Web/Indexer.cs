/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

using System.Xml;

namespace Serenity.Web
{
    public sealed class IndexData : IndexDataNode
    {
        private string location;

        public string Location
        {
            get
            {
                return this.location;
            }
        }
    }
    public class IndexDataNode
    {
        internal IndexDataNode()
        {
        }
        private IndexDataNode(string name)
        {
            this.name = name;
        }
        private List<IndexDataNode> children;
        private readonly string name;
        private string icon;

    }
    public sealed class Indexer
    {
        #region Constructors - Public
        private Indexer()
        {
        }
        #endregion
        #region Fields - Private
        private static Indexer defaultIndexer = new Indexer();
        #endregion
        #region Methods - Public
        public byte[] Generate(string location)
        {
            StringBuilder output = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(output);
            writer.WriteStartDocument();
            writer.WriteProcessingInstruction("xml-stylesheet", "type='text/xsl' href='/resource/serenity/index.xslt'");
            writer.WriteStartElement("index");
            writer.WriteStartElement("location");
            writer.WriteString(location);
            writer.WriteEndElement();
            writer.WriteStartElement("group");

            writer.WriteEndDocument();
			writer.Flush();
			writer.Close();

            return Encoding.UTF8.GetBytes(output.ToString());
        }
        #endregion
        #region Properties - Public
        public static Indexer DefaultIndexer
        {
            get
            {
                return Indexer.defaultIndexer;
            }
        }
        #endregion
    }
}