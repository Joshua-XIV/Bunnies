using Dalamud.Interface.Colors;
using Dalamud.Plugin.Ipc.Exceptions;
using ECommons.ImGuiMethods;
using ECommons.Logging;
using Dalamud.Bindings.ImGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace FirstPlugin.Ui.SettingsWindow
{
    internal class AutoReatinerSettings
    {
        private static bool EnableRetainers = C.enableRetainers;
        private static bool EnableSubs = C.enableSubs;
        private static bool EnableMulti = C.enableMulti;

        public static void Draw()
        {
            if (!PluginInstalled("AutoRetainer"))
            {
                var backgroundColor = Vector4.Zero;
                ImGui.TextColored(ImGuiColors.DalamudRed, "AutoRetainer is currently not installed or enabled. Click to copy Repo.");
                C.enableRetainers = false;
                C.enableSubs = false;
                C.enableMulti = false;
                if (ImGui.IsItemHovered())
                {
                    backgroundColor = new Vector4(0.5f, 0.5f, 0.5f, 0.15f);
                    if (ImGui.IsItemClicked())
                    {
                        ImGui.SetClipboardText(IPC.AutoRetainerIPC.Repo);
                        DuoLog.Information("Repo URL Copied");
                        Notify.Info("Repo URL Copied");
                    }
                    var textSize = ImGui.CalcTextSize("AutoRetainer is currently not installed or enabled. Click to copy Repo.");
                    var cursorPos = ImGui.GetCursorScreenPos() - new Vector2(0, textSize.Y + 2);
                    ImGui.GetWindowDrawList().AddRectFilled(cursorPos, cursorPos + (textSize) - new Vector2(0,2), backgroundColor.ToUint());
                }
                C.Save();
            }

            if (PluginInstalled("AutoRetainer"))
            {
                if (Helpers.CheckboxWithTooltip("Enable Retainer Support", ref EnableRetainers,
                "Enables retainer support for this character."))
                {
                    C.enableRetainers = EnableRetainers;
                }

                // Enables submarine support for this character.
                if (Helpers.CheckboxWithTooltip("Enable Submarine Support", ref EnableSubs,
                "Currently Not Supported!"))
                {
                    C.enableSubs = EnableSubs;
                }

                // Enables multi support after completion of looping Bunnies
                if (Helpers.CheckboxWithTooltip("Enable Multi Support", ref EnableMulti,
                "Currently Not Supported!"))
                {
                    C.enableMulti = EnableMulti;
                }
            }
        }
    }
}
