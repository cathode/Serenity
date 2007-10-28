/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright � 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
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
    public abstract class ResourceClass : Resource
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
            this.Name = name;
        }
        #endregion
        #region Fields - Private
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
				return null;
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
    }
}
