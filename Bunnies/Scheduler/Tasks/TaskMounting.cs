using Dalamud.Game.ClientState.Conditions;
using ECommons.DalamudServices;
using ECommons.Logging;
using FFXIVClientStructs.FFXIV.Client.Game;

namespace FirstPlugin.Scheduler.Tasks
{
    internal static class TaskMounting
    {
        public static void Enqueue()
        {
            TaskPluginLog.Enqueue("Mounting up!");
            P.taskManager.Enqueue(() => UpdateCurrentTask("Mounting Up"));
            P.taskManager.Enqueue(MountUp);
            P.taskManager.Enqueue(() => UpdateCurrentTask("idle"));
        }

        internal unsafe static bool? MountUp()
        {
            if (Svc.Condition[ConditionFlag.Mounted] && PlayerNotBusy()) return true;

            if (CurrentZoneID() == Pagos ||  CurrentZoneID() == Pyros || CurrentZoneID() == Hydatos)
            {
                if (!Svc.Condition[ConditionFlag.Casting] && !Svc.Condition[ConditionFlag.Unknown57] && PlayerNotBusy())
                {
                    ActionManager.Instance()->UseAction(ActionType.GeneralAction, 24);
                }
            }
            return false;
        }
    }
}
