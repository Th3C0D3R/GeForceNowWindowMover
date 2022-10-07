using GeForceNowWindowMover.Froms;
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
			private bool fixedWindow = true;

		  [STAThread]
		  static void Main(string[] args)
		  {
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Console.SetWindowSize(113, 19);
				Console.SetBufferSize(113, 3000);
				if(args.Length > 0)
				{
					for (int i = 0; i < args.Length; i++)
					{
						var arg = args[i].Split("=");
						if(arg[0] == "fixed" || arg[0] == "f")
						{
							
						}
						else if(arg[0] == "process" || arg[0] == "p")
						{

						}
					}
				}
				Menu();
		  }

		  private static void Menu()
		  {
				Console.Clear();
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
					 Process proc = null;
					 frmSelectProcess selectProcess = new frmSelectProcess();
					 DialogResult dialog = selectProcess.ShowDialog();
					 if (dialog != DialogResult.OK) Menu();
					 proc = selectProcess.selProc;
					 RunWrapper(proc);
				}
				else if (option == 2)
				{
					 RunFixed();
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
				frmWrapper.ShowDialog();
		  }

		  private static void RunFixed(Process proc = null)
		  {
				if(proc == null)
				{
					 frmSelectProcess selectProcess = new frmSelectProcess();
					 selectProcess.ShowDialog();
					 proc = selectProcess.selProc;
				}
				while (Settings.Default.firstRun)
				{
					 Utils.callResizeForm();
				}            
				Utils.Resize(proc);
		  }
	 }
}
