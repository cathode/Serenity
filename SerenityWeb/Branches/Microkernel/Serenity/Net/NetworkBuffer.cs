/* Serenity - The next evolution of web server technology.
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/
 * 
 * This software is released under the terms and conditions of the Microsoft
 * Public License (Ms-PL), a copy of which should have been included with
 * this distribution as License.txt.
 *
 * Contributors:
 * Will 'AnarkiNet' Shelley (AnarkiNet@gmail.com)
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
            this.dataBuffer = new byte[DefaultBufferSize];
            this.receiveBuffer = new byte[DefaultBufferSize];
        }
        #region Fields
        private byte[] dataBuffer;
        private int dataPosition = 0;
        private byte[] receiveBuffer;
        private const int DefaultBufferSize = 1024;
        #endregion
        /// <summary>
        /// Swaps the <see cref="Receive"/> into the data buffer and
        /// resets the receive buffer.
        /// </summary>
        public void SwapBuffers(int received)
        {
            if (received > 0)
            {
                if (dataPosition + received > this.dataBuffer.Length)
                {
                    //Need to resize the data buffer to accomodate new data
                    byte[] buffer = new byte[(this.dataBuffer.Length - dataPosition) + received];
                    this.dataBuffer.CopyTo(buffer, 0);
                    this.dataBuffer = buffer;
                }

                Array.Copy(this.receiveBuffer, 0, this.dataBuffer, this.dataPosition, received);
                this.dataPosition += received;
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
        public byte[] Data
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
