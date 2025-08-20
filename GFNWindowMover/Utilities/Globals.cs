using System.Numerics;

using ImGuiNET;

namespace GFNWindowMover.Utilities;
public static class Globals
{
	public const string AppName = "GFNWindowMover 2";

	public static IntPtr CmdHandle { get; set; } = IntPtr.Zero;
	public static Vector2 WindowLocation { get; set; } = default;
	public static Vector2 WindowSize { get; set; } = default;
	public static Vector2 GuiLocation { get; set; } = default;
	public static Vector2 GuiSize { get; set; } = default;
	public static Program? OverlayInstance { get; set; } = null;

	public static bool ShowMenu = true;
	public static bool ProcessLoaded = false;

	public static VColor ColorSelection = new();

	public static int Width = 100;
	public static int Height = 100;
	public static int Left = 0;
	public static int Top = 0;

	public static string _lastProcName = "";

	public static int ScreenSelect = 0;

	public enum XColor
	{
		Red = 0xff0000,
		Green = 0x00ff00,
		Blue = 0x0000ff,
		Black = 0x000000,
		White = 0xffffff
	}
	public struct VColor
	{
		public static Vector4 Red = new(FromRGBA(255, 0, 0, 255));
		public static Vector4 Green = new(FromRGBA(0, 255, 0, 255));
		public static Vector4 Blue = new(FromRGBA(0, 0, 255, 255));
		public static Vector4 Black = new(FromRGBA(0, 0, 0, 255));
		public static Vector4 White = new(FromRGBA(255, 255, 255, 255));
		public static Vector4 Wheat = new(FromRGBA(245, 222, 179, 255));

		public Vector4 Selected = default;

		public static explicit operator uint(VColor a)
		{
			return ImGui.ColorConvertFloat4ToU32(a.Selected);
		}

		public static uint SelectColor(Vector4 a)
		{
			ColorSelection.Selected = a;
			return (uint)ColorSelection;
		}
		private static float[] FromRGBA(int r, int g, int b, int a)
		{
			return [r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f];
		}

		public VColor()
		{ }
	}

	private static Settings? _settings;
	public static Settings Setting
	{
		get
		{
			if (_settings == null)
			{
				_settings = Settings.LoadSettings();
				LoadSettingIntoUI();
			}
			return _settings;
		}
		set { _settings = value; }
	}
	public static void LoadSettingIntoUI()
	{
		_lastProcName = Setting.LastProcessName;
		Width = Setting.LastWidth;
		Height = Setting.LastHeight;
	}
}
