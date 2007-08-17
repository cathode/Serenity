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
        public string RenderIndexSet(HdfDataset dataset)
        {
            dataset.IsCaseSensitive = false;
            //string resourceName = dataset["title"].Value;

            return null;
        }
    }
}
