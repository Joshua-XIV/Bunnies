using Dalamud.Interface.Colors;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using ECommons.ImGuiMethods;
using ImGuiNET;
using System.Numerics;
using Dalamud.Interface;
using FirstPlugin.Ui.SettingsWindow;

namespace FirstPlugin.Ui.SettingWindow;

internal class SettingsWindow : Window
{
    public SettingsWindow(): base ("Bunnies Settings ###BunniesSettingsWindow")
    {
        SizeConstraints = new()
        {
            MinimumSize = new(150, 150),
            MaximumSize = new(9999, 9999)
        };
        P.windowSystem.AddWindow(this);
    }

    public void Dispose() {}

    public override void Draw()
    {
        ImGuiEx.EzTabBar("Bunnies Settings Tabs",
                        ("General Settings", GeneralSettings.Draw,null, true),
                        ("AutoRetainer Settings", AutoReatinerSettings.Draw, null, true)
                        );
    }
}
