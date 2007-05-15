using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity
{
    public static class GlobalSettings
    {
        #region Fields - Private
        private static string defaultEnvironment = GlobalSettings.SystemName;
        private static string defaultModule = GlobalSettings.SystemName;
        private static string defaultResourceClass = GlobalSettings.SystemName;
        private static string defaultResourceName = GlobalSettings.SystemName;
        private static string defaultTheme = GlobalSettings.SystemName;
        #endregion
        #region Fields - Public
        public const string SystemName = "system";
        #endregion
        #region Properties - Public
        public static string DefaultEnvironment
        {
            get
            {
                return GlobalSettings.defaultEnvironment;
            }
        }
        public static string DefaultModule
        {
            get
            {
                return GlobalSettings.defaultModule;
            }
        }
        public static string DefaultResourceClass
        {
            get
            {
                return GlobalSettings.defaultResourceClass;
            }
        }
        public static string DefaultResourceName
        {
            get
            {
                return GlobalSettings.defaultResourceName;
            }
        }
        public static string DefaultTheme
        {
            get
            {
                return GlobalSettings.defaultTheme;
            }
        }
        #endregion
    }
}
