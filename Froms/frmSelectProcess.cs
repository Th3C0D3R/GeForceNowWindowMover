using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeForceNowWindowMover.Froms
{
    public partial class frmSelectProcess : Form
    {
        public Process selProc { get; set; } = null;
        private Process selProcPrivat = null;

        public frmSelectProcess()
        {
            InitializeComponent();
            RefreshProcessList();
        }

        private void lvProcess_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lvProcess.SelectedItems.Count > 0)
            {
                ListViewItem lvProcessItem = (ListViewItem)lvProcess.SelectedItems[0];
                if(lvProcessItem.Tag != null && lvProcessItem.Tag.GetType() == typeof(Process))
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

        private void btnSelect_Click(object sender, EventArgs e)
        {
            selProc = selProcPrivat;
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            lvProcess.Items.Clear();
            ilIcons.Images.Clear();
            RefreshProcessList();
        }
    }
}
