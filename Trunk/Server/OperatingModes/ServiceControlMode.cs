using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;

namespace Serenity.OperatingModes
{
    public partial class ServiceControlMode : Form
    {
        //private ServiceInstaller installer = new ServiceInstaller();

        public ServiceControlMode()
        {
            InitializeComponent();
            //installer.Description = "Runs Serenity as a system service.";
            //installer.DisplayName = "Serenity Service";
        }
        private void Write()
        {
            this.MessageBox.Text += "- - -\r\n";
        }
        private void Write(String message)
        {
            this.MessageBox.Text += message + "\r\n";
        }

        private void InstallButton_Click(object sender, EventArgs e)
        {
            this.Write("Attempting to install the Serenity service...");

            //WS: insert code to install service here, I don't know what to do.
            

            this.Write("Failed: Not implemented yet.");
            this.Write();
        }

        private void UninstallButton_Click(object sender, EventArgs e)
        {
            this.Write("Attempting to uninstall the Serenity service...");

            this.Write("Failed: Not implemented yet.");
            this.Write();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            this.Write("Attempting to start the Serenity service...");

            this.Write("Failed: Not implemented yet.");
            this.Write();
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            this.Write("Attempting to stop the Serenity service...");

            this.Write("Failed: Not implemented yet.");
            this.Write();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}