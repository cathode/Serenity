/******************************************************************************
 * Serenity - Managed Web Application Server. ( http://gearedstudios.com/ )   *
 * Copyright © 2006-2015 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.IO;
using System.Linq;

namespace Serenity.Data
{
    /// <summary>
    /// Provides a wrapper around the underlying database layer.
    /// </summary>
    public class Database
    {
        #region Constructors
        internal Database()
        {

        }
        #endregion
        #region Methods - Public
        /// <summary>
        /// Creates the database of the specified scope if it does not exist.
        /// </summary>
        /// <returns></returns>
        public bool Open()
        {
            throw new NotImplementedException();
            
        }
        #endregion
    }
}
