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
using System.Net.Sockets;
using System.Text;

namespace Serenity.Web.Drivers
{
    /// <summary>
    /// Provides a base class for Web Adapters,
    /// objects used to transform a CommonContext to or from an array of bytes.
    /// </summary>
    public abstract class WebAdapter
    {
        #region Constructors - Internal
        protected WebAdapter()
        {
        }
        #endregion
        #region Methods - Protected
        protected abstract bool WriteHeaders(Socket socket, CommonContext context);
        protected abstract bool WriteContent(Socket socket, CommonContext context);
        #endregion
        #region Methods - Public
        public abstract bool ReadContext(Socket socket, out CommonContext context);
        public virtual bool WriteContext(Socket socket, CommonContext context)
        {
            if (!context.HeadersWritten)
            {
                context.HeadersWritten = this.WriteHeaders(socket, context);
            }

            if (context.HeadersWritten)
            {
                return this.WriteContent(socket, context);
            }
            else
            {
                return false;
            }
        }
        
        #endregion
    }
}