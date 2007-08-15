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