using System;
using System.Numerics;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using ImGuiNET;

namespace SamplePlugin.Windows;

public class PidgeWindow : Window, IDisposable
{
    private string PidgeImagePath;

    public PidgeWindow(Plugin plugin, string pidgeImagePath)
        : base("My Pidge Window##Pidge", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        PidgeImagePath = pidgeImagePath;
    }

    public void Dispose() { }

    public override void Draw()
    {
        ImGui.Text($"Here is a Pidge");

        ImGui.Spacing();

        var pidgeImage = Plugin.TextureProvider.GetFromFile(PidgeImagePath).GetWrapOrDefault();
        if (pidgeImage != null)
        {
            ImGuiHelpers.ScaledIndent(55f);
            ImGui.Image(pidgeImage.ImGuiHandle, new Vector2(pidgeImage.Width / 12, pidgeImage.Height / 12));
            ImGuiHelpers.ScaledIndent(-55f);
        }
        else
        {
            ImGui.Text("Image not found.");
        }
    }
}

