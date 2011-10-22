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
using System.Diagnostics.Contracts;
using System.Threading;

namespace Serenity.Net
{
    /// <summary>
    /// Represents a discrete buffer of binary data that is part of a <see cref="NetworkBuffer"/>.
    /// </summary>
    public sealed class NetworkBufferFrame
    {
        #region Fields
        internal readonly NetworkBuffer Owner;
        private readonly AutoResetEvent AutoReset;
        private readonly byte[] content;
        private readonly int capacity = 0;
        private int contentSize = 0;
        #endregion
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkBufferFrame"/> class.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="capacity"></param>
        internal NetworkBufferFrame(NetworkBuffer owner, int capacity)
        {
            Contract.Requires(owner != null);
            Contract.Requires(capacity > 0);

            this.content = new byte[capacity];
            this.capacity = capacity;

            this.Owner = owner;
            this.AutoReset = new AutoResetEvent(false);
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the capacity of the buffer frame (in bytes).
        /// </summary>
        public int Capacity
        {
            get
            {
                return this.capacity;
            }
        }

        /// <summary>
        /// Gets the byte array that is the content of the buffer frame.
        /// </summary>
        public byte[] Content
        {
            get
            {
                return this.content;
            }
        }

        /// <summary>
        /// Gets or sets the number of bytes in <see cref="NetworkBufferFrame.Content"/>
        /// that are actually data.
        /// </summary>
        /// <remarks>
        /// Because the array used to hold the buffer frame content is reused,
        /// and is created with a predetermined size, it may not be filled to
        /// it's capacity in all scenarios.
        /// </remarks>
        public int ContentSize
        {
            get
            {
                return this.contentSize;
            }
            set
            {
                Contract.Requires(value <= this.Capacity);

                this.contentSize = value;
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Notifies the owning <see cref="NetworkBuffer"/> that processing is
        /// done, and there is no further use for the contents of this frame.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public void Release()
        {
            this.ContentSize = 0;
            this.AutoReset.Set();
        }

        /// <summary>
        /// 
        /// </summary>
        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(this.ContentSize >= 0);
            Contract.Invariant(this.ContentSize <= this.Capacity);
            Contract.Invariant(this.Content.Length == this.Capacity);
            Contract.Invariant(this.Content != null);
        }
        #endregion
    }
}