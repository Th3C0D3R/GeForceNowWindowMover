using System.Diagnostics;

using ClickableTransparentOverlay;

using GFNWindowMover.Utilities;

using static GFNWindowMover.Utilities.Globals;

namespace GFNWindowMover;

public partial class Program : Overlay
{
	private static Process? lastProcess;
	protected override void Render()
	{
		UI.Render();
	}

	static void Main(string[] args)
	{
		if (Setting.LastProcessName.Length > 0 && Setting.LastProcessName != null)
		{
			lastProcess = Utils.GetProcessByName(Setting.LastProcessName);
		}
		if (args.Length > 0)
		{
			if (Utils.TestObject(Utils.ArgHelper(args), false)) return;
			if (Utils.ProcessName == string.Empty && lastProcess == null)
			{

				return;
			}
			else if (Utils.ProcessName != string.Empty)
			{
				lastProcess = Utils.GetProcessByName(Utils.ProcessName);
			}
			if (lastProcess != null)
			{
				if (Utils.NoFixedWindow)
				{
					//RunWrapper(lastProcess);
				}
				else
				{
					//RunFixed(lastProcess);
				}
			}
			else
			{

				return;
			}
		}
		else
		{
			Program p = new();
			p.Start().Wait();

			OverlayInstance = p;
		}
	}
}
