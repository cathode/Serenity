/*
Serenity - The next evolution of web server technology
Serenity/Attributes/Module.cs
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

namespace Serenity.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class ModuleTitleAttribute : Attribute
    {
        public ModuleTitleAttribute(string name)
        {
            this.Name = name;
        }
        public readonly string Name;

    }
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class ModuleDefaultPageAttribute : Attribute
    {
        public ModuleDefaultPageAttribute(string typeName)
        {
            this.TypeName = typeName;
        }
        public readonly string TypeName;
    }
}
