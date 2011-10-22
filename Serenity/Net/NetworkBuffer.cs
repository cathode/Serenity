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
using System.Threading;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace Serenity.Net
{
    /// <summary>
    /// Provides a pool of buffer frames that can be used to store data received
    /// through a socket, and provides synchronization 
    /// </summary>
    public sealed class NetworkBuffer
    {
        #region Fields
        /// <summary>
        /// Holds the default buffer size in bytes, if none is specified.
        /// </summary>
        public static readonly int DefaultFrameLength = 512;

        /// <summary>
        /// Holds the default number of buffer frames to use, if none is specified.
        /// </summary>
        public static readonly int DefaultFrameCount = 2;

        /// <summary>
        /// Holds the <see cref="NetworkBufferFrame"/> instances that are ready to be checked out.
        /// </summary>
        private readonly Queue<NetworkBufferFrame> inactiveFrames;

        /// <summary>
        /// Holds the <see cref="NetworkBufferFrame"/> instances that have been checked out.
        /// </summary>
        private readonly Collection<NetworkBufferFrame> activeFrames;

        /// <summary>
        /// Holds the internally-used synchronization object.
        /// </summary>
        private readonly object sync = new object();

        private int frameLength;
        #endregion
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkBuffer"/> class.
        /// </summary>
        public NetworkBuffer()
        {
            this.inactiveFrames = new Queue<NetworkBufferFrame>(NetworkBuffer.DefaultFrameCount);
            this.activeFrames = new Collection<NetworkBufferFrame>();
            this.frameLength = NetworkBuffer.DefaultFrameLength;
        }
        #endregion
        #region Properties
        public int FrameLength
        {
            get
            {
                return this.frameLength;
            }
            set
            {
                Contract.Requires(value > 0);

                this.frameLength = value;
            }
        }
        /// <summary>
        /// Gets a value indicating the number of buffer frames associated with this
        /// <see cref="NetworkBuffer"/> which have been checked out / are in-use.
        /// </summary>
        public int CheckedOut
        {
            get
            {
                return this.activeFrames.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating the number of buffer frames associated with
        /// this <see cref="NetworkBuffer"/> which have been checked in / are available.
        /// </summary>
        public int CheckedIn
        {
            get
            {
                return this.inactiveFrames.Count;
            }
        }
        public bool AutoCheckIn
        {
            get;
            set;
        }
        #endregion
        #region Methods
        public NetworkBufferFrame CheckOut()
        {
            lock (this.sync)
            {
                if (this.CheckedIn == 0)
                    this.inactiveFrames.Enqueue(new NetworkBufferFrame(this, this.FrameLength));

                var f = this.inactiveFrames.Dequeue();
                this.activeFrames.Add(f);
                if (this.AutoCheckIn)
                {
                    var act = new Action<NetworkBufferFrame>(f.Owner.CheckIn);
                    act.BeginInvoke(f, new AsyncCallback(this.AutoCheckInCallback), act);
                }
                return f;
            }
        }
        private void CheckIn(NetworkBufferFrame frame)
        {
            
            lock (this.sync)
            {
                if (this.CheckedOut == 0)
                    throw new NotImplementedException();

                this.activeFrames.Remove(frame);
                this.inactiveFrames.Enqueue(frame);
                
            }
        }

        private void AutoCheckInCallback(IAsyncResult result)
        {
            var act = (Action<NetworkBufferFrame>)result.AsyncState;
            
            act.EndInvoke(result);
        }

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(this.FrameLength > 0);
        }
        #endregion
    }
}
