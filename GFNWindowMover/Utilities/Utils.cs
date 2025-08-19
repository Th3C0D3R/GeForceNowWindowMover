using System.Diagnostics;
using System.Text.Json;

using static GFNWindowMover.Utilities.Globals;

namespace GFNWindowMover.Utilities;
internal static class Utils
{

	#region Arguments
	public static bool NoFixedWindow { get; set; } = false;
	public static string ProcessName { get; set; } = string.Empty;
	public static int PreDefHeight { get; set; } = 720;
	public static int PreDefWidth { get; set; } = 1280;
	#endregion

	public static void Log(string message, string origin = "")
	{
		if (!string.IsNullOrWhiteSpace(origin)) origin = $"{origin}";
		else origin = $"Log";
#if DEBUG
		Debugger.Log(0, origin, message + "\n");
		Console.WriteLine(message);
#else
		Console.WriteLine(message);
#endif
	}

	public static Process? GetProcessByName(string name)
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
					if (args[i + 1].Length > 0 && !args[i + 1].StartsWith('-'))
					{
						ProcessName = args[i + 1].Trim(' ');
						returnValue = true;
					}
					break;
				//case "help":
				//case "h":
				//	PrintConsoleHelp();
				//	Console.ReadKey();
				//	returnValue = false;
				//	break;
				//case "resizeOnly":
				//case "r":
				//	CallResizeForm();
				//	returnValue = false;
				//	break;
				case "height":
					if (args[i + 1].Length > 0 && !args[i + 1].StartsWith('-'))
					{
						if (int.TryParse(args[i + 1].Trim(' '), out int height))
						{
							PreDefHeight = height;
						}
						returnValue = true;
					}
					break;
				case "width":
					if (args[i + 1].Length > 0 && !args[i + 1].StartsWith('-'))
					{
						if (int.TryParse(args[i + 1].Trim(' '), out int width))
						{
							PreDefWidth = width;
						}
						returnValue = true;
					}
					break;
				default:
					break;
			}
		}
		return returnValue;
	}
	public static bool TestObject<T>(object testobject, T expect)
	{
		if (testobject.GetType() == typeof(T))
		{
			if (((T)testobject).Equals(expect))
			{
				return true;
			}
		}
		return false;
	}
}

public class Settings
{
	public string LastProcessName { get; set; } = "notepad";
	public int LastWidth { get; set; } = 100;
	public int LastHeight { get; set; } = 200;
	public int LastGUIWidth { get; set; } = 300;
	public int LastGUIHeight { get; set; } = 400;
	public int LastGUIX { get; set; } = 0;
	public int LastGUIY { get; set; } = 0;

	public Settings()
	{ }

	public void PrepareSave()
	{
		LastProcessName = _lastProcName;
		LastWidth = Width;
		LastHeight = Height;
		LastGUIHeight = (int)GuiSize.Y;
		LastGUIWidth = (int)GuiSize.X;
		LastGUIX = (int)GuiLocation.X;
		LastGUIY = (int)GuiLocation.Y;
	}
	public void SaveSettings()
	{
		string appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppName);
		Directory.CreateDirectory(appDataFolder);

		string filePath = Path.Combine(appDataFolder, "settings.json");
		string json = JsonSerializer.Serialize(this);
		File.WriteAllText(filePath, json);
	}
	public static Settings LoadSettings()
	{
		string appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppName);
		string filePath = Path.Combine(appDataFolder, "settings.json");

		if (File.Exists(filePath))
		{
			try
			{
				string json = File.ReadAllText(filePath);
				Settings? _settings = JsonSerializer.Deserialize<Settings>(json);
				if (_settings != null) return _settings;
			}
			catch { }
			throw new Exception($"Settings File corrupted: {filePath}");
		}
		else return new();
	}
	public static void ResetSettings()
	{
		string appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppName);
		string filePath = Path.Combine(appDataFolder, "settings.json");

		if (File.Exists(filePath))
		{
			try
			{
				Setting = new();
				Setting.SaveSettings();
				return;
			}
			catch { }
			throw new Exception($"Settings File could not be deleted: {filePath}");
		}
		else
		{
			Setting = new();
			Setting.SaveSettings();
			return;
		}
	}
}
