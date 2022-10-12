using GeForceNowWindowMover.Froms;
using GeForceNowWindowMover.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
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

        #region Properties 

        #region Calculation Size/Position
        public static int OffsetX { get; set; } = 38;
        public static int OffsetY { get; set; } = 16;
        public static int InnerWidth { get; set; } = 32;
        public static int InnerHeight { get; set; } = 54;
        public static int SpaceBorder { get; set; } = 15;
        #endregion

        #region Arguments
        public static bool nofixedWindow { get; set; } = false;
        public static string processName { get; set; } = string.Empty;
        #endregion

        #endregion

        #region Methods
        public static Process GetProcessByName(string name)
        {
            var processList = Process.GetProcesses();
            foreach (var process in processList)
            {
                if (process.MainWindowTitle.Length <= 0) continue;
                if (process.ProcessName == name)
                {
                    return process;
                }
            }
            return null;
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
                MoveWindow(proc.MainWindowHandle, frm.Location.X + (OffsetX / 2), frm.Location.Y + (OffsetY + SpaceBorder), InnerWidth, InnerHeight, true);
            }
            else
            {
                MoveWindow(proc.MainWindowHandle, Settings.Default.X, Settings.Default.Y, Settings.Default.Width, Settings.Default.Height, true);
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
        public static void MinimizeConsole()
        {
            IntPtr hWndConsole = GetConsoleWindow();
            if (hWndConsole != IntPtr.Zero)
            {
                ShowWindow(hWndConsole, SW_HIDE);
            }
        }
        public static void PrintConsoleHelp()
        {
            Console.WriteLine($"");
            Console.WriteLine($"Usage: ");
            Console.WriteLine($"  {Path.GetFileName(Assembly.GetEntryAssembly().Location)} (--nofixed | -n)");
            Console.WriteLine($"  {Path.GetFileName(Assembly.GetEntryAssembly().Location)} (--nofixed | -n) --process|-p <Processname>");
            Console.WriteLine($"  {Path.GetFileName(Assembly.GetEntryAssembly().Location)} (--fixed | -f)");
            Console.WriteLine($"  {Path.GetFileName(Assembly.GetEntryAssembly().Location)} (--fixed | -f) --process|-p <Processname>");
            Console.WriteLine($"");
            Console.WriteLine($"");
            Console.WriteLine($"PROCESSNAME WITHOUT .EXE/EXTENSION (e.g: GeForceNOW instead of GeForceNOW.exe");
            Console.WriteLine($"");
        }
        public static Process UserChooseProcess()
        {
            Process proc = null;
            if (Settings.Default.lastProcess.Length > 0 && Settings.Default.lastProcess != null)
            {
                proc = Utils.GetProcessByName(Settings.Default.lastProcess);
            }
            else
            {
                frmSelectProcess selectProcess = new frmSelectProcess();
                DialogResult dialog = selectProcess.ShowDialog();
                if (dialog != DialogResult.OK)
                {
                    return null;
                }
                proc = selectProcess.selProc;
            }
            return proc;
        }
        public static object ArgHelper(string[] args)
        {
            object returnValue = false;
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i].Trim(' ','-');
                switch (arg)
                {
                    case "nofixed":
                    case "n":
                        nofixedWindow = true;
                        returnValue = true;
                        break;
                    case "fixed":
                    case "f":
                        nofixedWindow = false;
                        returnValue = true;
                        break;
                    case "process":
                    case "p":
                        if (args[i+1].Length > 0 && !args[i+1].StartsWith("-"))
                        {
                            processName = args[i + 1].Trim(' ');
                            returnValue = true;
                        }
                        break;
                    case "help":
                    case "h":
                        PrintConsoleHelp();
                        Console.ReadKey();
                        returnValue = false;
                        break;
                    case "resizeOnly":
                    case "r":
                        callResizeForm();
                        returnValue = false;
                        break;
                    default:
                        returnValue = false;
                        break;
                }
            }
            return returnValue;
        }
        public static bool TestObject<t>(object testobject, t expect)
        {
            if (testobject.GetType() == typeof(t))
            {
                if (((t)testobject).Equals(expect))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
