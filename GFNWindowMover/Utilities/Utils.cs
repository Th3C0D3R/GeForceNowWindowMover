using System.Text.Json;

using static GFNWindowMover.Utilities.Globals;

namespace GFNWindowMover.Utilities;
internal static class Utils
{
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
