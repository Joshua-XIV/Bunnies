using Dalamud.Interface.Components;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using Dalamud.Bindings.ImGui;
using ECommons.ImGuiMethods;
using System.Numerics;
using static FFXIVClientStructs.FFXIV.Client.UI.RaptureAtkHistory.Delegates;
using FirstPlugin.Scheduler.Tasks;
using FirstPlugin.Scheduler;
using Dalamud.Interface.Colors;
using ECommons.Logging;
using static System.Net.WebRequestMethods;

namespace FirstPlugin.Ui.MainWindow
{
    internal class StartBunnies
    {
        public static bool IsRunning = false;
        private static string ButtonName = "Normal Raid";
        public static void Draw()
        {
            // Goes to Pagos (disabled for now)
            ImGui.BeginDisabled(true);
            if (ImGui.RadioButton("Pagos", C.zoneSelected == 0))
            {
                C.zoneSelected = 0;
                C.Save();
            }
            ImGui.EndDisabled();
            ImGuiComponents.HelpMarker("Disabled for now");

            // Goes to Pyros (WIP)
            if (ImGui.RadioButton("Pyros", C.zoneSelected == 1))
            {
                C.zoneSelected = 1;
                C.Save();
            }
            ImGuiComponents.HelpMarker("WIP");

            // Goes to Hydatos (disabled for now)
            ImGui.BeginDisabled(true);
            if (ImGui.RadioButton("Hydatos", C.zoneSelected == 2))
            {
                C.zoneSelected = 2;
                C.Save();
            }
            ImGui.EndDisabled();
            ImGuiComponents.HelpMarker("Disabled for now");

            ImGui.Text($"Task: {icurrentTask}");
            TimeSpan currentTime = P.stopwatch.Elapsed;
            string currentTimeF = currentTime.ToString(@"mm\:ss\.fff");
            ImGui.Text($"Time Elapsed is: {currentTimeF}");

            if (C.zoneSelected == 0)
                ButtonName = "Pagos";
            else if (C.zoneSelected == 1)
                ButtonName = "Pyros";
            else if (C.zoneSelected == 2)
                ButtonName = "Hydatos";

            if (!PluginInstalled("vnavmesh") || !PluginInstalled("RotationSolver") || !PluginInstalled("BossModReborn"))
            {
                ImGui.Text("The following plugins are required to start Bunnies automation.");
                PluginGreenRedText(PluginInstalled("vnavmesh"), "vnavmesh");
                ImGui.SameLine();
                if (ImGui.Button("Get Repo Url ##Vnav"))
                {
                    ImGui.SetClipboardText(IPC.NavmeshIPC.Repo);
                    DuoLog.Information("Repo URL Copied");
                    Notify.Info("Repo URL Copied");
                }
                PluginGreenRedText(PluginInstalled("RotationSolver"), "Rotation Solver Reborn");
                ImGui.SameLine();
                if (ImGui.Button("Get Repo Url ##RSR"))
                {
                    ImGui.SetClipboardText(RSR);
                    DuoLog.Information("Repo URL Copied");
                    Notify.Info("Repo URL Copied");
                }
                PluginGreenRedText(PluginInstalled("BossModReborn"), "BossMod Reborn");
                ImGui.SameLine();
                if (ImGui.Button("Get Repo Url ##BMR"))
                {
                    ImGui.SetClipboardText(BMR);
                    DuoLog.Information("Repo URL Copied");
                    Notify.Info("Repo URL Copied");
                }
            }
            else
            {
                if (ImGui.Button(!IsRunning ? $"Start {ButtonName}" : "Stop"))
                {
                    if (!IsRunning)
                    {
                        ToggleRotationAIOff();
                        SchedulerMain.EnablePlugin();
                    }
                    else
                    {
                        SchedulerMain.DisablePlugin();
                        RunCommand("e [Bunnies] Bunnies Stopped.");
                    }
                }
            }
        }
    }
}
