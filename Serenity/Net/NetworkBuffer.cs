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
    /// <summary>
    /// Provides a complex buffer for receiving data and then working with it.
    /// </summary>
    public sealed class NetworkBuffer
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkBuffer"/> class.
        /// </summary>
        public NetworkBuffer()
        {
            this.dataBuffer = new Queue<byte>();
            this.receiveBuffer = new byte[DefaultBufferSize];
        }
        #endregion
        #region Fields
        private readonly Queue<byte> dataBuffer;
        private byte[] receiveBuffer;
        /// <summary>
        /// Holds the default receive buffer size if none is specified.
        /// </summary>
        public const int DefaultBufferSize = 1024;
        #endregion
        #region Methods
        /// <summary>
        /// Swaps the receive buffer into the data buffer and resets the receive buffer.
        /// </summary>
        /// <param name="received">The amount of the receive buffer that was actually used.</param>
        public void SwapBuffers(int received)
        {
            if (received > 0)
                for (int i = 0; i < received; i++)
                    dataBuffer.Enqueue(receiveBuffer[i]);
        }
        /// <summary>
        /// Resizes the <see cref="Receive"/> to the specified size, clearing all data from it.
        /// </summary>
        /// <param name="size">The new size.</param>
        public void ResizeReceiveBuffer(int size)
        {
            //TODO: Determine if zero-size receive buffer should be allowed/makes sense.
            this.receiveBuffer = new byte[Math.Max(1, size)];
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets a queue of bytes that contains the unparsed data that has been buffered.
        /// </summary>
        public Queue<byte> Data
        {
            get
            {
                return this.dataBuffer;
            }
        }
        /// <summary>
        /// Gets the byte buffer used to store incoming network data.
        /// </summary>
        public byte[] Receive
        {
            get
            {
                return this.receiveBuffer;
            }
        }
        #endregion
    }
}
