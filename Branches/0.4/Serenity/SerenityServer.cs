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
using System.Text;

using Serenity.Properties;
using Serenity.Themes;
using Serenity.Web.Drivers;
using Serenity.Xml;

using Serenity.Web;

namespace Serenity
{
    /// <summary>
    /// Performs core startup tasks.
    /// </summary>
    public static class SerenityServer
    {
        /// <summary>
        /// Loads environments and modules and assigns defaults.
        /// </summary>
        public static void Run()
        {
            Theme theme = new Theme(SerenityInfo.SystemName);
            theme.AccentA.TextColor.Value = "114 124 163";
            theme.AccentB.TextColor.Value = "159 184 205";
            theme.AccentC.TextColor.Value = "210 218 122";
            theme.AccentD.TextColor.Value = "250 218 122";
            theme.AccentE.TextColor.Value = "184 132 114";
            theme.AccentF.TextColor.Value = "145 115 106";
            theme.ContentA.TextColor.Value = "0 0 0";
            theme.ContentA.BackgroundColor.Value = "251 251 251";
            theme.ContentB.TextColor.Value = "70 70 83";
            theme.ContentB.BackgroundColor.Value = "231 243 246";

            Border border = theme.HeadingA.Border;

            border.Top.BorderType = BorderType.Dotted;
            border.Top.Width.Value = 1;

            border.Left.BorderType = BorderType.Dotted;
            border.Left.Width.Value = 1;

            theme.HeadingA.Padding.Top.Value = 8;
            theme.HeadingA.Padding.Left.Value = 16;

            Theme.SystemInstance = theme;

            SerenityModule.SystemInstance = SerenityModule.LoadModuleFile("serenity.dll", SerenityInfo.SystemName);

            SerenityEnvironment.LoadAllEnvironments();
            if (SerenityEnvironment.ContainsInstance(SerenityInfo.SystemName) == true)
            {
                SerenityEnvironment.SystemInstance = SerenityEnvironment.GetInstance(SerenityInfo.SystemName);
            }

            SerenityModule.TryLoadAllModules();

            /*
            if ((Settings.Default.ActiveThemes != null) && (Settings.Default.ActiveThemes.Count == 0))
            {
                foreach (string name in Settings.Default.ActiveThemes)
                {
                    TryResult<Theme> result = Theme.TryLoadTheme(name);
                    if (result.IsSuccessful == true)
                    {
                        Log.Write("Loaded theme '" + name + "'.", LogMessageLevel.Info);
                    }
                    else
                    {
                        Log.Write("Failed to load theme '" + name + "'.", LogMessageLevel.Warning);
                    }
                }
                Theme.DefaultInstance = Theme.GetInstance(Settings.Default.DefaultTheme);
            }
            */

            Log.Write(string.Format("Loaded: {0} environments, {1} modules, {2} themes.",
                SerenityEnvironment.Instances.Length,
                SerenityModule.Instances.Length,
                Theme.Instances.Length), LogMessageLevel.Info);
            WebDriverSettings InitSettings = WebDriverSettings.Create(80, 1000);
            InitSettings.RecieveInterval = 0;
            InitSettings.FallbackPorts = new ushort[] { 8080, 8081 };

            WebManager.AddDriver(new HttpDriver(new ContextHandler()));
            WebManager.Initialize<HttpDriver>(InitSettings);
            WebManager.StartAll();

            Log.Write("Server shutting down", LogMessageLevel.Info);
        }
    }
}
