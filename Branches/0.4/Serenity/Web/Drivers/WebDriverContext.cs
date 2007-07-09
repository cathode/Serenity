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
using System.Threading;

namespace Serenity.Web.Drivers
{
    public sealed class WebDriverContext
    {
        #region Constructors - Public
        public WebDriverContext() : this(WebDriverContext.DefaultBufferSize)
        {
        }
        public WebDriverContext(int bufferSize)
        {
            this.Buffer = new byte[bufferSize];
        }
        #endregion
        #region Fields - Private
        private byte[] buffer = new byte[DefaultBufferSize];
        private Socket workSocket;
		private ManualResetEvent signal = new ManualResetEvent(false);
        #endregion
        #region Fields - Public
        public const int MaxBufferSize = 65536;
        public const int MinBufferSize = 32;
        public const int DefaultBufferSize = MinBufferSize * 4;
        #endregion
        #region Properties - Public
        public byte[] Buffer
        {
            get
            {
                return this.buffer;
            }
            set
            {
                if (value != null && value.Length > WebDriverContext.MinBufferSize)
                {
                    this.buffer = value;
                }
            }
        }
		public ManualResetEvent Signal
		{
			get
			{
				return this.signal;
			}
		}
        public Socket WorkSocket
        {
            get
            {
                return this.workSocket;
            }
            set
            {
                this.workSocket = value;
            }
        }
        #endregion
    }
}
