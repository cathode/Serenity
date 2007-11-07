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
    /// Used to indicate the severity or type of a log message.
    /// </summary>
    public enum LogMessageLevel
    {
        /// <summary>
        /// Indicates that the message is only useful for debugging purposes,
        /// the message is only useful to developers.
        /// </summary>
        Debug,
        /// <summary>
        /// Indicates that the message contains informational content about
        /// something that has taken place.
        /// </summary>
        Info,
        /// <summary>
        /// Indicates that the message might be related to an issue with the
        /// current behaviour of the server.
        /// </summary>
        Notice,
        /// <summary>
        /// Indicates that the message is informing the reader about unstable
        /// or unsafe behaviour or configuration of the server.
        /// </summary>
        Warning,
        /// <summary>
        /// Indicates that the message describes a critical problem that has taken place.
        /// </summary>
        Error,
    }
    /// <summary>
    /// Provides a method which allows other parts of Serenity or
    /// loaded modules to write messages to a central log file.
    /// </summary>
    public sealed class Log : IDisposable
    {
        //TODO: re-implement log class as a non-static class where each Log object represents an individual logfile.
        #region Constructors - Private
        public Log()
        {
            this.outputStream = Console.OpenStandardOutput();
        }
        public Log(Stream outputStream)
        {
            this.outputStream = outputStream;
        }
        #endregion
        #region Fields - Private
        private Stream outputStream;
        private Queue<LogMessage> messages = new Queue<LogMessage>();
        private TimeSpan maxWait = TimeSpan.FromMilliseconds(250);
        private DateTime lastWrite = DateTime.Now;
        #endregion
        #region Methods - Public
        public void Dispose()
        {
            this.outputStream.Dispose();
        }
        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="message">A string containing a description of the message.</param>
        /// <param name="level">A LogMessageLevel object describing the severity of the message.</param>
        public void Write(string message, LogMessageLevel level)
        {
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
        #region Properties - Public
        public TimeSpan MaxWait
        {
            get
            {
                return this.maxWait;
            }
        }
        #endregion
    }
    public class LogMessage
    {
        #region Constructors - Public
        public LogMessage(string message, LogMessageLevel level)
        {
            this.level = level;
            this.message = message;
            
            this.timestamp = DateTime.UtcNow;

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