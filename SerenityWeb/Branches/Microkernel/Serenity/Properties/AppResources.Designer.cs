﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1434
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Serenity.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class AppResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal AppResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Serenity.Properties.AppResources", typeof(AppResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Client connected from {0}..
        /// </summary>
        internal static string ClientConnectedMessage {
            get {
                return ResourceManager.GetString("ClientConnectedMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Description.
        /// </summary>
        internal static string DirectoryDescriptionColumn {
            get {
                return ResourceManager.GetString("DirectoryDescriptionColumn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (unnamed).
        /// </summary>
        internal static string DirectoryItemDefaultName {
            get {
                return ResourceManager.GetString("DirectoryItemDefaultName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Modified.
        /// </summary>
        internal static string DirectoryModifiedColumn {
            get {
                return ResourceManager.GetString("DirectoryModifiedColumn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Name.
        /// </summary>
        internal static string DirectoryNameColumn {
            get {
                return ResourceManager.GetString("DirectoryNameColumn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Size.
        /// </summary>
        internal static string DirectorySizeColumn {
            get {
                return ResourceManager.GetString("DirectorySizeColumn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Index of {0}.
        /// </summary>
        internal static string DirectoryTitle {
            get {
                return ResourceManager.GetString("DirectoryTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Parameter &apos;{0}&apos; cannot be empty..
        /// </summary>
        internal static string ParamEmptyException {
            get {
                return ResourceManager.GetString("ParamEmptyException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Parameter &apos;{0}&apos; has an invalid format..
        /// </summary>
        internal static string ParamHasInvalidFormatException {
            get {
                return ResourceManager.GetString("ParamHasInvalidFormatException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The resource add request failed because it would create a circular relationship..
        /// </summary>
        internal static string ResourceAddCreatesCircularRelationException {
            get {
                return ResourceManager.GetString("ResourceAddCreatesCircularRelationException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The current Resource does not support children..
        /// </summary>
        internal static string ResourceDoesNotSupportChildrenException {
            get {
                return ResourceManager.GetString("ResourceDoesNotSupportChildrenException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified resource cannot be added because it already has a parent resource defined..
        /// </summary>
        internal static string ResourceHasParentException {
            get {
                return ResourceManager.GetString("ResourceHasParentException", resourceCulture);
            }
        }
    }
}