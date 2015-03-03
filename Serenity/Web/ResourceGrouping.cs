/******************************************************************************
 * Serenity - Managed Web Application Server. ( http://gearedstudios.com/ )   *
 * Copyright © 2006-2015 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.Contracts;

namespace Serenity.Web
{
    /// <summary>
    /// Represents a categorization of a resource.
    /// </summary>
    public sealed class ResourceGrouping : IEquatable<ResourceGrouping>, IComparable<ResourceGrouping>
    {
        #region Fields
        public static readonly ResourceGrouping Resources = new ResourceGrouping("Resource", "Resources");
        public static readonly ResourceGrouping Directories = new ResourceGrouping("Directory", "Directories");
        public static readonly ResourceGrouping Files = new ResourceGrouping("File", "Files");
        public static readonly ResourceGrouping Dynamic = new ResourceGrouping("Dynamic", "Dynamic");
        public static readonly ResourceGrouping Unspecified = new ResourceGrouping("Unspecified", "Unspecified");
        private readonly string singularForm;
        private readonly string pluralForm;
        #endregion
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the ResourceGroup class, using a
        /// provided singular and plural form of the grouping name represented
        /// by the new ResourceGroup instance.
        /// </summary>
        /// <param name="singularForm">The singular form of the grouping name.</param>
        /// <param name="pluralForm">The plural form of the grouping name.</param>
        public ResourceGrouping(string singularForm, string pluralForm)
        {
            Contract.Requires(singularForm != null);
            Contract.Requires(pluralForm != null);

            this.singularForm = singularForm;
            this.pluralForm = pluralForm;
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets the singular form of the grouping name.
        /// </summary>
        public string SingularForm
        {
            get
            {
                return this.singularForm;
            }
        }
        /// <summary>
        /// Gets the plural form of the grouping name.
        /// </summary>
        public string PluralForm
        {
            get
            {
                return this.pluralForm;
            }
        }
        #endregion
        
        #region Methods - Public
        /// <summary>
        /// Gets the singular form of the grouping name.
        /// </summary>
        /// <returns>A string containing the singular form of the grouping name.</returns>
        public override string ToString()
        {
            return this.ToString(true);
        }
        /// <summary>
        /// Gets the singular or the plural form of the grouping name.
        /// </summary>
        /// <param name="useSingular">If true, singular form is returned;
        /// otherwise plural form is returned.</param>
        /// <returns>A string containing the singular or plural form of the grouping name.</returns>
        public string ToString(bool useSingular)
        {
            if (useSingular)
                return this.singularForm ?? string.Empty;
            else
                return this.pluralForm ?? string.Empty;
        }
        #endregion
        

        #region IEquatable<ResourceGrouping> Members

        public bool Equals(ResourceGrouping other)
        {
            return this.SingularForm.Equals(other.SingularForm) && this.PluralForm.Equals(other.PluralForm);
        }

        #endregion

        #region IComparable<ResourceGrouping> Members

        public int CompareTo(ResourceGrouping other)
        {
            return this.SingularForm.CompareTo(other.SingularForm);
        }

        #endregion
    }
}
