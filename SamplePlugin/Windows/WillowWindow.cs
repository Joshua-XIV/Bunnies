using System;
using System.Numerics;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using ImGuiNET;

namespace SamplePlugin.Windows;

public class WillowWindow : Window, IDisposable
{
    private string WillowImagePath;

    public WillowWindow(Plugin plugin, string willowImagePath)
        : base("My Willow Window##Willow", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        WillowImagePath = willowImagePath;
    }

    public void Dispose() { }

    public override void Draw()
    {
        ImGui.Text($"Here is a Willow");

        ImGui.Spacing();

        var willowImage = Plugin.TextureProvider.GetFromFile(WillowImagePath).GetWrapOrDefault();
        if (willowImage != null)
        {
            ImGuiHelpers.ScaledIndent(55f);
            ImGui.Image(willowImage.ImGuiHandle, new Vector2(willowImage.Width / 12, willowImage.Height / 12));
            ImGuiHelpers.ScaledIndent(-55f);
        }
        else
        {
            ImGui.Text("Image not found.");
        }
    }
}

