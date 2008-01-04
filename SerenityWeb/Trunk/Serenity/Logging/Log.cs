/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2008 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Serenity.Logging
{
    /// <summary>
    /// Provides a method which allows other parts of Serenity or
    /// loaded modules to write messages to a central log file.
    /// </summary>
    public sealed class Log : IDisposable
    {
        #region Constructors - Private
        /// <summary>
        /// Initializes a new instance of the Log class, using the standard output stream of the console.
        /// </summary>
        public Log() : this(Console.OpenStandardOutput())
        {
        }
        /// <summary>
        /// Initializes a new instance of the Log class, using the specified outputStream.
        /// </summary>
        /// <param name="outputStream">A stream to which log messages will be written to.</param>
        public Log(Stream outputStream)
        {
            if (outputStream == null)
            {
                throw new ArgumentNullException("outputStream");
            }
            else if (!outputStream.CanWrite)
            {
                throw new ArgumentException(__Strings.StreamMustSupportWriting, "outputStream");
            }

            this.outputStream = outputStream;
        }
        #endregion
        #region Fields - Private
        private bool isDisposed = false;
        private DateTime lastWrite = DateTime.Now;
        private TimeSpan maxWait = TimeSpan.FromMilliseconds(250);
        private Queue<LogMessage> messages = new Queue<LogMessage>();
        private Stream outputStream;
        #endregion
        #region Methods - Public
        /// <summary>
        /// Releases unmanaged resources used by the current Log.
        /// </summary>
        public void Dispose()
        {
            this.outputStream.Dispose();
            this.outputStream = null;
            this.messages = null;
            this.isDisposed = true;
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="message">A string containing a description of the message.</param>
        /// <param name="level">A LogMessageLevel object describing the severity of the message.</param>
        public void Write(string message, LogMessageLevel level)
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
            else if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            else if (message == string.Empty)
            {
                throw new ArgumentException(__Strings.ArgumentCannotBeEmpty, "message");
            }

            lock (this)
            {
                this.messages.Enqueue(new LogMessage(message, level));

                if (DateTime.Now - this.lastWrite > this.maxWait)
                {
                    LogMessage logMessage;
                    while (this.messages.Count > 0)
                    {
                        logMessage = this.messages.Dequeue();

                        byte[] buffer = Encoding.UTF8.GetBytes(logMessage.ToString() + "\r\n");
                        this.outputStream.Write(buffer, 0, buffer.Length);
                    }
                }
            }
        }
        #endregion
    }
    
}