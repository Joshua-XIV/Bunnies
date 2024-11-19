using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using SamplePlugin.Windows;
using Microsoft.VisualBasic;

namespace SamplePlugin;

public sealed class Plugin : IDalamudPlugin
{
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] internal static ITextureProvider TextureProvider { get; private set; } = null!;
    [PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;

    private const string CommandMainName = "/josh";
    private const string CommandSettingName = "/joshconfig";
    private const string PidgeCommandName = "/pidge";
    private const string PeteCommandName = "/pete";
    private const string WillowCommandName = "/willow";


    public Configuration Configuration { get; init; }

    public readonly WindowSystem WindowSystem = new("Josh's Plugin");
    private ConfigWindow ConfigWindow { get; init; }
    private MainWindow MainWindow { get; init; }
    private PidgeWindow PidgeWindow { get; init; }
    private PeteWindow PeteWindow { get; init; }
    private WillowWindow WillowWindow { get; init; }

    public Plugin()
    {
        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();

        // you might normally want to embed resources and load them from the manifest stream
        var goatImagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "goat.png");
        var pidgeImagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "pidge.png");
        var peteImagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "pete.jpg");
        var willowImagePath = Path.Combine(PluginInterface.AssemblyLocation?.Directory?.FullName!, "willow.jpg");

        ConfigWindow = new ConfigWindow(this);
        MainWindow = new MainWindow(this, goatImagePath);
        PidgeWindow = new PidgeWindow(this, pidgeImagePath);
        PeteWindow = new PeteWindow(this, peteImagePath);
        WillowWindow = new WillowWindow(this, willowImagePath);

        WindowSystem.AddWindow(ConfigWindow);
        WindowSystem.AddWindow(MainWindow);
        WindowSystem.AddWindow(PidgeWindow);
        WindowSystem.AddWindow(PeteWindow);
        WindowSystem.AddWindow(WillowWindow);

        CommandManager.AddHandler(CommandMainName, new CommandInfo(OnCommand)
        {
            HelpMessage = "This is my command, nobody use it!"
        });

        CommandManager.AddHandler(CommandSettingName, new CommandInfo(SettingCommand) 
        { 
            HelpMessage = "This is my setting command!" 
        });

        CommandManager.AddHandler(PidgeCommandName, new CommandInfo(PidgeCommand)
        {
            HelpMessage = "This is my pidge command!"
        });

        CommandManager.AddHandler(PeteCommandName, new CommandInfo(PeteCommand)
        {
            HelpMessage = "This is my pete command!"
        });

        CommandManager.AddHandler(WillowCommandName, new CommandInfo(WillowCommand)
        {
            HelpMessage = "This is my willow command!"
        });

        PluginInterface.UiBuilder.Draw += DrawUI;

        // This adds a button to the plugin installer entry of this plugin which allows
        // to toggle the display status of the configuration ui
        PluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUI;

        // Adds another button that is doing the same but for the main ui of the plugin
        PluginInterface.UiBuilder.OpenMainUi += ToggleMainUI;
    }

    public void Dispose()
    {
        WindowSystem.RemoveAllWindows();

        ConfigWindow.Dispose();
        MainWindow.Dispose();
        PidgeWindow.Dispose();
        PeteWindow.Dispose();   
        WillowWindow.Dispose(); 

        CommandManager.RemoveHandler(CommandMainName);
        CommandManager.RemoveHandler(CommandSettingName);
        CommandManager.RemoveHandler(PidgeCommandName);
        CommandManager.RemoveHandler(PeteCommandName);
        CommandManager.RemoveHandler(WillowCommandName);

    }

    private void OnCommand(string command, string args)
    {
        // in response to the slash command, just toggle the display status of our main ui
        ToggleMainUI();
    }

    private void SettingCommand(string command, string args)
    {
        ToggleConfigUI();
    }

    private void PidgeCommand(string command, string args)
    {
        TogglePidgeUI();
    }

    private void PeteCommand(string command, string args)
    {
        TogglePeteUI();
    }

    private void WillowCommand(string command, string args)
    {
        ToggleWillowUI();
    }
    private void DrawUI() => WindowSystem.Draw();

    public void ToggleConfigUI() => ConfigWindow.Toggle();
    public void ToggleMainUI() => MainWindow.Toggle();
    public void TogglePidgeUI() => PidgeWindow.Toggle();
    public void TogglePeteUI() => PeteWindow.Toggle();
    public void ToggleWillowUI() => WillowWindow.Toggle();
}
