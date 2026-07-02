using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using GFNWindowMover.Utilities;

using ImGuiNET;

using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using static GFNWindowMover.Utilities.Globals;

namespace GFNWindowMover;

public static class UI
{
    private static bool _showProcessPopup;
    private static int _selectedIndex = -1;
    private static readonly List<ProcessItem> _processList = [];
    private static bool _showFixedEditor;
    private static bool _showWrapperWindow;
    private static bool _initialized;
    private static string _statusMessage = string.Empty;
    private static bool _isWorkingEnabled = true;
    private static DateTime _lastWrapperSaveUtc = DateTime.MinValue;
    private static Rect _wrapperInnerRect = new(0, 0, 0, 0);
    private static readonly long _moveIntervalTicks = Stopwatch.Frequency / 60;
    private static long _nextMoveTick;
    private static Rect _lastAppliedRect = new(0, 0, 0, 0);
    private static IntPtr _lastAppliedHandle = IntPtr.Zero;
    private static Rect _lastOverlayRect = new(0, 0, 0, 0);

    public static bool OpenFixedEditorOnStart { get; set; }

    public static void Render()
    {
        EnsureInitialized();
        EnsureOverlayCoversVirtualDesktop();
        DrawControlWindow();
        DrawProcessPopup();
        DrawFixedEditor();
        DrawWrapperWindow();
        ApplyWindowMove();
    }

    private static void EnsureInitialized()
    {
        if (_initialized)
        {
            return;
        }

        _showFixedEditor = Setting.FirstRun || OpenFixedEditorOnStart;
        _showWrapperWindow = ActiveMode == RunMode.Wrapper;
        _initialized = true;
    }

    private static void EnsureOverlayCoversVirtualDesktop()
    {
        if (OverlayInstance == null)
        {
            return;
        }

        var virtualScreen = SystemInformation.VirtualScreen;
        var targetRect = new Rect(virtualScreen.X, virtualScreen.Y, virtualScreen.Width, virtualScreen.Height);
        if (_lastOverlayRect == targetRect)
        {
            return;
        }

        OverlayInstance.Position = new System.Drawing.Point(targetRect.X, targetRect.Y);
        OverlayInstance.Size = new System.Drawing.Size(targetRect.Width, targetRect.Height);
        _lastOverlayRect = targetRect;
    }

    private static void DrawControlWindow()
    {
        ImGui.SetNextWindowSize(new Vector2(640, 350), ImGuiCond.FirstUseEver);
        ImGui.Begin("GFN WindowMover", ImGuiWindowFlags.MenuBar);

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
                if (ImGui.MenuItem("Reset Saved Settings"))
                {
                    Settings.ResetSettings();
                    LoadSettingsIntoRuntimeState();
                    _showFixedEditor = true;
                    _showWrapperWindow = false;
                    _statusMessage = "Settings reset.";
                }
                ImGui.EndMenu();
            }
            ImGui.EndMenuBar();
        }

        if (ImGui.Button(_isWorkingEnabled ? "Disable" : "Enable"))
        {
            _isWorkingEnabled = !_isWorkingEnabled;
            _statusMessage = _isWorkingEnabled ? "Window lock enabled." : "Window lock disabled.";
        }
        ImGui.SameLine();
        ImGui.Text($"Selected Process: {(string.IsNullOrWhiteSpace(LastProcessName) ? "<none>" : LastProcessName)}");
        ImGui.SameLine();
        if (ImGui.Button("Choose Process"))
        {
            LoadProcesses();
            _showProcessPopup = true;
        }
        ImGui.SameLine();
        if (ImGui.Button("Refresh Process"))
        {
            TargetProcess = Utils.GetProcessByName(LastProcessName);
        }
        
        
        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        bool isFixed = ActiveMode == RunMode.Fixed;
        if (ImGui.RadioButton("Fixed Position/Size", isFixed))
        {
            SetMode(RunMode.Fixed);
        }
        ImGui.SameLine();
        if (ImGui.RadioButton("Wrapper Window", !isFixed))
        {
            SetMode(RunMode.Wrapper);
        }

        ImGui.Spacing();
        if (ImGui.Button("Open Fixed Region Editor"))
        {
            _showFixedEditor = true;
            _showWrapperWindow = false;
        }
        ImGui.SameLine();
        if (ImGui.Button("Start Fixed Mode"))
        {
            SetMode(RunMode.Fixed);
            _showFixedEditor = false;
            _showWrapperWindow = false;
        }
        ImGui.SameLine();
        if (ImGui.Button("Start Wrapper Mode"))
        {
            SetMode(RunMode.Wrapper);
            _showWrapperWindow = true;
            _showFixedEditor = false;
        }

        ImGui.Spacing();
        if (!string.IsNullOrWhiteSpace(_statusMessage))
        {
            ImGui.TextColored(VColor.Green, _statusMessage);
        }

        if (ImGui.IsKeyPressed(ImGuiKey.End))
        {
            Environment.Exit(0);
        }

        ImGui.End();
    }

    private static void DrawProcessPopup()
    {
        if (_showProcessPopup)
        {
            ImGui.OpenPopup("Choose Process");
        }

        if (!ImGui.BeginPopupModal("Choose Process", ref _showProcessPopup, ImGuiWindowFlags.AlwaysAutoResize))
        {
            return;
        }

        ImGui.Text("Select a visible process window:");
        if (ImGui.BeginListBox("##processList", new Vector2(650, 300)))
        {
            for (int i = 0; i < _processList.Count; i++)
            {
                bool isSelected = i == _selectedIndex;
                if (ImGui.Selectable($"{_processList[i].WindowTitle} ({_processList[i].ProcessName})", isSelected))
                {
                    _selectedIndex = i;
                }
                if (isSelected)
                {
                    ImGui.SetItemDefaultFocus();
                }
            }
            ImGui.EndListBox();
        }

        if (ImGui.Button("Select") && _selectedIndex >= 0)
        {
            ProcessItem item = _processList[_selectedIndex];
            LastProcessName = item.ProcessName;
            if (!string.Equals(Setting.LastProcessName, LastProcessName, StringComparison.OrdinalIgnoreCase))
            {
                Setting.LastProcessName = LastProcessName;
                Setting.SaveSettings();
            }
            TargetProcess = Utils.GetProcessByName(LastProcessName);
            _statusMessage = $"Selected process: {LastProcessName}";
            _showProcessPopup = false;
            ImGui.CloseCurrentPopup();
        }
        ImGui.SameLine();
        if (ImGui.Button("Cancel"))
        {
            _showProcessPopup = false;
            ImGui.CloseCurrentPopup();
        }

        ImGui.EndPopup();
    }

    private static void DrawFixedEditor()
    {
        if (!_showFixedEditor)
        {
            return;
        }

        ImGui.SetNextWindowPos(ToOverlaySpace(new Vector2(Setting.FixedX, Setting.FixedY)), ImGuiCond.FirstUseEver);
        ImGui.SetNextWindowSize(new Vector2(Setting.FixedWidth, Setting.FixedHeight), ImGuiCond.FirstUseEver);

        if (!ImGui.Begin("Fixed Region Editor", ref _showFixedEditor, ImGuiWindowFlags.NoCollapse))
        {
            ImGui.End();
            return;
        }

        ImGui.Text("Move and resize this window to the target region.");
        if (ImGui.Button("Save Region (keeps this window open)"))
        {
            Vector2 pos = ToAbsoluteDesktopSpace(ImGui.GetWindowPos());
            Vector2 size = ImGui.GetWindowSize();

            Setting.FixedX = (int)pos.X;
            Setting.FixedY = (int)pos.Y + 65;
            Setting.FixedWidth = (int)size.X;
            Setting.FixedHeight = (int)size.Y - 65;
            Setting.FirstRun = false;
            Setting.LastUseWrapperMode = ActiveMode == RunMode.Wrapper;
            Setting.SaveSettings();
            ActiveMode = RunMode.Fixed;
            Process? target = Utils.ResolveTargetProcess();
            if (target != null)
                Utils.BringWindowToFront(target);
            _statusMessage = "Fixed region saved.";
        }
        ImGui.SameLine();
        if (ImGui.Button("Save + Run Fixed (closes this window)"))
        {
            Vector2 pos = ToAbsoluteDesktopSpace(ImGui.GetWindowPos());
            Vector2 size = ImGui.GetWindowSize();

            Setting.FixedX = (int)pos.X;
            Setting.FixedY = (int)pos.Y + 65;
            Setting.FixedWidth = (int)size.X;
            Setting.FixedHeight = (int)size.Y - 65;
            Setting.FirstRun = false;
            Setting.LastUseWrapperMode = false;
            Setting.SaveSettings();
            SetMode(RunMode.Fixed);
            _showFixedEditor = false;
            _statusMessage = "Fixed mode started.";
        }
        Vector2 innerPos = ImGui.GetCursorScreenPos();
        Vector2 innerSize = ImGui.GetContentRegionAvail();
        if (innerSize.X > 20 && innerSize.Y > 20)
        {
            ImGui.BeginChild("##WrapperInner", innerSize, ImGuiChildFlags.Borders);
            ImGui.Text("This area will be the size and position the window will be displayed");
            ImGui.EndChild();
        }
        ImGui.End();
    }

    private static void DrawWrapperWindow()
    {
        if (ActiveMode != RunMode.Wrapper || !_showWrapperWindow)
        {
            return;
        }

        ImGui.SetNextWindowPos(ToOverlaySpace(new Vector2(Setting.WrapperX, Setting.WrapperY)), ImGuiCond.FirstUseEver);
        ImGui.SetNextWindowSize(new Vector2(Setting.WrapperWidth, Setting.WrapperHeight), ImGuiCond.FirstUseEver);

        if (!ImGui.Begin("Wrapper Window", ref _showWrapperWindow, ImGuiWindowFlags.NoCollapse))
        {
            ImGui.End();
            return;
        }

        ImGui.Text("The selected process is continuously moved into the inner area below.");
        Vector2 innerPos = ImGui.GetCursorScreenPos();
        Vector2 innerSize = ImGui.GetContentRegionAvail();
        if (innerSize.X > 20 && innerSize.Y > 20)
        {
            ImGui.BeginChild("##WrapperInner", innerSize, ImGuiChildFlags.Borders);
            ImGui.Text(" ");
            ImGui.EndChild();
            Vector2 absInnerPos = ToAbsoluteDesktopSpace(innerPos);
            _wrapperInnerRect = new Rect((int)absInnerPos.X, (int)absInnerPos.Y, (int)innerSize.X, (int)innerSize.Y);
        }

        Vector2 wrapperPos = ToAbsoluteDesktopSpace(ImGui.GetWindowPos());
        Vector2 wrapperSize = ImGui.GetWindowSize();
        TryPersistWrapperLayout((int)wrapperPos.X, (int)wrapperPos.Y, (int)wrapperSize.X, (int)wrapperSize.Y);

        ImGui.End();
    }

    private static void ApplyWindowMove()
    {
        if (!_isWorkingEnabled)
        {
            return;
        }

        Process? target = Utils.ResolveTargetProcess();
        if (target == null)
        {
            return;
        }

        Rect targetRect;
        switch (ActiveMode)
        {
            case RunMode.Fixed:
                targetRect = new Rect(Setting.FixedX, Setting.FixedY, Setting.FixedWidth, Setting.FixedHeight);
                break;
            case RunMode.Wrapper:
                if (_wrapperInnerRect.Width > 0 && _wrapperInnerRect.Height > 0)
                {
                    targetRect = _wrapperInnerRect;
                    break;
                }
                return;
            default:
                return;
        }

        IntPtr handle = target.MainWindowHandle;
        if (targetRect.Width <= 0 || targetRect.Height <= 0 || handle == IntPtr.Zero)
        {
            return;
        }

        long now = Stopwatch.GetTimestamp();
        bool sameWindow = handle == _lastAppliedHandle;
        bool sameRect = targetRect == _lastAppliedRect;
        if (sameWindow && sameRect && now < _nextMoveTick)
        {
            return;
        }

        if (Utils.ApplyWindowRect(target, targetRect.X, targetRect.Y, targetRect.Width, targetRect.Height))
        {
            if (ActiveMode == RunMode.Wrapper)
            {
                Utils.BringWindowToFront(target);
            }
            _lastAppliedHandle = handle;
            _lastAppliedRect = targetRect;
            _nextMoveTick = now + _moveIntervalTicks;
        }
    }

    private static void RestartApplication()
    {
        Process.Start(Process.GetCurrentProcess().MainModule?.FileName ?? Process.GetCurrentProcess().StartInfo.FileName);
        Environment.Exit(0);
    }

    private static void LoadProcesses()
    {
        _processList.Clear();
        _selectedIndex = -1;

        EnumWindows((hWnd, _) =>
        {
            if (!IsWindowVisible(hWnd))
            {
                return true;
            }

            int length = GetWindowTextLength(hWnd);
            if (length == 0)
            {
                return true;
            }

            StringBuilder sb = new(length + 1);
            GetWindowText(hWnd, sb, sb.Capacity);
            if (string.IsNullOrWhiteSpace(sb.ToString()))
            {
                return true;
            }

            GetWindowThreadProcessId(hWnd, out uint pid);
            Process proc;
            try
            {
                proc = Process.GetProcessById((int)pid);
            }
            catch
            {
                return true;
            }

            if (_processList.Any(p => p.ProcessName.Equals(proc.ProcessName, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            _processList.Add(new ProcessItem(sb.ToString(), proc.ProcessName, hWnd));
            if (proc.ProcessName.Equals(LastProcessName, StringComparison.OrdinalIgnoreCase))
            {
                _selectedIndex = _processList.Count - 1;
            }

            return true;
        }, IntPtr.Zero);
    }

    private static void TryPersistWrapperLayout(int x, int y, int width, int height)
    {
        if (Setting.WrapperX == x &&
            Setting.WrapperY == y &&
            Setting.WrapperWidth == width &&
            Setting.WrapperHeight == height)
        {
            return;
        }

        Setting.WrapperX = x;
        Setting.WrapperY = y;
        Setting.WrapperWidth = width;
        Setting.WrapperHeight = height;

        DateTime now = DateTime.UtcNow;
        if (now - _lastWrapperSaveUtc >= TimeSpan.FromSeconds(1))
        {
            Setting.SaveSettings();
            _lastWrapperSaveUtc = now;
        }
    }

    private static void SetMode(RunMode mode)
    {
        if (ActiveMode == mode && Setting.LastUseWrapperMode == (mode == RunMode.Wrapper))
        {
            return;
        }

        ActiveMode = mode;
        Setting.LastUseWrapperMode = mode == RunMode.Wrapper;
        Setting.SaveSettings();
    }

    private static Vector2 ToAbsoluteDesktopSpace(Vector2 overlaySpace)
    {
        return new Vector2(overlaySpace.X + _lastOverlayRect.X, overlaySpace.Y + _lastOverlayRect.Y);
    }

    private static Vector2 ToOverlaySpace(Vector2 absoluteDesktopSpace)
    {
        return new Vector2(absoluteDesktopSpace.X - _lastOverlayRect.X, absoluteDesktopSpace.Y - _lastOverlayRect.Y);
    }

    private readonly record struct ProcessItem(string WindowTitle, string ProcessName, IntPtr Handle);
    private readonly record struct Rect(int X, int Y, int Width, int Height);

    #region WinAPI
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
    #endregion
}
