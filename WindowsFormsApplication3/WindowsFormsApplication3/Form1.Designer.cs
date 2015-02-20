using System;

namespace WindowsFormsApplication3
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
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
            this.Start1 = new System.Windows.Forms.Button();
            this.Start2 = new System.Windows.Forms.Button();
			this.tbX = new System.Windows.Forms.TextBox ();
			this.tbY = new System.Windows.Forms.TextBox ();
			this.addButton = new System.Windows.Forms.Button ();
            this.SuspendLayout();
            // 
            // Start1
            // 
            this.Start1.Location = new System.Drawing.Point(527, 12);
            this.Start1.Name = "Start1";
            this.Start1.Size = new System.Drawing.Size(75, 23);
            this.Start1.TabIndex = 0;
            this.Start1.Text = "Start 1";
            this.Start1.UseVisualStyleBackColor = true;
            this.Start1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Start2
            // 
            this.Start2.Location = new System.Drawing.Point(527, 41);
            this.Start2.Name = "Start2";
            this.Start2.Size = new System.Drawing.Size(75, 23);
            this.Start2.TabIndex = 1;
            this.Start2.Text = "Start 2";
            this.Start2.UseVisualStyleBackColor = true;
            this.Start2.Click += new System.EventHandler(this.button2_Click);
			//
			// Forsøg på x-koordinat textbox
			//
			this.tbX.Location = new System.Drawing.Point (527, 70);
			this.tbX.Name = "X-koordinat";
			this.tbX.Size = new System.Drawing.Size (75, 23);
			this.tbX.TabIndex = 2;
			this.tbX.Text = "X-koordinat";
			this.Controls.Add (this.tbX);
			//
			//Forsøg på y-koordinat
			//
			this.tbY.Location = new System.Drawing.Point (527, 100);
			this.tbY.Name = "Y-koordinat";
			this.tbY.Size = new System.Drawing.Size (75, 23);
			this.tbY.TabIndex = 3;
			this.tbY.Text = "Y-koordinat";
			this.Controls.Add (this.tbY);
			//
			// addButton
			//
			this.addButton.Location = new System.Drawing.Point (527, 130);
			this.addButton.Name = "Add";
			this.addButton.Size = new System.Drawing.Size (90, 23);
			this.addButton.TabIndex = 2;
			this.addButton.Text = "Add";
			this.addButton.UseVisualStyleBackColor = true;
			this.addButton.Click += new System.EventHandler (this.addButton_Click);
			this.Controls.Add (this.addButton);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 521);
            this.Controls.Add(this.Start2);
            this.Controls.Add(this.Start1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Start1;
        private System.Windows.Forms.Button Start2;
		public System.Windows.Forms.TextBox tbX;
		public System.Windows.Forms.TextBox tbY;
		private System.Windows.Forms.Button addButton;
    }
}

