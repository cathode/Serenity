﻿/*
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

using Serenity.Hdf;
using Serenity.Themes;
using Serenity.Xml.Html;

namespace Serenity.Presentation
{
    /// <summary>
    /// Generates pretty index page content.
    /// </summary>
    public class IndexRenderer
    {
        public HtmlElement RenderIndexSet(HdfDataset dataset)
        {
            dataset.IsCaseSensitive = false;
            //string resourceName = dataset["title"].Value;

            return null;
        }
    }
}