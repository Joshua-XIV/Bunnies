using Dalamud.Interface.Colors;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using ECommons.ImGuiMethods;
using Dalamud.Bindings.ImGui;
using System.Numerics;
using Dalamud.Interface;
using FFXIVClientStructs.FFXIV.Component.GUI;
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
                this.DrawStatsTab(C.stats, out bool reset, C.pyrosStats, C.pagosStats, C.hydatosStats);
                
                if (reset)
                {
                    C.stats = new();
                    C.pagosStats = new();
                    C.pyrosStats = new();
                    C.hydatosStats = new();

                    C.Save();
                }
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("Session"))
            {
                this.DrawStatsTab(C.sessionStats, out bool reset, C.pyrosSessionStats, C.pagosSessionStats, C.hydatosSessionStats);

                if (reset)
                {
                    C.sessionStats = new();
                    C.pagosSessionStats = new();
                    C.pyrosSessionStats = new();
                    C.hydatosSessionStats = new();
                }
                ImGui.EndTabItem();
            }
            ImGui.EndTabBar();
        }
    }
    private bool showAllStats = true;
    private bool showSessionStats = true;
    private bool showPagosStats = false;
    private bool showSessionPagosStats = false;
    private bool showPyrosStats = false;
    private bool showSessionPyrosStats = false;
    private bool showHydatosStats = false;
    private bool showSessionHydatostats = false;
    private void DrawStatsTab(Stats stat, out bool reset, PyrosStats pyrosStat, PagosStats pagosStat, HydatosStats hydatosStat)
    {
        float windowHeight = ImGui.GetWindowHeight();
        float buttonYpos = windowHeight - 30 - ImGui.GetStyle().WindowPadding.Y;
        // Reserve space for the Reset Stats button at the bottom
        float buttonHeight = 30; // Height of the button
        float windowPadding = ImGui.GetStyle().WindowPadding.Y; // Window padding
        float availableHeight = ImGui.GetContentRegionAvail().Y - buttonHeight - windowPadding;
        var availableWidth = new Vector2(ImGui.GetContentRegionAvail().X, 0);
        string[] texts = { "Pagos Stats", "Pyros Stats", "Hydatos Stats" };
        float[] textSize = { ImGui.CalcTextSize(texts[0]).X, ImGui.CalcTextSize(texts[1]).X, ImGui.CalcTextSize(texts[2]).X };
        float[] textStartX = { (availableWidth[0] - textSize[0]) * 0.5f, (availableWidth[0] - textSize[1]) * .5f, (availableWidth[0] - textSize[2]) * .5f };
        //ImGui.SetCursorPosY(buttonYpos);
        ImGui.BeginChild("StatsRegion", new Vector2(0, availableHeight), true, ImGuiWindowFlags.None);
        DrawMainSelectables("Total Stats", ref showAllStats, availableWidth, .5f * (availableWidth[0] - ImGui.CalcTextSize("Total Stats").X));
        if (showAllStats)
            DrawStats(stat);

        ImGui.Separator();
        DrawMainSelectables(texts[0], ref showPagosStats, availableWidth, textStartX[0]);
        if (showPagosStats)
            DrawPagosStats(pagosStat);

        ImGui.Separator();
        DrawMainSelectables(texts[1], ref showPyrosStats, availableWidth, textStartX[1]);
        if (showPyrosStats)
            DrawPyrosStats(pyrosStat);

        ImGui.Separator();
        DrawMainSelectables(texts[2], ref showHydatosStats, availableWidth, textStartX[2]);
        if (showHydatosStats)
            DrawHydtosStats(hydatosStat);

        if (!showHydatosStats)
            ImGui.Separator();
        ImGui.EndChild();
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

        ImGui.Columns(3, "", false);

        // Top Middle
        ImGui.NextColumn();

        ImGuiEx.CenterColumnText(ImGuiColors.DalamudRed, "Bunnies", true);
        ImGuiHelpers.ScaledDummy(10f);

        // Setting up columns for stats
        ImGui.Columns(2, "", false);

        // Dictionary for Iteration
        var totalStats = new Dictionary<string, int>
        {
            { "Gil Earned", stat.gilEarned },
            { "Gold Coffers", stat.goldCoffer },
            { "Silver Coffers", stat.silverCoffer },
            { "Bronze Coffers", stat.bronzeCoffer },
            { "Eldthurs Mount", stat.eldthursCounter },
            { "Pyros Hairstyles", stat.pyrosHairStyleCounter},
            { "Copycat Bulb", stat.bulbMinion },
            { "Petrel Mount", stat.petrelCounter }
        };

        foreach (var (label, value) in totalStats)
        {
            ImGuiEx.CenterColumnText($"{label}", true);
            ImGuiEx.CenterColumnText($"{value:N0}");
            ImGui.NextColumn();
        }

        ImGui.Columns(1, "", false);
        ImGui.Dummy(new Vector2(0, 20));
    }

    private void DrawPagosStats(PagosStats stats)
    {
        if (!C.hasUpdatedStats)
        {
            C.hasUpdatedStats = true;
        }

        ImGui.Columns(3, "", false);

        // Top Middle
        ImGui.NextColumn();

        ImGuiEx.CenterColumnText(ImGuiColors.DalamudRed, "Pagos", true);
        ImGuiHelpers.ScaledDummy(10f);

        // Setting up columns for stats
        ImGui.Columns(2, "", false);

        var PagosStats = new Dictionary<string, int>
        {
            { "Gil Earned", stats.gilEarned },
            { "Gold Coffers", stats.goldCoffer },
            { "Silver Coffers", stats.silverCoffer },
            { "Bronze Coffers", stats.bronzeCoffer },
            { "Eldthurs Mount", stats.bulbMinion },
            { "Pyros Hairstyles", stats.hakutakuEye}
        };

        foreach (var (label, value) in PagosStats)
        {
            ImGuiEx.CenterColumnText($"{label}", true);
            ImGuiEx.CenterColumnText($"{value:N0}");
            ImGui.NextColumn();
        }

        ImGui.Columns(1, "", false);
    }

    private void DrawPyrosStats(PyrosStats stats)
    {
        if (!C.hasUpdatedStats)
        {
            C.hasUpdatedStats = true;
        }

        ImGui.Columns(3, "", false);

        // Top Middle
        ImGui.NextColumn();

        ImGuiEx.CenterColumnText(ImGuiColors.DalamudRed, "Pyros", true);
        ImGuiHelpers.ScaledDummy(10f);

        // Setting up columns for stats
        ImGui.Columns(2, "", false);

        var PyrosStats = new Dictionary<string, int>
        {
            { "Gil Earned", stats.gilEarned },
            { "Gold Coffers", stats.goldCoffer },
            { "Silver Coffers", stats.silverCoffer },
            { "Bronze Coffers", stats.bronzeCoffer },
            { "Eldthurs Mount", stats.eldthursCounter },
            { "Pyros Hairstyles", stats.pyrosHairStyleCounter}
        };

        foreach (var (label, value) in PyrosStats)
        {
            ImGuiEx.CenterColumnText($"{label}", true);
            ImGuiEx.CenterColumnText($"{value:N0}");
            ImGui.NextColumn();
        }

        ImGui.Columns(1, "", false);
    }

    private void DrawHydtosStats(HydatosStats stats)
    {
        if (!C.hasUpdatedStats)
        {
            C.hasUpdatedStats = true;
        }

        ImGui.Columns(3, "", false);

        // Top Middle
        ImGui.NextColumn();

        ImGuiEx.CenterColumnText(ImGuiColors.DalamudRed, "Pagos", true);
        ImGuiHelpers.ScaledDummy(10f);

        // Setting up columns for stats
        ImGui.Columns(2, "", false);

        var HydatosStats = new Dictionary<string, int>
        {
            { "Gil Earned", stats.gilEarned },
            { "Gold Coffers", stats.goldCoffer },
            { "Silver Coffers", stats.silverCoffer },
            { "Bronze Coffers", stats.bronzeCoffer },
            { "Petrel Mount", stats.petrelCounter },
        };

        foreach (var (label, value) in HydatosStats)
        {
            ImGuiEx.CenterColumnText($"{label}", true);
            ImGuiEx.CenterColumnText($"{value:N0}");
            ImGui.NextColumn();
        }

        ImGui.Columns(1, "", false);
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
