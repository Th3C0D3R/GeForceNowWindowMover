﻿namespace GeForceNowWindowMover
{
    partial class frmWrapper
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWrapper));
            this.pnlInner = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pnlInner
            // 
            resources.ApplyResources(this.pnlInner, "pnlInner");
            this.pnlInner.BackColor = System.Drawing.Color.White;
            this.pnlInner.Name = "pnlInner";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // frmWrapper
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pnlInner);
            this.Name = "frmWrapper";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.TransparencyKey = System.Drawing.Color.White;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmWrapper_FormClosing);
            this.SizeChanged += new System.EventHandler(this.frmWrapper_SizeChanged);
            this.Move += new System.EventHandler(this.frmWrapper_Move);
            this.StyleChanged += new System.EventHandler(this.frmWrapper_StyleChanged);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel pnlInner;
        private System.Windows.Forms.Label label1;
    }
}