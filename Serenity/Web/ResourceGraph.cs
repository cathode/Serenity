/******************************************************************************
 * Serenity - Managed Web Application Server. ( http://gearedstudios.com/ )   *
 * Copyright © 2006-2015 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Serenity.Web
{
    /// <summary>
    /// Represents a hierarchial graph of <see cref="ResourceGraphNode"/> items.
    /// </summary>
    public class ResourceGraph
    {
        #region Fields
        public const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ012345678_-{}()[]/";
        private ResourceGraphNode root;
        #endregion
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceGraph"/> class.
        /// </summary>
        public ResourceGraph()
        {
            this.root = new ResourceGraphNode();
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the <see cref="ResourceGraphNode"/> at the root of the graph.
        /// </summary>
        public ResourceGraphNode Root
        {
            get
            {
                return this.root;
            }
            set
            {
                this.root = value;
            }
        }
        #endregion
        #region Methods
        [Pure]
        public static bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            for (int i = 0; i < name.Length; i++)
                if (!AllowedChars.Contains(name[i]))
                    return false;

            return true;
        }
        #endregion
    }
}
