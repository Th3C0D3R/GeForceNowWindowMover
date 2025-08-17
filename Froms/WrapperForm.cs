using GeForceNowWindowMover.Helper;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace GeForceNowWindowMover
{
    public partial class FrmWrapper : Form
    {
        private readonly Process proc = null;
        public FrmWrapper(Process p)
        {
            InitializeComponent();
            this.proc = p;
            Utils.StateChange(this.proc, this, FormWindowState.Normal);
            this.Text = $"{proc.MainWindowTitle} (WRAPPED)";
        }

        private void FrmWrapper_Move(object sender, EventArgs e) => Utils.Resize(this.proc, this);
        private void FrmWrapper_SizeChanged(object sender, EventArgs e) => Utils.Resize(this.proc, this);
        private void FrmWrapper_StyleChanged(object sender, EventArgs e) => Utils.StateChange(this.proc, this, this.WindowState);
        private void FrmWrapper_Load(object sender, EventArgs e) => Utils.Resize(this.proc, this);
        private void FrmWrapper_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dlres = MessageBox.Show("Do you want to exit the wrapped process too?", "Exit Process", MessageBoxButtons.YesNo);
            if (dlres == DialogResult.Yes)
            {
                this.proc?.Kill();
            }
        }

        public void LoadSettings()
        {
            this.Height = Utils.Wrapped_Height;
            this.Width = Utils.Wrapped_Width;
            this.Location = new System.Drawing.Point(Utils.Wrapped_X,Utils.Wrapped_Y);
        }
    }
}
