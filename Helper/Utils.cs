using GeForceNowWindowMover.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeForceNowWindowMover.Helper
{

    public static class Utils
    {
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("user32.dll")]
        private static extern int GetWindowRect(IntPtr hwnd, out Rectangle rect); 
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        const uint SWP_NOSIZE = 0x0001;
        const uint SWP_NOZORDER = 0x0004;
        const uint SWP_SHOWWINDOW = 0x0040;
        const uint SWP_HIDEWINDOW = 0x0080;
        const float offsetX = 5f;
        const int offsetY = 30;
        const int offsetWidth = (int)(2 * offsetX) + 3;
        const int offsetHeight = (int)(offsetX +offsetY) + 2;

        public static Process ChooseProcess()
        {
            Process[] processes = Process.GetProcessesByName("notepad++");
            while (processes.Length == 0)
            {
                Console.WriteLine("No GeForceNOW process found!\nPlease start the game first\nThen press any key in console to continue!");
                Console.ReadKey();
                processes = Process.GetProcessesByName("notepad++");
            }
            Console.WriteLine("Choose the Process matching the Window-Title (usually '*Gamename* at GeForce NOW')");
            for (int i = 0; i < processes.Length; i++)
            {
                if (processes[i].MainWindowTitle.Length <= 0) continue;
                Console.WriteLine($"{i+1}) {processes[i].MainWindowTitle}");
            }
            Console.Write("\r\nSelect a process: ");
            var input = Console.ReadLine();
            if (input.Length > processes.Length) return ChooseProcess();
            else
            {
                if (int.TryParse(input, out int num))
                {
                    if (num == -1)
                    {
                        Settings.Default.firstRun = true;
                        Settings.Default.Save();
                        ChooseProcess();
                    }
                    return processes[num-1];
                }
                else return ChooseProcess();
            }
        }

        public static void callResizeForm()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FrmMessure form = new FrmMessure();

            DialogResult dlgRes = form.ShowDialog();

            if (dlgRes != DialogResult.OK && Settings.Default.firstRun)
            {
                return;
            }
            Settings.Default.firstRun = false;
            Settings.Default.Save();
        }

        public static void Resize(Process proc, frmWrapper frm = null)
        {
            if(frm != null)
            {
                SetWindowPos(proc.MainWindowHandle, IntPtr.Zero, frm.Location.X + (int)offsetX, frm.Location.Y + offsetY, 0, 0, SWP_NOSIZE | SWP_NOZORDER);
                MoveWindow(proc.MainWindowHandle, frm.Location.X + (int)offsetX, frm.Location.Y + offsetY, frm.Width - offsetWidth, frm.Height - offsetHeight, true);
            }
            else
            {
                SetWindowPos(proc.MainWindowHandle, IntPtr.Zero, Settings.Default.X, Settings.Default.Y, 0, 0, SWP_NOSIZE | SWP_NOZORDER);
                MoveWindow(proc.MainWindowHandle, Settings.Default.X, Settings.Default.Y, Settings.Default.Width, Settings.Default.Height, true);
                MessageBox.Show($"{proc.MainWindowTitle} successfull modified\nProgram shuting down", "Successfull modified");
            }
        }
        public static void RePosition(Process proc, frmWrapper frm)
        {
            if(frm != null)
            {
                SetWindowPos(proc.MainWindowHandle, IntPtr.Zero, frm.Location.X + (int)offsetX, frm.Location.Y + offsetY, 0, 0, SWP_NOSIZE | SWP_NOZORDER);
                MoveWindow(proc.MainWindowHandle, frm.Location.X + (int)offsetX, frm.Location.Y + offsetY, frm.Width - offsetWidth, frm.Height - offsetHeight, true);
            }
        }
        public static void StateChange(Process proc, frmWrapper frm, FormWindowState state)
        {
            if (frm != null)
            {
                uint flag = state == FormWindowState.Minimized ? SWP_HIDEWINDOW : SWP_SHOWWINDOW;
                SetWindowPos(proc.MainWindowHandle, IntPtr.Zero, frm.Location.X + (int)offsetX, frm.Location.Y + offsetY, 0, 0, SWP_NOSIZE | SWP_NOZORDER | flag);
                MoveWindow(proc.MainWindowHandle, frm.Location.X + (int)offsetX, frm.Location.Y + offsetY, frm.Width - offsetWidth, frm.Height - offsetHeight, true);
            }
        }
        public static void BringToFront(Process proc)
        {
            SetForegroundWindow(proc.MainWindowHandle);
        }
    }
}
