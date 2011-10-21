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

namespace Serenity.Net
{
    public enum ConnectionListenerState
    {
        Faulted = -1,
        Stopped = 0,
        Stopping = 1,
        Initializing = 2,
        Initialized = 3,
        Starting = 4,
        Started = 5,
    }
}
