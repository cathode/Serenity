/*
Serenity - The next evolution of web server technology

Copyright © 2006-2007 Serenity Project (http://serenityproject.net/)

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
        static Log()
        {
            if (Directory.Exists(SPath.LogsFolder) == false)
            {
                Directory.CreateDirectory(SPath.LogsFolder);
            }
        }
        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="MessageText">A string containing a description of the message.</param>
        /// <param name="Level">A LogMessageLevel object describing the severity of the message.</param>
        public static void Write(string MessageText, LogMessageLevel Level)
        {
            MessageText = Log.Sanitize(MessageText);
            DateTime When = DateTime.UtcNow;

            string AssemblyFile;
            if (Level > LogMessageLevel.Warning)
            {
                AssemblyFile = Path.GetFileName(Assembly.GetCallingAssembly().Location);
            }
            else
            {
                AssemblyFile = "none";
            }

            StringBuilder Output = new StringBuilder();
            Output.AppendFormat("{0}\t{1}\t{2}\t{3}\r\n",
                When.ToString("s"),
                (Byte)Level,
                AssemblyFile,
                MessageText);

            if (Log.console == true)
            {
                Console.WriteLine("[{0}] {1} ({2}) {3}",
                    When.ToString("s"),
                    Level.ToString(),
                    AssemblyFile,
                    MessageText);
            }
            if (Log.file == true)
            {
                Byte[] WriteContent = Encoding.UTF8.GetBytes(Output.ToString());
                Log.mutex.WaitOne();
                using (FileStream OutputStream = File.Open(SPath.LogFile, FileMode.Append))
                {
                    OutputStream.Write(WriteContent, 0, WriteContent.Length);
                    OutputStream.Flush();
                    OutputStream.Close();
                }
                Log.mutex.ReleaseMutex();
            }
        }
        private static string Sanitize(string Input)
        {
            string Output = Input;
            if (Input.IndexOfAny(new char[] { '\r', '\n', '\t' }) != -1)
            {
                Output = Output.Replace("\r", "\\r");
                Output = Output.Replace("\n", "\\n");
                Output = Output.Replace("\t", "\\t");
            }

            return Output;
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
        private static bool console = false;
        private static bool file = true;
        private static Mutex mutex = new Mutex();
    }
}
