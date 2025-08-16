using Dalamud.Interface.Colors;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using ECommons.ImGuiMethods;
using Dalamud.Bindings.ImGui;
using System.Numerics;
using Dalamud.Interface;
using FirstPlugin.Scheduler.Handlers;
using FirstPlugin.Scheduler.Tasks;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using FFXIVClientStructs.FFXIV.Component.GUI;
using System.Reflection;
using FFXIVClientStructs.FFXIV.Client.UI;
using Dalamud.Game.Config;
using ECommons.Logging;
using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using FFXIVClientStructs.FFXIV.Client.Game.Fate;
using FirstPlugin.IPC;
namespace FirstPlugin.Ui.DebugWindow;

internal class DebugWindow : Window
{
    public DebugWindow() : base($"Bunnies Debug ###BunniesDebugWindow")
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
            IconOffset = new(2, 2),
            ShowTooltip = () => ImGui.SetTooltip("Open settings window")
        });

        P.windowSystem.AddWindow(this);
    }
    public void Dispose() {}

    public override void Draw()
    {
        bool debug = C.enableDebug;
        if (ImGui.Checkbox("Debug Stats", ref debug))
        {
            C.UpdatePyrosStats(PyrosStats => { PyrosStats.gilEarned = C.stats.gilEarned; });
            C.UpdatePyrosStats(PyrosStats => { PyrosStats.goldCoffer = C.stats.goldCoffer; });
            C.UpdatePyrosStats(PyrosStats => { PyrosStats.silverCoffer = C.stats.silverCoffer; });
            C.UpdatePyrosStats(PyrosStats => { PyrosStats.bronzeCoffer = C.stats.bronzeCoffer; });
            C.UpdatePyrosStats(PyrosStats => { PyrosStats.eldthursCounter = C.stats.eldthursCounter; });
            C.UpdatePyrosStats(PyrosStats => { PyrosStats.pyrosHairStyleCounter = C.stats.pyrosHairStyleCounter; });
            C.enableDebug = debug;
        }
    }
}
