using GeForceNowWindowMover.Properties;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace GeForceNowWindowMover.Froms
{
    public partial class FrmSelectProcess : Form
    {
        public Process SelProc { get; set; } = null;
        private Process selProcPrivat = null;

        public FrmSelectProcess()
        {
            InitializeComponent();
            RefreshProcessList();
        }

        private void LvProcess_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvProcess.SelectedItems.Count > 0)
            {
                ListViewItem lvProcessItem = (ListViewItem)lvProcess.SelectedItems[0];
                if (lvProcessItem.Tag != null && lvProcessItem.Tag.GetType() == typeof(Process))
                {
                    selProcPrivat = (Process)lvProcessItem.Tag;
                }
            }
        }

        private void RefreshProcessList()
        {
            var processList = Process.GetProcesses();
            foreach (var process in processList)
            {
                if (process.MainWindowTitle.Length <= 0) continue;
                string fullPath = process.MainModule.FileName;
                if (System.IO.File.Exists(fullPath))
                {
                    var icon = Icon.ExtractAssociatedIcon(fullPath);
                    var key = process.Id.ToString();
                    this.ilIcons.Images.Add(key, icon.ToBitmap());
                    ListViewItem lvi = new ListViewItem()
                    {
                        Text = process.ProcessName,
                        ImageKey = key,
                        Tag = process
                    };
                    this.lvProcess.Items.Add(lvi);
                }
            }
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            SelProc = selProcPrivat;
            Settings.Default.lastProcess = SelProc.ProcessName;
            Settings.Default.Save();
        }
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            lvProcess.Items.Clear();
            ilIcons.Images.Clear();
            RefreshProcessList();
        }
    }
}
