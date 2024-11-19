using System;
using System.Numerics;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using ImGuiNET;

namespace SamplePlugin.Windows;

public class PeteWindow : Window, IDisposable
{
    private string PeteImagePath;

    public PeteWindow(Plugin plugin, string peteImagePath)
        : base("My Pete Window##Pete", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        PeteImagePath = peteImagePath;
    }

    public void Dispose() { }

    public override void Draw()
    {
        ImGui.Text($"Here is a Pete");

        ImGui.Spacing();

        var peteImage = Plugin.TextureProvider.GetFromFile(PeteImagePath).GetWrapOrDefault();
        if (peteImage != null)
        {
            ImGuiHelpers.ScaledIndent(55f);
            ImGui.Image(peteImage.ImGuiHandle, new Vector2(peteImage.Width / 12, peteImage.Height / 12));
            ImGuiHelpers.ScaledIndent(-55f);
        }
        else
        {
            ImGui.Text("Image not found.");
        }
    }
}

