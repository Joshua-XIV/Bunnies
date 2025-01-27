using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Component.GUI;
using ECommons.Automation;
using ECommons.Throttlers;

namespace FirstPlugin.Scheduler.Tasks
{
    internal static class TaskRepairNpc
    {
        internal static void Enqueue(string name)
        {
            TaskPluginLog.Enqueue("Task to repair at NPC");
            TaskTargetName.Enqueue(name);
            TaskInteractName.Enqueue(name);
            P.taskManager.Enqueue(RepairNpc);
            TaskGetOut.Enqueue();
        }
        internal unsafe static bool RepairNpc()
        {
            if (!NeedsRepair(C.repairSlider))
            {
                return true;
            }
            else if (TryGetAddonByName<AtkUnitBase>("SelectYesno", out var addon) && IsAddonReady(addon))
            {
                Svc.Log.Debug("SelectYesno Callback");
                if (FrameThrottler.Throttle("SelectYesnoThrottle", 70))
                    Callback.Fire(addon, true, 0);
            }
            else if (TryGetAddonByName<AtkUnitBase>("Repair", out var addon4) && IsAddonReady(addon4))
            {
                Svc.Log.Debug("Repair Callback");
                if (FrameThrottler.Throttle("GlobalTurnInGenericThrottle", 70))
                    Callback.Fire(addon4, true, 0);
            }
            return false;
        }
    }
}
