using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity
{
    public static class GlobalSettings
    {
        #region Fields - Private
        private static string defaultEnvironment = SerenityInfo.SystemName;
        private static string defaultModule = SerenityInfo.SystemName;
        private static string defaultResourceClass = "dynamic";
        private static string defaultResourceName = "default";
        private static string defaultTheme = SerenityInfo.SystemName;
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
