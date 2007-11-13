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

using Serenity.Web;

namespace Serenity.Pages
{
    /// <summary>
    /// Provides a web administration page to add, remove, view, and edit Domain Settings.
    /// </summary>
    public sealed class DomainsPage : ContentPage
    {
        public override void OnInitialization()
        {

        }
        public override void OnRequest(CommonContext context)
        {
            CommonResponse response = context.Response;

            response.WriteLine(Doctype.XHTML11.ToString());
            response.WriteLine(@"<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
	<title>Domain Configuration Panel</title>
	<link rel='stylesheet' type='text/css' href='/theme/stylesheet' />
</head>
<body class='ContentA'>
	<div class='header'>
		<h2 class='HeadingA'>Domain Configuration Panel</h2>
	</div>
	<div class='setting'>
		" + @"
	</div>
</body>
</html>");
            response.ContentType = MimeType.TextHtml;
        }
        public override void OnShutdown()
        {

        }

        public override string Name
        {
            get
            {
                return "Domains";
            }
        }
    }
}
