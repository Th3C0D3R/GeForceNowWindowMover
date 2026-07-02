using ClickableTransparentOverlay;
using GFNWindowMover.Utilities;
using static GFNWindowMover.Utilities.Globals;
using System.Threading.Tasks;

namespace GFNWindowMover;

public partial class Program : Overlay
{
	public Program()
	{
		OverlayInstance = this;
	}

	protected override void Render()
	{
		UI.Render();
	}

	static void Main(string[] args)
	{
		AppDomain.CurrentDomain.ProcessExit += (_, _) => Utils.ReleaseManagedWindow(restoreBounds: true);
		AppDomain.CurrentDomain.UnhandledException += (_, _) => Utils.ReleaseManagedWindow(restoreBounds: true);
		TaskScheduler.UnobservedTaskException += (_, e) =>
		{
			Utils.ReleaseManagedWindow(restoreBounds: true);
			e.SetObserved();
		};
		Console.CancelKeyPress += (_, _) => Utils.ReleaseManagedWindow(restoreBounds: true);

		_ = Setting;

		if (args.Length > 0 && Utils.TestObject(Utils.ArgHelper(args), false))
		{
			return;
		}

		if (!string.IsNullOrWhiteSpace(Utils.ProcessName))
		{
			LastProcessName = Utils.ProcessName;
			Setting.LastProcessName = Utils.ProcessName;
		}
		else
		{
			LastProcessName = Setting.LastProcessName ?? string.Empty;
		}

		ActiveMode = Utils.NoFixedWindow ? RunMode.Wrapper : ActiveMode;
		if (args.Length > 0 && !Utils.NoFixedWindow)
		{
			ActiveMode = RunMode.Fixed;
		}

		if (Utils.ResizeOnly)
		{
			ActiveMode = RunMode.Fixed;
			UI.OpenFixedEditorOnStart = true;
		}

		Setting.LastUseWrapperMode = ActiveMode == RunMode.Wrapper;
		Setting.SaveSettings();

		TargetProcess = Utils.GetProcessByName(LastProcessName);

		Program p = new();
		p.Start().Wait();
	}
}
