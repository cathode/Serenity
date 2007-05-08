using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

using Serenity.Web.Drivers;

namespace Serenity.OperatingModes
{
    public partial class ServerControl : Form
    {
        public ServerControl()
        {
            InitializeComponent();
        }
        private void Write()
        {
            this.MessageBox.Text += "- - -\r\n";
        }
        private void Write(String message)
        {
            this.MessageBox.Text += message + "\r\n";
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            this.Write("Starting Serenity...");
            WebManager.StartAll(true);
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            this.Write("Stopping Serenity...");
            WebManager.StopAll();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}