using Dalamud.Interface.Colors;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using ECommons.ImGuiMethods;
using ImGuiNET;
using System.Numerics;
using Dalamud.Interface;

namespace FirstPlugin.Ui.MainWindow;

internal class MainWindow : Window
{
    public MainWindow() : base($"Bunnies {P.GetType().Assembly.GetName().Version} ###BunniesMainWindow")
    {
        SizeConstraints = new()
        {
            MinimumSize = new(150, 150),
            MaximumSize = new(9999, 9999)
        };

        TitleBarButtons.Add(new()
        {
            Click = (m) => { if (m == ImGuiMouseButton.Left) P.settingsWindow.IsOpen = !P.settingsWindow.IsOpen; },
            Icon = FontAwesomeIcon.Cog,
            IconOffset = new(2,2),
            ShowTooltip = () => ImGui.SetTooltip("Open settings window")
        });

        P.windowSystem.AddWindow(this);
    }
    public void Dispose() {}
    private void DrawStatsTab()
    {
        if (ImGui.BeginTabBar("Stats"))
        {
            if (ImGui.BeginTabItem("Lifetime"))
            {
                this.DrawStatsTab(C.stats, out bool reset);
                
                if (reset)
                {
                    C.stats = new();
                    C.Save();
                }
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("Session"))
            {
                this.DrawStatsTab(C.sessionStats, out bool reset);

                if (reset)
                {
                    C.sessionStats = new();
                }
                ImGui.EndTabItem();
            }
            ImGui.EndTabBar();
        }
    }

    private void DrawStatsTab(Stats stat, out bool reset)
    {
        DrawStats(stat);

        float windowHeight = ImGui.GetWindowHeight();
        float buttonYpos = windowHeight - 30 - ImGui.GetStyle().WindowPadding.Y;
        ImGui.SetCursorPosY(buttonYpos);

        bool isCtrlHeld = ImGui.GetIO().KeyCtrl;

        using (var _ = ImRaii.PushStyle(ImGuiStyleVar.Alpha, 0.5f, !ImGui.GetIO().KeyCtrl))
        {
            reset = ImGui.Button("RESET STATS", new Vector2(ImGui.GetContentRegionAvail().X, 30)) && ImGui.GetIO().KeyCtrl;
        }
        if (ImGui.IsItemHovered()) ImGui.SetTooltip(isCtrlHeld ? "Press to reset your stats." : "Hold Ctrl to enable the button.");
    }

    private void DrawStats(Stats stat)
    {
        if (!C.hasUpdatedStats)
        {
            C.hasUpdatedStats = true;
        }

        ImGui.Columns(3, null, false);

        // Top Middle
        ImGui.NextColumn();

        ImGuiEx.CenterColumnText(ImGuiColors.DalamudRed, "Bunnies", true);
        ImGuiHelpers.ScaledDummy(10f);

        // Setting up columns for stats
        ImGui.Columns(2,null, false);

        // Dictionary for Iteration
        var stats = new Dictionary<string, int>
        {
            { "Gil Earned", stat.gilEarned },
            { "Gold Coffers", stat.goldCoffer },
            { "Silver Coffers", stat.silverCoffer },
            { "Bronze Coffers", stat.bronzeCoffer },
            { "Eldthurs Mount", stat.eldthursCounter }
        };

        foreach (var (label, value) in stats)
        {
            ImGuiEx.CenterColumnText($"{label}", true);
            ImGuiEx.CenterColumnText($"{value:N0}");
            ImGui.NextColumn();
        }

        ImGui.Columns(1, null, false);
        ImGui.Separator();
        ImGui.Dummy(new Vector2(0, 20));
    }

    public override void Draw()
    {
        ImGuiEx.EzTabBar("Bunnies Bar",
                        ("Start Bunnies", StartBunnies.Draw, null, true),
                        ("Stats", DrawStatsTab, null, true),
                        ("About", About.Draw, null, true)
                        );
    }
}
