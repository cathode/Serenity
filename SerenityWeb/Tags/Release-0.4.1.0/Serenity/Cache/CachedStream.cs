/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Permissive License (Ms-PL), a copy of which should have been included with *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Serenity
{
    public sealed class CachedStream : Stream
    {
        internal CachedStream(Stream baseStream)
        {
            if (baseStream.CanRead == false)
            {
                throw new ArgumentException("Base Stream must support reading.");
            }
            this.baseStream = baseStream;
        }
        private Stream baseStream;
        private List<Byte> cache = new List<byte>();
        private long cacheCapacity = default(long);
        public long CacheLength
        {
            get
            {
                return this.cache.Count;
            }
        }
        public long CacheCapacity
        {
            get
            {
                return this.cacheCapacity;
            }
            set
            {
                this.cacheCapacity = value;
            }
        }
        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        public override void Flush()
        {
            throw new NotSupportedException();
        }

        public override long Length
        {
            get
            {
                return this.baseStream.Length;
            }
        }

        public override long Position
        {
            get
            {
                return this.baseStream.Position;
            }
            set
            {
                this.baseStream.Position = value;
            }
        }
        /// <summary>
        /// Preloads a specified number of bytes from the BaseStream into the cache.
        /// </summary>
        /// <param name="count">The number of bytes to preload.</param>
        /// <returns>The number of bytes actually preloaded.
        /// If the end of the BaseStream was reached prematurely,
        /// or if the cache capacity was reached prematurely,
        /// this number may be less than count.</returns>
        public int Preload(int count)
        {
            int i;
            long originalPosition = this.baseStream.Position;
            this.baseStream.Position = this.cache.Count;
            for (i = 0; i < count; i++)
            {
                int b = this.baseStream.ReadByte();
                if (b != -1)
                {
                    this.cache.Add((Byte)b);
                }
                else
                {
                    break;
                }
            }

            return i;
        }
        public int PreloadAll()
        {
            return this.Preload((int)this.Length);
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (this.cache.Count > 0)
            {

            }
            return 0;
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            return this.baseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }
}