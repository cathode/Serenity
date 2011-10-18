/******************************************************************************
 * Serenity - Managed Web Application Server. ( http://gearedstudios.com/ )   *
 * Copyright © 2006-2011 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Serenity.Net
{
    public interface IListener
    {
        #region Properties
        int Port
        {
            get;
            set;
        }
        IPAddress Address
        {
            get;
            set;
        }
        #endregion
        #region Methods
        bool Start();
        bool Stop();
        bool Restart();
        #endregion
    }
}
