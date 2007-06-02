/*
Serenity - The next evolution of web server technology

Copyright © 2006-2007 Serenity Project (http://SerenityProject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Serenity
{
    internal enum LogState
    {
        Started,
        Stopped,
    }
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
    public static class Log
    {
        #region Constructors - Private
        static Log()
        {
            Log.waiting = new Queue<LogEntry>();
            
        }
        #endregion
        #region Fields - Private
        private static bool console = false;
        private static bool file = true;
        private static Queue<LogEntry> waiting;
        private static int interval = Log.MinimumInterval;
        private static LogState currentState = LogState.Stopped;
        private static LogState desiredState = LogState.Stopped;
        #endregion
        #region Fields - Public
        public const int MinimumInterval = 500;
        #endregion
        #region Methods - Private
        private static void LogWorker(object unused)
        {
            if (Log.desiredState == LogState.Started)
            {
                Log.currentState = LogState.Started;

                while (Log.currentState == LogState.Started)
                {
                    while (Log.waiting.Count > 0)
                    {
                        LogEntry entry = Log.waiting.Dequeue();
                        if (Log.console == true)
                        {
                            Console.WriteLine("[{0}] {1} ({2}) {3}",
                                entry.Time.ToString("s"),
                                entry.Level.ToString(),
                                entry.AssemblyFile,
                                entry.Message);
                        }
                        if (Log.file == true)
                        {
                            using (FileStream fs = (File.Exists(SPath.LogFile) ? File.Open(SPath.LogFile, FileMode.Append) : File.Open(SPath.LogFile, FileMode.Create)))
                            {
                                byte[] content = Encoding.UTF8.GetBytes(string.Format("{0}\t{1}\t{2}\t{3}\r\n",
                                    entry.Time.ToString("s"),
                                    entry.Level.ToString(),
                                    entry.AssemblyFile,
                                    entry.Message));

                                fs.Write(content, 0, content.Length);
                                fs.Flush();
                                fs.Close();
                            }
                        }
                    }
                    if (Log.desiredState == LogState.Stopped)
                    {
                        break;
                    }
                    else
                    {
                        Thread.Sleep(Log.interval);
                    }
                }
            }
            Log.currentState = LogState.Stopped;
        }
        private static string Sanitize(string value)
        {
            string Output = value;
            if (value.IndexOfAny(new char[] { '\r', '\n', '\t' }) != -1)
            {
                Output = Output.Replace("\r", "\\r");
                Output = Output.Replace("\n", "\\n");
                Output = Output.Replace("\t", "\\t");
            }

            return Output;
        }
        #endregion
        #region Methods - Public
        public static void StartLogging()
        {
            Log.desiredState = LogState.Started;
            ThreadPool.QueueUserWorkItem(new WaitCallback(Log.LogWorker));
        }
        public static void StopLogging()
        {
            Log.desiredState = LogState.Stopped;
            while (Log.currentState != Log.desiredState)
            {
                Thread.Sleep(Log.Interval + (Log.Interval / 2));
            }
        }
        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="message">A string containing a description of the message.</param>
        /// <param name="level">A LogMessageLevel object describing the severity of the message.</param>
        public static void Write(string message, LogMessageLevel level)
        {
            Log.waiting.Enqueue(LogEntry.Create(message, level));
        }
        #endregion
        #region Properties - Public
        public static int Interval
        {
            get
            {
                return Log.interval;
            }
            set
            {
                if (value >= Log.MinimumInterval)
                {
                    Log.interval = value;
                }
                else
                {
                    Log.interval = Log.MinimumInterval;
                }
            }
        }
        /// <summary>
        /// Gets or sets a boolean value which determines whether log messages
        /// will be written to the console (true), or not (false).
        /// </summary>
        /// <remarks>
        /// Default value is false.
        /// </remarks>
        public static bool LogToConsole
        {
            get
            {
                return Log.console;
            }
            set
            {
                Log.console = value;
            }
        }
        /// <summary>
        /// Gets or sets a boolean value which determines whether log messages 
        /// will be saved to the log file on the file system (true), or not (false).
        /// </summary>
        /// <remarks>
        /// Default value is true.
        /// </remarks>
        public static bool LogToFile
        {
            get
            {
                return Log.file;
            }
            set
            {
                Log.file = value;
            }
        }
        #endregion
    }
    internal class LogEntry
    {
        #region Fields - Internal
        internal string Message;
        internal LogMessageLevel Level;
        internal string AssemblyFile;
        internal DateTime Time;
        #endregion;

        internal static LogEntry Create(string message, LogMessageLevel level)
        {
            LogEntry entry = new LogEntry();

            entry.Message = message;
            entry.Level = level;
            entry.Time = DateTime.UtcNow;

            if (level > LogMessageLevel.Info)
            {
                entry.AssemblyFile = Path.GetFileName(Assembly.GetCallingAssembly().Location);
            }
            else
            {
                entry.AssemblyFile = "none";
            }

            return entry;
        }
    }
}