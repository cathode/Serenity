using System;

namespace Serenity.OperatingModes
{
    partial class ServerControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.StartButton = new System.Windows.Forms.Button();
            this.StopButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.MessageBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(12, 12);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(96, 24);
            this.StartButton.TabIndex = 2;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // StopButton
            // 
            this.StopButton.Location = new System.Drawing.Point(12, 42);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(96, 24);
            this.StopButton.TabIndex = 3;
            this.StopButton.Text = "Stop";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(12, 172);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(96, 24);
            this.CloseButton.TabIndex = 5;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // MessageBox
            // 
            this.MessageBox.Location = new System.Drawing.Point(114, 12);
            this.MessageBox.Multiline = true;
            this.MessageBox.Name = "MessageBox";
            this.MessageBox.ReadOnly = true;
            this.MessageBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.MessageBox.Size = new System.Drawing.Size(266, 184);
            this.MessageBox.TabIndex = 6;
            // 
            // ControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 208);
            this.Controls.Add(this.MessageBox);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.StartButton);
            this.MaximizeBox = false;
            this.Name = "ControlForm";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Serenity Server Control";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.TextBox MessageBox;
    }
}

