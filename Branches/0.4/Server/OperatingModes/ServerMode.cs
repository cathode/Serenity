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
using System.Text;
using System.Threading;

using Serenity;
using Serenity.Themes;
using Serenity.Web;
using Serenity.Web.Drivers;

namespace Server.OperatingModes
{
    internal class ServerMode
    {
        internal static void Run()
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

            SerenityEnvironment.LoadAllEnvironments();
            if (SerenityEnvironment.ContainsInstance(SerenityInfo.SystemName) == true)
            {
                SerenityEnvironment.SystemInstance = SerenityEnvironment.GetInstance(SerenityInfo.SystemName);
            }

            Module.LoadAllModules();
            Module module = Module.GetModule("system");

            Log.Write(string.Format("Loaded: {0} environments, {1} modules, {2} themes.",
                SerenityEnvironment.Instances.Length,
                Module.ModuleCount,
                Theme.Instances.Length), LogMessageLevel.Info);
			WebDriverSettings settings = new WebDriverSettings();
			settings.Block = true;
			settings.ContextHandler = new ContextHandler();
            settings.Ports = new ushort[] { 80, 8080, 8081 };

            WebDriver driver = new HttpDriver(settings);

            driver.Initialize();

			if (!driver.Start())
			{
				Log.Write("Failed to start web driver", LogMessageLevel.Warning);
			}
			else
			{
				while (driver.Status == WebDriverStatus.Started)
				{
					Thread.Sleep(1000);
				}
			}

            Log.Write("Server shutting down", LogMessageLevel.Info);
            Console.WriteLine("Press any key...");
            Console.Read();
        }
    }
}
