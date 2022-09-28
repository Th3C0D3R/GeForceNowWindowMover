using GeForceNowWindowMover.Helper;
using GeForceNowWindowMover.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeForceNowWindowMover
{
    internal class Program
    {


        [STAThread]
        static void Main(string[] args)
        {
            Console.Clear();
            int option = -1;
            Console.WriteLine("Please select the method to modify the game windo: ");
            Console.WriteLine($"1 ) Wrapper Form (the GeForce Now window will be wrapped inside a form which can be moved and resized)");
            Console.WriteLine($"2 ) Fixed Position and Size (predefine a fixed position and size for the GeForce Now window)");
            Console.Write("\r\nSelect a option: ");
            var input = Console.ReadLine();
            if (!int.TryParse(input, out option)) Main(args);
            if (option == 1)
            {
                Process proc = null;
                while (proc == null)
                {
                    proc = Utils.ChooseProcess();
                }
                RunWrapper(proc);
            }
            else if(option == 2)
            {
                RunFixed();
            }
            else
            {
                Main(args);
            }
        }

        private static void RunWrapper(Process proc)
        {
            frmWrapper frmWrapper = new frmWrapper(proc);
            frmWrapper.ShowDialog();
        }

        private static void RunFixed(Process proc = null)
        {
            Console.WriteLine("\nIf you want to resize the window, enter '-1' into the 'Choose Process'-Menu later!!\n");
            if(proc == null)
            {
                while (proc == null)
                {
                    proc = Utils.ChooseProcess();
                }
            }
            while (Settings.Default.firstRun)
            {
                Utils.callResizeForm();
            }            
            Utils.Resize(proc);
        }
    }
}
