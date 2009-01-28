/* Serenity - The next evolution of web server technology.
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/
 * 
 * This software is released under the terms and conditions of the Microsoft Public License (Ms-PL), a copy of which
 * should have been included with this distribution as License.txt.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serenity.Net
{
    public sealed class NetworkBuffer
    {
        public NetworkBuffer()
        {
            this.dataBuffer = new Queue<byte>();
            this.receiveBuffer = new byte[DefaultBufferSize];
        }
        #region Fields
        private readonly Queue<byte> dataBuffer;
        private byte[] receiveBuffer;
        private const int DefaultBufferSize = 1024;
        #endregion
        /// <summary>
        /// Swaps the <see cref="Receive"/> into the data buffer and resets the receive buffer.
        /// </summary>
        /// <param name="received">The amount of the receive buffer that was actually used.</param>
        public void SwapBuffers(int received)
        {
            if (received > 0)
            {
                for (int i = 0; i < received; i++)
                {
                    dataBuffer.Enqueue(receiveBuffer[i]);
                }
            }
        }
        /// <summary>
        /// Resizes the <see cref="Receive"/>, clearing all data from it.
        /// </summary>
        /// <param name="size">The new size.</param>
        public void ResizeReceiveBuffer(int size)
        {
            this.receiveBuffer = new byte[Math.Max(1, size)];
        }
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
    }
}
