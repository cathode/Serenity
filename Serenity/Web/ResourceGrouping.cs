/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *----------------------------------------------------------------------------*
 * Authors:                                                                   *
 * - Will 'AnarkiNet' Shelley (AnarkiNet@gmail.com): Original Author          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity.Web
{
    public struct ResourceGrouping : IEquatable<ResourceGrouping>, IComparable<ResourceGrouping>
    {
        /// <summary>
        /// Initializes a new instance of the ResourceGroup class, using a
        /// provided singular and plural form of the grouping name represented
        /// by the new ResourceGroup instance.
        /// </summary>
        /// <param name="singularForm">The singular form of the grouping name.</param>
        /// <param name="pluralForm">The plural form of the grouping name.</param>
        public ResourceGrouping(string singularForm, string pluralForm)
        {
            if (singularForm == null)
            {
                throw new ArgumentNullException("singularForm");
            }
            else if (pluralForm == null)
            {
                throw new ArgumentNullException("pluralForm");
            }
            this.singularForm = singularForm;
            this.pluralForm = pluralForm;
        }
        private readonly string singularForm;
        private readonly string pluralForm;
        #region Fields - Public
        public static readonly ResourceGrouping Resources = new ResourceGrouping("Resource", "Resources");
        public static readonly ResourceGrouping Directories = new ResourceGrouping("Directory", "Directories");
        public static readonly ResourceGrouping Files = new ResourceGrouping("File", "Files");
        public static readonly ResourceGrouping Dynamic = new ResourceGrouping("Dynamic", "Dynamic");
        public static readonly ResourceGrouping Unspecified = new ResourceGrouping("Unspecified", "Unspecified");
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
            {
                return this.singularForm;
            }
            else
            {
                return this.pluralForm;
            }
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
