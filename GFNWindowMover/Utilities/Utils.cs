using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;
using static GFNWindowMover.Utilities.Globals;

namespace GFNWindowMover.Utilities;

internal static class Utils
{
	private static DateTime _nextResolveAttemptUtc = DateTime.MinValue;
	private static ManagedWindowState? _managedWindowState;

	#region Arguments
	public static bool NoFixedWindow { get; set; }
	public static string ProcessName { get; set; } = string.Empty;
	public static int PreDefHeight { get; set; } = 720;
	public static int PreDefWidth { get; set; } = 1280;
	public static bool ResizeOnly { get; set; }
	#endregion

	[DllImport("user32.dll", SetLastError = true)]
	private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);
	[DllImport("user32.dll", SetLastError = true)]
	private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);
	[DllImport("user32.dll", SetLastError = true)]
	private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
	[DllImport("user32.dll", EntryPoint = "GetWindowLongPtrW", SetLastError = true)]
	private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);
	[DllImport("user32.dll", EntryPoint = "GetWindowLongW", SetLastError = true)]
	private static extern int GetWindowLong32(IntPtr hWnd, int nIndex);

	private static readonly IntPtr HWND_TOPMOST = new(-1);
	private static readonly IntPtr HWND_NOTOPMOST = new(-2);
	private const uint SWP_NOSIZE = 0x0001;
	private const uint SWP_NOMOVE = 0x0002;
	private const uint SWP_NOACTIVATE = 0x0010;
	private const int GWL_EXSTYLE = -20;
	private const int WS_EX_TOPMOST = 0x00000008;

	public static void Log(string message, string origin = "Log")
	{
#if DEBUG
		Debugger.Log(0, origin, message + "\n");
#endif
		Console.WriteLine(message);
	}

	public static Process? GetProcessByName(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return null;
		}

		Process[] processList;
		try
		{
			processList = Process.GetProcessesByName(name);
		}
		catch
		{
			return null;
		}

		foreach (var process in processList)
		{
			if (process.MainWindowTitle.Length <= 0)
			{
				continue;
			}
			if (process.MainWindowHandle != IntPtr.Zero)
			{
				return process;
			}
		}
		return null;
	}

	public static Process? ResolveTargetProcess()
	{
		if (TargetProcess != null && !TargetProcess.HasExited && TargetProcess.MainWindowHandle != IntPtr.Zero)
		{
			return TargetProcess;
		}

		DateTime now = DateTime.UtcNow;
		if (now < _nextResolveAttemptUtc)
		{
			return null;
		}

		TargetProcess = GetProcessByName(LastProcessName);
		_nextResolveAttemptUtc = now + TimeSpan.FromSeconds(1);
		return TargetProcess;
	}

	public static bool ApplyWindowRect(Process proc, int x, int y, int width, int height)
	{
		if (proc.HasExited || proc.MainWindowHandle == IntPtr.Zero)
		{
			return false;
		}
		if (width <= 0 || height <= 0)
		{
			return false;
		}
		return MoveWindow(proc.MainWindowHandle, x, y, width, height, true);
	}

	public static bool ApplyFixedWindow(Process proc)
	{
		return ApplyWindowRect(proc, Setting.FixedX, Setting.FixedY, Setting.FixedWidth, Setting.FixedHeight);
	}

	public static bool BringWindowToFront(Process proc)
	{
		if (proc.HasExited || proc.MainWindowHandle == IntPtr.Zero)
		{
			return false;
		}

		TrackManagedWindow(proc);
		return SetWindowPos(proc.MainWindowHandle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
	}

	public static void ReleaseManagedWindow(bool restoreBounds)
	{
		if (_managedWindowState == null)
		{
			return;
		}

		ManagedWindowState state = _managedWindowState.Value;
		_managedWindowState = null;

		if (state.Handle == IntPtr.Zero)
		{
			return;
		}

		if (!state.WasTopMostInitially)
		{
			SetWindowPos(state.Handle, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
		}

		if (restoreBounds && state.OriginalWidth > 0 && state.OriginalHeight > 0)
		{
			MoveWindow(state.Handle, state.OriginalX, state.OriginalY, state.OriginalWidth, state.OriginalHeight, true);
		}
	}

	private static void TrackManagedWindow(Process proc)
	{
		IntPtr handle = proc.MainWindowHandle;
		if (handle == IntPtr.Zero)
		{
			return;
		}

		if (_managedWindowState != null && _managedWindowState.Value.Handle == handle)
		{
			return;
		}

		// Switching targets: drop topmost on old one but keep its current placement.
		ReleaseManagedWindow(restoreBounds: false);

		if (!GetWindowRect(handle, out RECT rect))
		{
			return;
		}

		bool wasTopMostInitially = (GetWindowExStyle(handle) & WS_EX_TOPMOST) == WS_EX_TOPMOST;
		_managedWindowState = new ManagedWindowState(
			handle,
			wasTopMostInitially,
			rect.Left,
			rect.Top,
			rect.Right - rect.Left,
			rect.Bottom - rect.Top
		);
	}

	private static int GetWindowExStyle(IntPtr hWnd)
	{
		if (IntPtr.Size == 8)
		{
			return (int)GetWindowLongPtr64(hWnd, GWL_EXSTYLE).ToInt64();
		}
		return GetWindowLong32(hWnd, GWL_EXSTYLE);
	}

	public static object ArgHelper(string[] args)
	{
		object returnValue = false;
		for (int i = 0; i < args.Length; i++)
		{
			string arg = args[i].Trim(' ', '-');
			switch (arg)
			{
				case "nofixed":
				case "n":
					NoFixedWindow = true;
					returnValue = true;
					break;
				case "fixed":
				case "f":
					NoFixedWindow = false;
					returnValue = true;
					break;
				case "process":
				case "p":
					if (TryGetNextArg(args, i, out string processName))
					{
						ProcessName = processName;
						returnValue = true;
						i++;
					}
					break;
				case "help":
				case "h":
					PrintConsoleHelp();
					return false;
				case "resizeOnly":
				case "r":
					ResizeOnly = true;
					returnValue = true;
					break;
				case "height":
					if (TryGetNextArg(args, i, out string heightValue) && int.TryParse(heightValue, out int height))
					{
						PreDefHeight = height;
						returnValue = true;
						i++;
					}
					break;
				case "width":
					if (TryGetNextArg(args, i, out string widthValue) && int.TryParse(widthValue, out int width))
					{
						PreDefWidth = width;
						returnValue = true;
						i++;
					}
					break;
			}
		}
		return returnValue;
	}

	public static bool TestObject<T>(object testobject, T expect)
	{
		if (testobject.GetType() == typeof(T))
		{
			return ((T)testobject).Equals(expect);
		}
		return false;
	}

	public static void PrintConsoleHelp()
	{
		Console.WriteLine("Usage:");
		Console.WriteLine("  GFNWindowMover.exe [...Options]");
		Console.WriteLine();
		Console.WriteLine("Options:");
		Console.WriteLine("  --nofixed | -n                  : Use wrapper window mode");
		Console.WriteLine("  --fixed | -f                    : Use fixed position mode");
		Console.WriteLine("  --process | -p <Processname>    : Predefine process (without .exe)");
		Console.WriteLine("  --resizeOnly | -r               : Open fixed region editor mode");
		Console.WriteLine("  --height <value>                : Predefine editor height");
		Console.WriteLine("  --width <value>                 : Predefine editor width");
		Console.WriteLine("  --help | -h                     : Show this help");
		Console.WriteLine();
	}

	private static bool TryGetNextArg(string[] args, int index, out string value)
	{
		value = string.Empty;
		int nextIndex = index + 1;
		if (nextIndex >= args.Length)
		{
			return false;
		}

		string next = args[nextIndex].Trim();
		if (string.IsNullOrWhiteSpace(next) || next.StartsWith("-", StringComparison.Ordinal))
		{
			return false;
		}

		value = next;
		return true;
	}

	private readonly record struct ManagedWindowState(
		IntPtr Handle,
		bool WasTopMostInitially,
		int OriginalX,
		int OriginalY,
		int OriginalWidth,
		int OriginalHeight
	);

	[StructLayout(LayoutKind.Sequential)]
	private struct RECT
	{
		public int Left;
		public int Top;
		public int Right;
		public int Bottom;
	}
}

public class Settings
{
	public string LastProcessName { get; set; } = string.Empty;
	public bool LastUseWrapperMode { get; set; }

	public bool FirstRun { get; set; } = true;

	public int FixedX { get; set; } = 0;
	public int FixedY { get; set; } = 0;
	public int FixedWidth { get; set; } = 900;
	public int FixedHeight { get; set; } = 600;

	public int WrapperX { get; set; } = 0;
	public int WrapperY { get; set; } = 0;
	public int WrapperWidth { get; set; } = 900;
	public int WrapperHeight { get; set; } = 600;

	public void SaveSettings()
	{
		string appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppName);
		Directory.CreateDirectory(appDataFolder);

		string filePath = Path.Combine(appDataFolder, "settings.json");
		string json = JsonSerializer.Serialize(this);
		if (File.Exists(filePath))
		{
			string existingJson = File.ReadAllText(filePath);
			if (string.Equals(existingJson, json, StringComparison.Ordinal))
			{
				return;
			}
		}
		File.WriteAllText(filePath, json);
	}

	public static Settings LoadSettings()
	{
		string appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppName);
		string filePath = Path.Combine(appDataFolder, "settings.json");

		if (!File.Exists(filePath))
		{
			return new();
		}

		try
		{
			string json = File.ReadAllText(filePath);
			Settings? settings = JsonSerializer.Deserialize<Settings>(json);
			return settings ?? new();
		}
		catch (Exception ex)
		{
			throw new Exception($"Settings file is corrupted: {filePath}", ex);
		}
	}

	public static void ResetSettings()
	{
		Setting = new();
		Setting.SaveSettings();
	}
}
