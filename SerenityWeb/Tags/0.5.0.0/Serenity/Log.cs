/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
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

namespace Serenity
{
    /// <summary>
    /// Provides a method which allows other parts of Serenity or
    /// loaded modules to write messages to a central log file.
    /// </summary>
    public sealed class Log : IDisposable
    {
        //TODO: re-implement log class as a non-static class where each Log object represents an individual logfile.
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
                throw new ArgumentException("Supplied outputStream must support writing.", "outputStream");
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
                throw new ArgumentException("Argument 'message' cannot be empty.", "message");
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
    /// <summary>
    /// Represents an individual log message.
    /// </summary>
    internal class LogMessage
    {
        #region Constructors - Public
        public LogMessage(string message, LogMessageLevel level)
        {
            this.level = level;
            this.message = message.Replace("\t", "\\t");
            
            this.timestamp = DateTime.UtcNow;

            // The assembly name is only retrieved if the message is more important than Info.
            if (level > LogMessageLevel.Info)
            {
                this.assemblyName = Path.GetFileName(Assembly.GetCallingAssembly().Location);
            }
            else
            {
                this.assemblyName = "none";
            }

        }
        #endregion
        #region Fields - Internal
        private string message;
        private LogMessageLevel level;
        private string assemblyName;
        private DateTime timestamp;
        #endregion
        #region Methods - Public
        public override string ToString()
        {
            return string.Format("{0}\t{1}\t{2}\t{3}\r\n",
                                    this.TimeStamp.ToString("s"),
                                    this.Level.ToString(),
                                    this.AssemblyName,
                                    this.Message);
        }
        #endregion
        #region Properties - Public
        public string AssemblyName
        {
            get
            {
                return this.assemblyName;
            }
        }
        public LogMessageLevel Level
        {
            get
            {
                return this.level;
            }
        }
        public string Message
        {
            get
            {
                return this.message;
            }
        }
        
        public DateTime TimeStamp
        {
            get
            {
                return this.timestamp;
            }
        }
        #endregion
    }
}