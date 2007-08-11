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

namespace Serenity
{
	/// <summary>
	/// Represents a Resource Class, which is an organizational and functional unit
	/// that assists in the processing of requests.
	/// </summary>
    public abstract class ResourceClass
    {
        #region Constructors - Private
        static ResourceClass()
        {
            ResourceClass.resourceClasses = new Dictionary<string, ResourceClass>();
        }
        #endregion
        #region Constructors - Public
		/// <summary>
		/// Initializes a new instance of the ResourceClass class.
		/// </summary>
		/// <param name="name"></param>
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
		/// <summary>
		/// When overridden in a derived class, handles a supplied CommonContext and generates a suitable response.
		/// </summary>
		/// <param name="context"></param>
        public abstract void HandleContext(CommonContext context);
		/// <summary>
		/// Gets a registered ResourceClass.
		/// </summary>
		/// <param name="name">The name of the ResourceClass to get.</param>
		/// <returns></returns>
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
		/// <summary>
		/// Registers a ResourceClass.
		/// </summary>
		/// <param name="resourceClass">The ResourceClass to register.</param>
		/// <returns></returns>
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
		/// <summary>
		/// Gets the name of the current ResourceClass.
		/// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }
        #endregion
		/// <summary>
		/// Provides a basic implementation of the ResourceClass class which does nothing.
		/// </summary>
        public sealed class DefaultImplementation : ResourceClass
        {
            #region Constructors - Internal
			/// <summary>
			/// Initializes a new instance of the ResourceClass.DefaultImplementation class.
			/// </summary>
            public DefaultImplementation() : base("default")
            {
            }
            #endregion
            #region Methods - Public
			/// <summary>
			/// Does nothing.
			/// </summary>
			/// <param name="context"></param>
            public override void HandleContext(CommonContext context)
            {
            }
            #endregion
        }
    }
}
