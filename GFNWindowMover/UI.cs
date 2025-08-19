using System.Numerics;

using ImGuiNET;

using Veldrid.MetalBindings;

using static GFNWindowMover.Utilities.Globals;
using static GFNWindowMover.Utilities.Utils;

namespace GFNWindowMover;
public static class UI
{
	static bool _ChooseProcessVisible;
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
		var wa = Screen.PrimaryScreen?.WorkingArea ?? Screen.AllScreens[0].WorkingArea;
		ImGui.SetNextWindowBgAlpha(10.0f);
		ImGui.Begin("GFNWindowMover 2 - by TH30D3R", ImGuiWindowFlags.MenuBar | ImGuiWindowFlags.NoCollapse);

		ImGui.SetWindowPos(new(wa.Left+10,wa.Top+10));
		ImGui.SetWindowSize(ImGui.GetWindowSize());

		ImGui.TextColored(VColor.Wheat, $"Last Processname: {Setting.LastProcessName}");
		ImGui.SameLine(ImGui.GetWindowSize().X - 40);
		if (ImGui.Button("X"))
		{
			Environment.Exit(0);
			return;
		}
		ImGui.TextColored(ProcessLoaded ? VColor.Green : VColor.Red, $"Process found: {ProcessLoaded}");

		if (ImGui.BeginMainMenuBar())
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
				if (ImGui.MenuItem("Choose Process"))
				{
					ImGui.OpenPopup("Choose Process", ImGuiPopupFlags.AnyPopup);
					_ChooseProcessVisible = true;
					DrawSettings();

				}
				ImGui.EndMenu();
			}
			ImGui.EndMainMenuBar();
		}
		ImGui.End();
	}

	static void DrawSettings()
	{
		if (_ChooseProcessVisible)
		{
			ImGui.SetNextWindowSize(new System.Numerics.Vector2(300, 150), ImGuiCond.FirstUseEver);
			if (ImGui.BeginPopupModal("Choose Process", ref _ChooseProcessVisible, ImGuiWindowFlags.NoCollapse))
			{
				float windowWidth = ImGui.GetWindowWidth();
				ImGui.SameLine(windowWidth - 25);
				if (ImGui.SmallButton("X"))
				{
					ImGui.CloseCurrentPopup();
					_ChooseProcessVisible = false;
				}

				ImGui.Separator();

				ImGui.Text("This is my popup content!");

				ImGui.Dummy(new Vector2(0, 10));

				if (ImGui.Button("Close"))
				{
					ImGui.CloseCurrentPopup();
					_ChooseProcessVisible = false;
				}

				ImGui.EndPopup();
			}
			else
			{
				_ChooseProcessVisible = false;
			}
			ImGui.End();
		}

	}

	static void DrawOverlay()
	{
		//var size = SystemParameters
		ImGui.SetNextWindowSize(WindowSize);
		ImGui.SetNextWindowPos(WindowLocation);
		ImGui.Begin("Overlay", ImGuiWindowFlags.NoDecoration
			| ImGuiWindowFlags.NoBackground
			| ImGuiWindowFlags.NoBringToFrontOnFocus
			| ImGuiWindowFlags.NoMove
			| ImGuiWindowFlags.NoInputs
			| ImGuiWindowFlags.NoCollapse
			| ImGuiWindowFlags.NoScrollbar
			| ImGuiWindowFlags.NoScrollWithMouse);

		if (ImGui.IsKeyPressed(ImGuiKey.End))
		{
			Environment.Exit(0);
			return;
		}
	}
}
