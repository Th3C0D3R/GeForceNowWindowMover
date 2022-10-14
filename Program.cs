using GeForceNowWindowMover.Helper;
using GeForceNowWindowMover.Properties;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace GeForceNowWindowMover
{
    internal class Program
    {
        private static Process lastProcess = null;

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Console.SetWindowSize(113, 19);
            Console.SetBufferSize(113, 3000);
            if (Settings.Default.lastProcess.Length > 0 || Settings.Default.lastProcess != null)
            {
                lastProcess = Utils.GetProcessByName(Settings.Default.lastProcess);
            }
            if (args.Length > 0)
            {
                if (Utils.TestObject(Utils.ArgHelper(args), false)) return;
                if (Utils.ProcessName == String.Empty && lastProcess == null)
                {
                    Settings.Default.lastProcess = String.Empty;
                    Settings.Default.Save();
                    Console.WriteLine($"No Process with the name {Utils.ProcessName} found and no last process saved!\nPlease make sure the process already started", "No Processname set");
                    Utils.PrintConsoleHelp();
                    return;
                }
                else if (Utils.ProcessName != String.Empty)
                {
                    lastProcess = Utils.GetProcessByName(Utils.ProcessName);
                }
                if (lastProcess != null)
                {
                    if (Utils.NoFixedWindow)
                    {
                        RunWrapper(lastProcess);
                    }
                    else
                    {
                        RunFixed(lastProcess);
                    }
                }
                else
                {
                    Settings.Default.lastProcess = String.Empty;
                    Settings.Default.Save();
                    Console.WriteLine($"No Process with the name {Utils.ProcessName} or {Settings.Default.lastProcess} found!\nPlease make sure the process already started", "No Process found");
                    Utils.PrintConsoleHelp();
                    return;
                }
            }
            else
            {
                Menu();
            }
        }

        private static void Menu()
        {
            Console.Clear();
            Process proc = null;
            int option = -1;
            Console.WriteLine("If you want to resize the window (fixed position and size), enter '-1' into the next menu!!\n\n");
            Console.WriteLine("Please select the method to modify the game window: ");
            Console.WriteLine($"1 ) [BUGGY] Wrapper Form (the GeForce Now window will be wrapped inside a form which can be moved and resized)");
            Console.WriteLine($"2 ) [RECOMMENDED] Fixed Position and Size (predefine a fixed position and size for the GeForce Now window)");
            Console.Write("\r\nSelect a option: ");
            var input = Console.ReadLine();
            if (!int.TryParse(input, out option)) Menu();
            if (option == 1)
            {
                proc = Utils.UserChooseProcess();
                if (proc != null)
                {
                    RunWrapper(proc);
                }
            }
            else if (option == 2)
            {
                proc = Utils.UserChooseProcess();
                if (proc == null) Menu();
                else RunFixed(proc);
            }
            else if (option == -1)
            {
                Settings.Default.firstRun = true;
                Settings.Default.Save();
                Menu();
            }
            else
            {
                Menu();
            }
        }

        private static void RunWrapper(Process proc)
        {
            Utils.MinimizeConsole();
            frmWrapper frmWrapper = new frmWrapper(proc);
            frmWrapper.LoadSettings();
            frmWrapper.ShowDialog();
        }

        private static void RunFixed(Process proc)
        {
            while (Settings.Default.firstRun)
            {
                Utils.callResizeForm();
            }
            Utils.Resize(proc);
        }
    }
}
