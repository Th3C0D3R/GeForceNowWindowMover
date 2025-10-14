using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using GFNWindowMover.Utilities;

using ImGuiNET;

using Microsoft.VisualBasic;

using Veldrid.MetalBindings;

using static GFNWindowMover.Utilities.Globals;
using static GFNWindowMover.Utilities.Utils;

namespace GFNWindowMover;
public static class UI
{
    static bool showProcessPopup = false;
    static string selectedProcess = "";
    static List<(string WindowTitle, string ProcessName, Screen? Screen)> processList = new();
    static int selectedIndex = -1;

    public static void Render()
    {
        InitStuff();
        if (ShowMenu)
        {
            DrawMenu();
            DrawOverlay();
        }
        ImGui.End();
    }
    static void DrawMenu()
    {
        ImGui.Begin("Example Overlay", ImGuiWindowFlags.MenuBar);

        // Menu Bar
        if (ImGui.BeginMenuBar())
        {
            if (ImGui.BeginMenu("Program"))
            {
                if (ImGui.MenuItem("Restart"))
                {
                    RestartApplication();
                }
                if (ImGui.MenuItem("Exit"))
                {
                    Environment.Exit(0);
                }
                ImGui.EndMenu();
            }

            if (ImGui.BeginMenu("Settings"))
            {
                if (ImGui.MenuItem("Choose Process"))
                {
                    LoadProcesses();
                    showProcessPopup = true;
                }
                ImGui.EndMenu();
            }

            ImGui.EndMenuBar();
        }

        ImGui.Text($"Selected Process: {selectedProcess}");

        // Process Selection Popup
        if (showProcessPopup)
        {
            ImGui.OpenPopup("Choose Process");
            //showProcessPopup = false;
        }

        if (ImGui.BeginPopupModal("Choose Process", ref showProcessPopup, ImGuiWindowFlags.AlwaysAutoResize))
        {
            ImGui.Text("Select a process:");

            if (ImGui.BeginListBox("##processList", new Vector2(400, 300)))
            {
                for (int i = 0; i < processList.Count; i++)
                {
                    bool isSelected = (i == selectedIndex);
                    if (ImGui.Selectable($"{processList[i].WindowTitle} ({processList[i].ProcessName})", isSelected))
                    {
                        selectedIndex = i;
                    }
                    if (isSelected)
                        ImGui.SetItemDefaultFocus();
                }
                ImGui.EndListBox();
            }

            if (ImGui.Button("Select"))
            {
                if (selectedIndex >= 0)
                {
                    selectedProcess = processList[selectedIndex].ProcessName;
                    showProcessPopup = false;
                }
                ImGui.CloseCurrentPopup();
            }
            ImGui.SameLine();
            if (ImGui.Button("Cancel"))
            {
                showProcessPopup = false;
                ImGui.CloseCurrentPopup();
            }

            ImGui.EndPopup();
        }

        ImGui.End();
    }

    static void DrawOverlay()
    {
        //var size = SystemParameters
        ImGui.SetNextWindowSize(Globals.WindowSize);
        ImGui.SetNextWindowPos(Globals.WindowLocation);
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


    static void RestartApplication()
    {
        Process.Start(Process.GetCurrentProcess().MainModule?.FileName ?? Process.GetCurrentProcess().StartInfo.FileName);
        Environment.Exit(0);
    }

    static void LoadProcesses()
    {
        processList.Clear();
        EnumWindows((hWnd, lParam) =>
        {
            if (!IsWindowVisible(hWnd)) return true;

            int length = GetWindowTextLength(hWnd);
            if (length == 0) return true;

            StringBuilder sb = new(length + 1);
            GetWindowText(hWnd, sb, sb.Capacity);

            GetWindowThreadProcessId(hWnd, out uint pid);
            var proc = Process.GetProcessById((int)pid);

            if (string.IsNullOrWhiteSpace(sb.ToString()))
                return true;

            if (processList.Any(l => l.ProcessName == proc.ProcessName))
                return true;

            // 🖥️ Get window rectangle
            if (GetWindowRect(hWnd, out RECT rect))
            {
                var windowCenter = new System.Drawing.Point(
                    rect.Left + (rect.Right - rect.Left) / 2,
                    rect.Top + (rect.Bottom - rect.Top) / 2
                );

                // Find which screen contains this point
                var screen = Screen.AllScreens.FirstOrDefault(s => s.Bounds.Contains(windowCenter));

                processList.Add((sb.ToString(), proc.ProcessName, screen));
            }


            return true;
        }, IntPtr.Zero);
    }

    static void InitStuff()
    {
        Program.SetWindowData(Globals.ScreenSelect);
    }

    #region WinAPI
    // WinAPI declarations
    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    [DllImport("user32.dll")]
    private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

    [DllImport("user32.dll")]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    [DllImport("user32.dll")]
    private static extern int GetWindowTextLength(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    [StructLayout(LayoutKind.Sequential)]
    struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
    #endregion
}
