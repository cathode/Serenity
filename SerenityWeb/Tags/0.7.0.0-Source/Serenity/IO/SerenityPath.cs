using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace Serenity.IO
{
    /// <summary>
    /// Provides Serenity-specific methods for working with path strings.
    /// </summary>
    public static class SerenityPath
    {
        #region Constructors - Private
        static SerenityPath()
        {
            SerenityPath.applicationDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            SerenityPath.workingDirectory = Directory.GetCurrentDirectory();
        }
        #endregion
        #region Fields - Private
        private static readonly string applicationDirectory;
        private static string workingDirectory;
        #endregion
        #region Methods - Public
        public static string Combine(params string[] segments)
        {
            string result = segments[0];
            for (int i = 1; i < segments.Length; i++)
            {
                result = Path.Combine(result, segments[i]);
            }
            return result;
        }
        #endregion
        #region Properties - Public
        /// <summary>
        /// Gets the directory where the application is installed.
        /// </summary>
        public static string ApplicationDirectory
        {
            get
            {
                return SerenityPath.applicationDirectory;
            }
        }
        /// <summary>
        /// Gets or sets the working directory of the application.
        /// </summary>
        public static string WorkingDirectory
        {
            get
            {
                return SerenityPath.workingDirectory;
            }
            set
            {
                SerenityPath.workingDirectory = value;
            }
        }
        /// <summary>
        /// Gets the directory where module assemblies are stored.
        /// </summary>
        public static string ModulesDirectory
        {
            get
            {
                return Path.Combine(SerenityPath.ApplicationDirectory, "modules");
            }
        }
        /// <summary>
        /// Gets the directory where log files are stored.
        /// </summary>
        public static string LogsDirectory
        {
            get
            {
                return Path.Combine(SerenityPath.WorkingDirectory, "logs");
            }
        }
        /// <summary>
        /// Gets the directory where database files are stored.
        /// </summary>
        public static string DataDirectory
        {
            get
            {
                return Path.Combine(SerenityPath.WorkingDirectory, "data");
            }
        }
        /// <summary>
        /// Gets the directory where configuration files are stored.
        /// </summary>
        public static string ConfigurationDirectory
        {
            get
            {
                return Path.Combine(SerenityPath.WorkingDirectory, "configuration");
            }
        }
        #endregion
    }
}
