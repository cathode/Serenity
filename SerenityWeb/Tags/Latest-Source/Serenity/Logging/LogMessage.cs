using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Serenity.Logging
{
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
