using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;

using Serenity;
using Serenity.Themes;
using Serenity.Web.Drivers;

namespace Serenity.OperatingModes
{
    public sealed class ServiceMode : ServiceBase
    {
        public ServiceMode()
        {
            this.ServiceName = "SerenityService";

            Log.LogToConsole = false;
            Log.Write("Service is starting", LogMessageLevel.Info);

        }

        protected override void OnStart(String[] args)
        {
            WebDriverSettings InitSettings = new WebDriverSettings();
            InitSettings.ListenPort = 80;
            InitSettings.RecieveTimeout = 200;
            InitSettings.RecieveInterval = 10;
            InitSettings.RecieveIntervalIdle = InitSettings.RecieveInterval * 4;
            InitSettings.RecieveTimeoutIdle = InitSettings.RecieveTimeout * 8;
            InitSettings.TimeToIdle = InitSettings.RecieveIntervalIdle * 2;
            InitSettings.FallbackPorts = new ushort[] { 8080, 8081 };

            WebManager.AddDriver(new HttpDriver(new ContextHandler()));
            WebManager.Initialize<HttpDriver>(InitSettings);
            WebManager.Start<HttpDriver>();
        }

        protected override void OnStop()
        {
            WebManager.StopAll();
        }
    }
}
