/******************************************************************************
 * Serenity - Managed Web Application Server. ( http://gearedstudios.com/ )   *
 * Copyright © 2006-2015 William 'cathode' Shelley. All Rights Reserved.      *
 * This software is released under the terms and conditions of the MIT/X11    *
 * license; see the included 'license.txt' file for the full text.            *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;

namespace Serenity
{
    /// <summary>
    /// Provides the Serenity Web Application Server service implementation.
    /// </summary>
    public sealed class SerenityServiceProgram : ServiceBase
    {
        #region Fields
        public static readonly string PreferredServiceName = "Serenity";
        public static readonly string PreferredServiceDescription = "Serenity";
        private WebServer webServer;
        #endregion
        #region Constructors
        public SerenityServiceProgram()
        {
            this.ServiceName = SerenityServiceProgram.PreferredServiceName;
        }
        #endregion
        #region Methods
        public static void Main(string[] args)
        {
#if DEBUG
            ServiceController ctrl = new ServiceController(SerenityServiceProgram.PreferredServiceName);
            ctrl.Start();
            ctrl.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromMilliseconds(30000));
#else
            if (args.Length > 0)
            {
                if (args[0].Equals("install", StringComparison.OrdinalIgnoreCase))
                {
                    var installer = new SerenityServiceInstaller();
                    installer.Install(null);
                }
            }
#endif
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            this.webServer = new WebServer();
            this.webServer.Start();
        }

        protected override void OnStop()
        {
            base.OnStop();
            this.webServer.Stop();

        }
        #endregion
    }
}
