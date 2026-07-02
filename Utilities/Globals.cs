using System.Diagnostics;
using System.Numerics;
using ImGuiNET;

namespace GFNWindowMover.Utilities;

public static class Globals
{
	public const string AppName = "GFNWindowMover2";

	public static Program? OverlayInstance { get; set; }
	public static bool ShowMenu = true;
	public static RunMode ActiveMode = RunMode.Fixed;
	public static Process? TargetProcess;
	public static string LastProcessName = string.Empty;

	public enum RunMode
	{
		Fixed,
		Wrapper
	}

	public struct VColor
	{
		public static Vector4 Red = new(FromRGBA(255, 0, 0, 255));
		public static Vector4 Green = new(FromRGBA(0, 255, 0, 255));
		public static Vector4 White = new(FromRGBA(255, 255, 255, 255));
		public static Vector4 Black = new(FromRGBA(0, 0, 0, 255));

		public static uint ToImGuiU32(Vector4 color) => ImGui.ColorConvertFloat4ToU32(color);

		private static float[] FromRGBA(int r, int g, int b, int a)
		{
			return [r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f];
		}
	}

	private static Settings? _settings;
	public static Settings Setting
	{
		get
		{
			if (_settings == null)
			{
				_settings = Settings.LoadSettings();
				LoadSettingsIntoRuntimeState();
			}
			return _settings;
		}
		set => _settings = value;
	}

	public static void LoadSettingsIntoRuntimeState()
	{
		LastProcessName = Setting.LastProcessName ?? string.Empty;
		ActiveMode = Setting.LastUseWrapperMode ? RunMode.Wrapper : RunMode.Fixed;
	}
}
