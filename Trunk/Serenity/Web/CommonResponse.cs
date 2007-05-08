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
using System.Net;
using System.Net.Sockets;
using System.Text;

using Serenity.Web.Drivers;

namespace Serenity.Web
{
    /// <summary>
    /// Represents a universal response to a request made by a client.
    /// </summary>
    public sealed class CommonResponse
    {
        #region Constructors - Internal
        internal CommonResponse(CommonContext Owner)
        {
            this.sendBuffer = new Byte[0];
            this.headers = new HeaderCollection();
            this.owningContext = Owner;
            this.mimeType = "text/plain";
            this.capabilities = new CommonCapabilities();
            this.useCompression = false;
        }
        #endregion
        #region Fields - Private
        private CommonCapabilities capabilities;
        private Socket clientSocket;
        private Byte[] sendBuffer;
        private string mimeType;
        private ushort statusCode;
        private string statusText;
        private HeaderCollection headers;
        private CommonContext owningContext;
        private bool useCompression;
        #endregion
        #region Fields - Public
        public StatusCode Status;
        #endregion
        internal void Bind(Socket ClientSocket)
        {
            this.clientSocket = ClientSocket;
        }

        /// <summary>
        /// Causes the currently buffered data to be written to the underlying client socket, then clears the Buffer.
        /// Note: The underlying Socket is unaffected if the current CommonResponse does not support chunked transmission.
        /// </summary>
        /// <returns>The number of bytes flushed, or -1 if an error occurred.</returns>
        public int Flush()
        {
            return 0;
        }
        /// <summary>
        /// Writes a sequence of bytes to the internal write Buffer.
        /// </summary>
        /// <param name="Content">The array of bytes to write.</param>
        /// <returns>The number of bytes written, or -1 if an error occurred.</returns>
        public int Write(Byte[] Content)
        {
            if (Content != null)
            {
                if (Content.Length > 0)
                {
                    if (this.sendBuffer.Length > 0)
                    {
                        Byte[] NewBuffer = new Byte[this.sendBuffer.Length + Content.Length];
                        this.sendBuffer.CopyTo(NewBuffer, 0);
                        Content.CopyTo(NewBuffer, this.sendBuffer.Length - 1);
                        this.sendBuffer = NewBuffer;
                    }
                    else
                    {
                        this.sendBuffer = new Byte[Content.Length];
                        Content.CopyTo(this.sendBuffer, 0);
                    }
                    return Content.Length;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return -1;
            }
        }
        public int Write(string Content)
        {
            return this.Write(Encoding.UTF8.GetBytes(Content));
        }

        #region Properties - Public
        /// <summary>
        /// Gets or sets a CommonCapabilities object describing what the current CommonResponse is able to do.
        /// </summary>
        public CommonCapabilities Capabilities
        {
            get
            {
                return this.capabilities;
            }
            set
            {
                this.capabilities = value;
            }
        }
        public string MimeType
        {
            get
            {
                return this.mimeType;
            }
            set
            {
                if (value.Contains("/") == true)
                {
                    if (value.IndexOf('/') == value.LastIndexOf('/'))
                    {
                        this.mimeType = value;
                    }
                }
            }
        }
        [Obsolete]
        public ushort StatusCode
        {
            get
            {
                return this.statusCode;
            }
            set
            {
                this.statusCode = value;
            }
        }
        [Obsolete]
        public string StatusText
        {
            get
            {
                return this.statusText;
            }
            set
            {
                this.statusText = value;
            }
        }
        internal Byte[] SendBuffer
        {
            get
            {
                return this.sendBuffer;
            }
        }
        public HeaderCollection Headers
        {
            get
            {
                return this.headers;
            }
        }

        public CommonContext Owner
        {
            get
            {
                return this.owningContext;
            }
        }

        /// <summary>
        /// Gets the WebDriver from which the current CommonContext originated from.
        /// </summary>
        public WebDriver Origin
        {
            get
            {
                return this.owningContext.Origin;
            }
        }
        public bool UseCompression
        {
            get
            {
                return this.useCompression;
            }
            set
            {
                this.useCompression = value;
            }
        }
        #endregion
    }
}