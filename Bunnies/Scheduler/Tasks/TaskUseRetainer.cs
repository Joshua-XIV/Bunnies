using Dalamud.Game.ClientState.Objects.Types;
using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Component.GUI;
using System.Numerics;

namespace FirstPlugin.Scheduler.Tasks
{
    internal static class TaskUseRetainer
    {
        internal static void Enqueue()
        {
            IGameObject? gameObject = null;
            P.taskManager.Enqueue(() => UpdateCurrentTask("Using Retainer"));
            P.taskManager.Enqueue(() =>TryGetObjectByName("Summoning Bell", out gameObject));
            TaskPluginLog.Enqueue("Using Auto Retainer Task");
            P.taskManager.Enqueue(PlayerNotBusy);
            TaskTargetName.Enqueue("Summoning Bell");
            TaskInteractName.Enqueue("Summoning Bell");
            P.taskManager.Enqueue(RetainerOpened);
            P.taskManager.Enqueue(() => !ARRetainersWaitingToBeProcessed());
            P.taskManager.Enqueue(() => P.autoRetainer.IsBusy());
            P.taskManager.Enqueue(() => !P.autoRetainer.IsBusy());
            TaskGetOut.Enqueue();
        }
        internal unsafe static bool RetainerOpened()
        {
            if (TryGetAddonByName<AtkUnitBase>("RetainerList", out var addon) && IsAddonReady(addon))
                return true;
            return false;
        }
    }
}
