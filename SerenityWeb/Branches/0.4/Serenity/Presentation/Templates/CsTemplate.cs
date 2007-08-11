/*
Serenity - The next evolution of web server technology
Serenity/Presentation/Templates/CsTemplate.cs
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

namespace Serenity.Presentation.Templates
{
    public sealed class CsTemplate
    {
        public static CsTemplate IndexTemplate
        {
            get
            {
                CsTemplate template = new CsTemplate();


                return template;
            }
        }
    }
    public enum CsTemplateElementType
    {
        Var,
        EVar,

    }
    public sealed class CsTemplateElement
    {
        public string LeadingContent;
        public string FollowingContent;
        public string InnerContent;
        
    }
}