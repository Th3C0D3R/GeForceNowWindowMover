using GeForceNowWindowMover.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeForceNowWindowMover
{
    public partial class frmWrapper : Form
    {
        private Process proc = null;
        public frmWrapper(Process p)
        {
            InitializeComponent();
            proc = p;
            Utils.RePosition(proc, this);
            Utils.StateChange(proc, this, FormWindowState.Normal);
            Utils.BringToFront(proc);
            this.Text = $"{proc.MainWindowTitle} (WRAPPED)";
        }


        #region WndProc
        protected override void WndProc(ref Message m)
        {
            const int RESIZE_HANDLE_SIZE = 10;
            switch (m.Msg)
            {
                case 0x0084/*NCHITTEST*/ :
                    base.WndProc(ref m);

                    if ((int)m.Result == 0x01/*HTCLIENT*/)
                    {
                        Point screenPoint = new Point(m.LParam.ToInt32());
                        Point clientPoint = this.PointToClient(screenPoint);
                        if (clientPoint.Y <= RESIZE_HANDLE_SIZE)
                        {
                            if (clientPoint.X <= RESIZE_HANDLE_SIZE)
                                m.Result = (IntPtr)13/*HTTOPLEFT*/ ;
                            else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE))
                                m.Result = (IntPtr)12/*HTTOP*/ ;
                            else
                                m.Result = (IntPtr)14/*HTTOPRIGHT*/ ;
                        }
                        else if (clientPoint.Y <= (Size.Height - RESIZE_HANDLE_SIZE))
                        {
                            if (clientPoint.X <= RESIZE_HANDLE_SIZE)
                                m.Result = (IntPtr)10/*HTLEFT*/ ;
                            else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE))
                                m.Result = (IntPtr)2/*HTCAPTION*/ ;
                            else
                                m.Result = (IntPtr)11/*HTRIGHT*/ ;
                        }
                        else
                        {
                            if (clientPoint.X <= RESIZE_HANDLE_SIZE)
                                m.Result = (IntPtr)16/*HTBOTTOMLEFT*/ ;
                            else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE))
                                m.Result = (IntPtr)15/*HTBOTTOM*/ ;
                            else
                                m.Result = (IntPtr)17/*HTBOTTOMRIGHT*/ ;
                        }
                    }
                    return;
            }
            base.WndProc(ref m);
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= 0x20000; // <--- use 0x20000
                return cp;
            }
        }
        #endregion

        private void frmWrapper_Move(object sender, EventArgs e)
        {
            Utils.RePosition(proc, this);
        }
        private void frmWrapper_SizeChanged(object sender, EventArgs e)
        {
            Utils.Resize(proc, this);
        }
        private void frmWrapper_StyleChanged(object sender, EventArgs e)
        {
            Utils.StateChange(proc, this, this.WindowState);
        }

        private void frmWrapper_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dlres = MessageBox.Show("Do you want to exit the wrapped process too?", "Exit Process", MessageBoxButtons.YesNo);
            if(dlres == DialogResult.Yes)
            {
                if (proc != null)
                {
                    proc.Kill();
                }
            }
        }
    }
}
