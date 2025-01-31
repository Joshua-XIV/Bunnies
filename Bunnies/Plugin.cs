using Dalamud.Game.Gui.Toast;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using ECommons;
using ECommons.Automation.NeoTaskManager;
using ECommons.Configuration;
using ECommons.DalamudServices;
using FirstPlugin.IPC;
using FirstPlugin.Ui.MainWindow;
using FirstPlugin.Ui.DebugWindow;
using FirstPlugin.Ui.SettingWindow;
using AutoRetainerAPI;
using System.Diagnostics;
using FirstPlugin.IPC.Lifestream;
using FirstPlugin.Scheduler;
using Dalamud.IoC;
using System.IO;
using Dalamud.Interface.Textures.TextureWraps;
using Dalamud.Interface.Internal;
using ECommons.ImGuiMethods;

namespace FirstPlugin;

public class Plugin : IDalamudPlugin
{
    public string Name => "Bunnies";
    internal static Plugin P = null!;
    private Config config;

    internal IChatGui ChatGui { get; private init; } = null!;

    internal IToastGui ToastGui { get; private init; } = null!;

    public static Config C => P.config;

    // internal window names
    internal WindowSystem windowSystem;
    internal MainWindow mainWindow;
    internal SettingsWindow settingsWindow;
    internal DebugWindow debugWindow;
    internal string clipboardPath;
    // toast?
    public Filter filter { get; }
    // IPC's/Internals
    internal AutoRetainerApi autoRetainerApi;
    internal LifestreamIPC lifestream;
    internal TaskManager taskManager;
    internal AutoRetainerIPC autoRetainer;
    internal PandoraIPC pandora;
    internal NavmeshIPC navmesh;
    internal BossModIPC bossmod;
    internal WrathIPC wrath;
    internal BunniesIPC bunniesIPC;

    // Timers
    internal Stopwatch stopwatch;
    internal TimeSpan totalRunTime;

    #pragma warning disable CS8618
    public Plugin(IDalamudPluginInterface pluginInterface, IChatGui chatGui, IToastGui toastGui)
    {
        P = this;
        ChatGui = chatGui;
        ToastGui = toastGui;
        this.filter = new Filter();
        ECommonsMain.Init(pluginInterface, P, ECommons.Module.DalamudReflector, ECommons.Module.ObjectFunctions);
        new ECommons.Schedulers.TickScheduler(Load);
    }
    //
    public void Load()
    {
        EzConfig.Migrate<Config>();
        config = EzConfig.Init<Config>();

        // Toast

        // IPC's
        taskManager = new();
        autoRetainer = new();
        autoRetainerApi = new();
        lifestream = new();
        navmesh = new();
        pandora = new();
        bossmod = new();
        wrath = new();
        bunniesIPC = new();

        // Windows
        windowSystem = new();
        mainWindow = new();
        debugWindow = new();
        settingsWindow = new();

        // Timers
        stopwatch = new();

        Svc.PluginInterface.UiBuilder.Draw += windowSystem.Draw;

        Svc.PluginInterface.UiBuilder.OpenMainUi += () =>
        {
            mainWindow.IsOpen = true;
        };

        Svc.PluginInterface.UiBuilder.OpenConfigUi += () =>
        {
            settingsWindow.IsOpen = !settingsWindow.IsOpen;
        };

        Svc.PluginInterface.UiBuilder.OpenConfigUi += () =>
        {
            debugWindow.IsOpen = !debugWindow.IsOpen;
        };

        EzCmd.Add("/bunnies", OnCommand, 
                """ 
                - Opens plugin interace
                /bunnies s|settings - Opens Settings 
                /bunnies pyros - Starts Pyros Bunnnies
                /bunnies pagos - Starts Pagos Bunnnies
                /bunnies hydatos - Starts Hydatos Bunnies
                /bunnies stop - Stops Bunnies
                """);

        Svc.Framework.Update += Tick;
        C.sessionStats.Reset();
        C.pagosSessionStats.Reset();
        C.pyrosSessionStats.Reset();
        C.hydatosSessionStats.Reset();
    }
            
    private void Tick(object _)
    {
        if (SchedulerMain.DoWeTick && Svc.ClientState.LocalPlayer != null)
        {
            SchedulerMain.Tick();
        }
    }
    public void Dispose()
    {
        this.filter.Dispose();
        ECommonsMain.Dispose();
    }

    private void OnCommand(string command, string args)
    {
        if (args.EqualsIgnoreCaseAny("d", "debug"))
        {
            debugWindow.IsOpen = !debugWindow.IsOpen;
        }

        else if (args.EqualsIgnoreCaseAny("s", "settings", "setting"))
        {
            settingsWindow.IsOpen = !settingsWindow.IsOpen;
        }

        else if (args.EqualsIgnoreCaseAny("stop"))
        {
            SchedulerMain.DisablePlugin();
            RunCommand("e [Bunnies] Bunnies Stopped.");
        }

        else if (args.EqualsIgnoreCaseAny("pagos"))
        {
            //C.zoneSelected = 0;
            //SchedulerMain.EnablePlugin();
        }

        else if (args.EqualsIgnoreCaseAny("pyros"))
        {
            if (PluginInstalled("vnavmesh") && PluginInstalled("RotationSolver") && PluginInstalled("BossModReborn"))
            {
                C.zoneSelected = 1;
                SchedulerMain.EnablePlugin();
            }
            else
            {
                NotifyPlugins();
                SchedulerMain.DisablePlugin();
            }
        }

        else if (args.EqualsIgnoreCaseAny("hydatps"))
        {
            //C.zoneSelected = 2;
            //SchedulerMain.EnablePlugin();
        }

        else
        {
            mainWindow.IsOpen = !mainWindow.IsOpen;
        }
    }
}
