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
        #region DllImport
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("user32.dll")]
        private static extern int GetWindowRect(IntPtr hwnd, out Rectangle rect);
        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern IntPtr GetConsoleWindow();
        [DllImport("User32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow([In] IntPtr hWnd, [In] Int32 nCmdShow);
        #endregion

        #region Constants
        const uint SWP_NOZORDER = 0x0004;
        const uint SWP_SHOWWINDOW = 0x0040;
        const uint SWP_HIDEWINDOW = 0x0080;
        const int SW_MINIMIZE = 6;
        const int SW_HIDE = 0;
        #endregion

        #region Properties Calculation Size/Position
        public static int OffsetX { get; set; } = 38;
        public static int OffsetY { get; set; } = 16;
        public static int InnerWidth { get; set; } = 32;
        public static int InnerHeight { get; set; } = 54;
        public static int SpaceBorder { get; set; } = 15;
        #endregion

        public static Process ChooseProcess()
        {
            Process[] processes = Process.GetProcessesByName("GeForceNOW");
            while (processes.Length == 0)
            {
                Console.WriteLine("No GeForceNOW process found!\nPlease start the game first\nThen press any key in console to continue!");
                Console.ReadKey();
                processes = Process.GetProcessesByName("GeForceNOW");
            }
            Console.WriteLine("Choose the Process matching the Window-Title (usually '*Gamename* at GeForce NOW')");
            for (int i = 0; i < processes.Length; i++)
            {
                if (processes[i].MainWindowTitle.Length <= 0) continue;
                Console.WriteLine($"{i + 1}) {processes[i].MainWindowTitle}");
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
                    return processes[num - 1];
                }
                else return ChooseProcess();
            }
        }
        public static void callResizeForm()
        {
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
            if (frm != null)
            {
                OffsetX = frm.ClientRectangle.Width - frm.pnlInner.Width;
                OffsetY = frm.ClientRectangle.Height - frm.pnlInner.Height;
                InnerWidth = frm.pnlInner.Width + SpaceBorder;
                InnerHeight = frm.pnlInner.Height + SpaceBorder - 6;
                MoveWindow(proc.MainWindowHandle, frm.Location.X + (OffsetX/2), frm.Location.Y + (OffsetY + SpaceBorder), InnerWidth, InnerHeight, true);
            }
            else
            {
                MoveWindow(proc.MainWindowHandle, Settings.Default.X, Settings.Default.Y, Settings.Default.Width, Settings.Default.Height, true);
                MessageBox.Show($"{proc.MainWindowTitle} successfull modified\nProgram shuting down", "Successfull modified");
            }
        }
        public static void StateChange(Process proc, frmWrapper frm, FormWindowState state)
        {
            if (frm != null)
            {
                uint flag = state == FormWindowState.Minimized ? SWP_HIDEWINDOW : SWP_SHOWWINDOW;
                SetWindowPos(proc.MainWindowHandle, IntPtr.Zero, frm.Location.X + OffsetX / 2, frm.Location.Y + OffsetY, InnerWidth, InnerHeight, SWP_NOZORDER | flag);
            }
        }
        public static Rectangle GetWindowSize(Process proc)
        {
            GetWindowRect(proc.MainWindowHandle, out Rectangle size);
            return size;
        }
        public static void MinimizeConsole()
        {
            IntPtr hWndConsole = GetConsoleWindow();
            if(hWndConsole != IntPtr.Zero)
            {
                ShowWindow(hWndConsole, SW_HIDE);
            }
        }
    }
}
