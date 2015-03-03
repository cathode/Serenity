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
using System.Configuration.Install;
using System.ComponentModel;

namespace Serenity
{
    /// <summary>
    /// Installs the Serenity Web Application Server as a local machine service.
    /// </summary>
    [RunInstaller(true)]
    public sealed class SerenityServiceInstaller : Installer
    {
        public SerenityServiceInstaller()
        {
            var processInstaller = new ServiceProcessInstaller();
            var serviceInstaller = new ServiceInstaller();

            // Use LocalService unless some reason comes up that it needs more security.
            processInstaller.Account = ServiceAccount.LocalService;

            serviceInstaller.StartType = ServiceStartMode.Automatic;

            // TODO: Evaluate possibility of localizing service name and description.
            serviceInstaller.DisplayName = SerenityServiceProgram.PreferredServiceName;
            serviceInstaller.ServiceName = SerenityServiceProgram.PreferredServiceName;
            serviceInstaller.Description = SerenityServiceProgram.PreferredServiceDescription;

            this.Installers.Add(processInstaller);
            this.Installers.Add(serviceInstaller);
        }
    }
}
