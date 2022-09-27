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

namespace GeForceNowWindowMover
{
    internal class Program
    {
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("user32.dll")]
        private static extern int GetWindowRect(IntPtr hwnd, out Rectangle rect);
        const uint SWP_NOSIZE = 0x0001;
        const uint SWP_NOZORDER = 0x0004;


        [STAThread]
        static void Main(string[] args)
        {

            Process proc = null;
            while (proc == null)
            {
                proc = ChooseProcess();
            }
            Resize(proc);
        }

        private static Process ChooseProcess()
        {
            Console.Clear();
            if (Settings.Default.firstRun)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                FrmMessure form = new FrmMessure();

                DialogResult dlgRes = form.ShowDialog();

                if (dlgRes != DialogResult.OK)
                {
                    Application.Exit();
                    return null;
                }
                Settings.Default.firstRun = false;
                Settings.Default.Save();
            }
            Console.WriteLine("\nIf you want to resize the window, enter '-1' into the process choose menu later!!\n");
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
                Console.WriteLine($"{i}) {processes[i].MainWindowTitle}");
            }
            Console.Write("\r\nSelect a process: ");
            var input = Console.ReadLine();
            if (input.Length > processes.Length) return ChooseProcess();
            else
            {
                if (int.TryParse(input, out int num))
                {
                    if(num == -1)
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        FrmMessure form = new FrmMessure();
                        DialogResult dlgRes = form.ShowDialog();
                        if (dlgRes != DialogResult.OK) return ChooseProcess();
                        Settings.Default.firstRun = false;
                        Settings.Default.Save();
                        return ChooseProcess();
                    }
                    return processes[num];
                }
                else return ChooseProcess();
            }
        }

        static void Resize(Process proc)
        {
            Rectangle rect;
            GetWindowRect(proc.MainWindowHandle, out rect);
            SetWindowPos(proc.MainWindowHandle, IntPtr.Zero, Settings.Default.X, Settings.Default.Y, 0, 0, SWP_NOSIZE | SWP_NOZORDER);
            MoveWindow(proc.MainWindowHandle, Settings.Default.X, Settings.Default.Y, Settings.Default.Width, Settings.Default.Height, true);
            MessageBox.Show($"{proc.MainWindowTitle} successfull modified\nProgram shuting down", "Successfull modified");
        }
    }
}
