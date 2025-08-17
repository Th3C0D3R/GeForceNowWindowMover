namespace GeForceNowWindowMover
{
    partial class FrmWrapper
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmWrapper));
			this.pnlInner = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// pnlInner
			// 
			resources.ApplyResources(this.pnlInner, "pnlInner");
			this.pnlInner.BackColor = System.Drawing.Color.White;
			this.pnlInner.Name = "pnlInner";
			// 
			// FrmWrapper
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Silver;
			this.Controls.Add(this.pnlInner);
			this.Name = "FrmWrapper";
			this.TopMost = true;
			this.TransparencyKey = System.Drawing.Color.White;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmWrapper_FormClosing);
			this.Load += new System.EventHandler(this.FrmWrapper_Load);
			this.SizeChanged += new System.EventHandler(this.FrmWrapper_SizeChanged);
			this.Move += new System.EventHandler(this.FrmWrapper_Move);
			this.StyleChanged += new System.EventHandler(this.FrmWrapper_StyleChanged);
			this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel pnlInner;
    }
}