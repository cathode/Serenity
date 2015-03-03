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

namespace Serenity
{
    public class VirtualHost
    {
        #region Fields
        private readonly List<string> aliases = new List<string>();
        #endregion
        #region Properties
        public string Hostname
        {
            get;
            set;
        }
        public List<string> Aliases
        {
            get
            {
                return this.aliases;
            }
        }
        #endregion
    }
}
