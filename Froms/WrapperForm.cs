using GeForceNowWindowMover.Helper;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace GeForceNowWindowMover
{
    public partial class frmWrapper : Form
    {
        private Process proc = null;
        public frmWrapper(Process p)
        {
            InitializeComponent();
            this.proc = p;
            Utils.StateChange(this.proc, this, FormWindowState.Normal);
            this.Text = $"{proc.MainWindowTitle} (WRAPPED)";
        }

        private void frmWrapper_Move(object sender, EventArgs e) => Utils.Resize(this.proc, this);
        private void frmWrapper_SizeChanged(object sender, EventArgs e) => Utils.Resize(this.proc, this);
        private void frmWrapper_StyleChanged(object sender, EventArgs e) => Utils.StateChange(this.proc, this, this.WindowState);
        private void frmWrapper_Load(object sender, EventArgs e) => Utils.Resize(this.proc, this);
        private void frmWrapper_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dlres = MessageBox.Show("Do you want to exit the wrapped process too?", "Exit Process", MessageBoxButtons.YesNo);
            if (dlres == DialogResult.Yes)
            {
                if (this.proc != null)
                {
                    this.proc.Kill();
                }
            }
        }
    }
}
