using ImGuiNET;

using static GFNWindowMover.Utilities.Globals;
using static GFNWindowMover.Utilities.Utils;

namespace GFNWindowMover;
public static class UI
{
	public static void Render()
	{
		if (ShowMenu)
		{
			DrawMenu();
			DrawOverlay();
		}
		ImGui.End();
	}
	static void DrawMenu()
	{
		ImGui.SetNextWindowBgAlpha(10.0f);
		ImGui.Begin("GFNWindowMover 2 - by TH30D3R");

		GuiSize = ImGui.GetWindowSize();
		GuiLocation = ImGui.GetWindowPos();
		ImGui.SetWindowPos(GuiLocation);
		ImGui.SetWindowSize(GuiSize);
		ImGui.TextColored(VColor.Wheat, $"Last Processname: {Setting.LastProcessName}");
		ImGui.SameLine(ImGui.GetWindowSize().X - 40);
		if (ImGui.Button("X"))
		{
			Environment.Exit(0);
			return;
		}
		ImGui.TextColored(ProcessLoaded ? VColor.Green : VColor.Red, $"Process found: {ProcessLoaded}");
		if (ImGui.BeginMenuBar())
		{
			if (ImGui.BeginMenu("Program"))
			{
				if (ImGui.MenuItem("Restart"))
				{

				}
				if (ImGui.MenuItem("Exit"))
				{
					Environment.Exit(0);
					return;
				}
				ImGui.EndMenu();
			}
			if (ImGui.BeginMenu("Settings"))
			{
				if (ImGui.MenuItem("Save")) { /* Handle Save */ }
				ImGui.EndMenu();
			}
			ImGui.EndMenuBar();
		}
	}

	static void DrawSettings()
	{

	}

	static void DrawOverlay()
	{

	}
}
