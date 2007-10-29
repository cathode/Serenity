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

using Serenity.Collections;
using Serenity.Web;

namespace Serenity.Pages
{
    public sealed class IndexerPage : ContentPage
    {
       

        public override void OnRequest(CommonContext context)
        {
            if (context.Request.RequestData.Contains("url"))
            {
                
            }
        }
        public override MimeType MimeType
        {
            get
            {
                return MimeType.ApplicationXml;
            }
            protected internal set
            {
            }
        }
        public override string Name
        {
            get
            {
                return "Indexer";
            }
            protected internal set
            { 
            }
        }
    }
}
