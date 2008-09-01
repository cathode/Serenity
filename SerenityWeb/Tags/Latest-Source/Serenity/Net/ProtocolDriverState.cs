/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Net.Sockets;
using System.Threading;
using SerenityProject.Common;

namespace Serenity.Net
{
    /// <summary>
    /// Provides a simple data structure used to pass objects to and from async callback methods.
    /// </summary>
    public sealed class ProtocolDriverState : Disposable
    {
        #region Constructors - Public
        /// <summary>
        /// Initializes a new instance of the WebDriverState class using the default buffer size.
        /// </summary>
        public ProtocolDriverState()
            : this(ProtocolDriverState.DefaultBufferSize)
        {
        }
        /// <summary>
        /// Initializes a new instance of the WebDriverState class using the supplied buffer size.
        /// </summary>
        /// <param name="bufferSize"></param>
        public ProtocolDriverState(int bufferSize)
        {
            if (bufferSize > ProtocolDriverState.MaxBufferSize || bufferSize < ProtocolDriverState.MinBufferSize)
            {
                throw new ArgumentOutOfRangeException("Invalid value specified for bufferSize. Valid values are between " + ProtocolDriverState.MinBufferSize.ToString() + " and " + ProtocolDriverState.MaxBufferSize + ".");
            }
            this.buffer = new byte[bufferSize];
        }
        #endregion
        #region Fields - Private
        private byte[] buffer;
        private ManualResetEvent signal = new ManualResetEvent(false);
        private Socket workSocket;
        #endregion
        #region Fields - Public
        /// <summary>
        /// Holds the maximum buffer size.
        /// </summary>
        public const int MaxBufferSize = 65536;
        /// <summary>
        /// Holds the minimum buffer size.
        /// </summary>
        public const int MinBufferSize = 32;
        /// <summary>
        /// Holds the default (optimal) buffer size.
        /// </summary>
        public const int DefaultBufferSize = MinBufferSize * 4;
        #endregion
        #region Methods - Protected
        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }
            (this.signal as IDisposable).Dispose();
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets or sets the data buffer associated with the current WebDriverState.
        /// </summary>
        public byte[] Buffer
        {
            get
            {
                return this.buffer;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                else if (value.Length > ProtocolDriverState.MaxBufferSize || value.Length < ProtocolDriverState.MinBufferSize)
                {
                    throw new ArgumentOutOfRangeException("Argument 'value' must be a byte[] with a length between "
                        + ProtocolDriverState.MinBufferSize + " and " + ProtocolDriverState.MaxBufferSize + ".", "value");
                }
                this.buffer = value;
            }
        }
        /// <summary>
        /// Gets or sets the socket, used to perform operations on, associated with the
        /// current WebDriverState.
        /// </summary>
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
