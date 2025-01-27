using ECommons.Automation;
using ECommons.DalamudServices;
using ECommons.Throttlers;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Component.GUI;

namespace FirstPlugin.Scheduler.Tasks
{
    internal class TaskSelfRepair
    {
        internal static void Enqueue()
        {
            TaskPluginLog.Enqueue("Repairing Self");
            P.taskManager.Enqueue(PlayerNotBusy);
            P.taskManager.Enqueue(OpenSelfRepair);
            P.taskManager.Enqueue(SelfRepair);
        }
        internal unsafe static bool SelfRepair()
        {
            if (!NeedsRepair(C.repairSlider))
            {
                return true;
            }
            else if (TryGetAddonByName<AtkUnitBase>("SelectYesno", out var addon) && IsAddonReady(addon))
            {
                if (FrameThrottler.Throttle("SelectYesnoThrottle", 300))
                {
                    Svc.Log.Debug("SelectYesno Callback");
                    Callback.Fire(addon, true, 0);
                }
            }
            else if (TryGetAddonByName<AtkUnitBase>("Repair", out var addon2) && IsAddonReady(addon2))
            {
                if (FrameThrottler.Throttle("GlobalTurnInGenericThrottle", 300))
                {
                    Svc.Log.Debug("Repair Callback");
                    Callback.Fire(addon2, true, 0);
                }
            }
            return false;
        }

        internal unsafe static bool OpenSelfRepair()
        {
            if (TryGetAddonByName<AtkUnitBase>("Repair", out var addon3) && IsAddonReady(addon3))
            {
                return true;
            }
            if (EzThrottler.Throttle("Opening Self Repair", 1000))
                ActionManager.Instance()->UseAction(ActionType.GeneralAction, 6);
            return false;
        }
    }
}
