namespace GeForceNowWindowMover.Froms
{
    partial class frmSelectProcess
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSelectProcess));
            this.lvProcess = new System.Windows.Forms.ListView();
            this.ilIcons = new System.Windows.Forms.ImageList(this.components);
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvProcess
            // 
            this.lvProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvProcess.HideSelection = false;
            this.lvProcess.LargeImageList = this.ilIcons;
            this.lvProcess.Location = new System.Drawing.Point(3, 6);
            this.lvProcess.Name = "lvProcess";
            this.lvProcess.Size = new System.Drawing.Size(299, 480);
            this.lvProcess.TabIndex = 0;
            this.lvProcess.UseCompatibleStateImageBehavior = false;
            this.lvProcess.SelectedIndexChanged += new System.EventHandler(this.lvProcess_SelectedIndexChanged);
            // 
            // ilIcons
            // 
            this.ilIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.ilIcons.ImageSize = new System.Drawing.Size(32, 32);
            this.ilIcons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelect.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelect.Location = new System.Drawing.Point(204, 490);
            this.btnSelect.Margin = new System.Windows.Forms.Padding(0);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(99, 26);
            this.btnSelect.TabIndex = 1;
            this.btnSelect.Text = "Select";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(3, 489);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(0);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(99, 26);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // frmSelectProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 521);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.lvProcess);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(225, 537);
            this.Name = "frmSelectProcess";
            this.Text = "Select Process";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvProcess;
        private System.Windows.Forms.ImageList ilIcons;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnRefresh;
    }
}