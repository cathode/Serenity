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
using System.Text;

using Serenity.Web;
using Serenity.Web.Drivers;

//WS: This code probably needs LOTS of work in the future.

namespace Serenity
{
    public abstract class ResourceClass
    {
        #region Constructors - Private
        static ResourceClass()
        {
            ResourceClass.resourceClasses = new Dictionary<string, ResourceClass>();
        }
        #endregion
        #region Constructors - Protected
        public ResourceClass(string name)
        {
            this.name = name;
        }
        #endregion
        #region Fields - Private
        private readonly string name;
        private static Dictionary<string, ResourceClass> resourceClasses;
        #endregion
        #region Methods - Public
        public abstract void HandleContext(CommonContext context);
        public static ResourceClass GetResourceClass(string name)
        {
            if (ResourceClass.resourceClasses.ContainsKey(name))
            {
                return ResourceClass.resourceClasses[name];
            }
            else
            {
                return new ResourceClass.DefaultImplementation();
            }
        }
        public static bool RegisterResourceClass(ResourceClass resourceClass)
        {
            if (!ResourceClass.resourceClasses.ContainsKey(resourceClass.Name))
            {
                ResourceClass.resourceClasses.Add(resourceClass.Name, resourceClass);
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
        #region Properties - Public
        public string Name
        {
            get
            {
                return this.name;
            }
        }
        #endregion
        public sealed class DefaultImplementation : ResourceClass
        {
            #region Constructors - Internal
            public DefaultImplementation() : base("default")
            {
            }
            #endregion
            #region Methods - Public
            public override void HandleContext(CommonContext context)
            {
            }
            #endregion
        }
    }
}
