using Dalamud.Game.ClientState.Conditions;
using ECommons.DalamudServices;
using ECommons.Logging;
using FFXIVClientStructs.FFXIV.Client.Game;

namespace FirstPlugin.Scheduler.Tasks
{
    internal static class TaskDismount
    {
        public static void Enqueue()
        {
            TaskPluginLog.Enqueue("Dismounting!");
            P.taskManager.Enqueue(() => UpdateCurrentTask("Dismounting"));
            P.taskManager.Enqueue(Dismount);
            P.taskManager.Enqueue(() => UpdateCurrentTask("idle"));
        }

        internal unsafe static bool? Dismount()
        {
            if (!Svc.Condition[ConditionFlag.Mounted] && PlayerNotBusy()) return true;

            if (CurrentZoneID() == Pagos || CurrentZoneID() == Pyros || CurrentZoneID() == Hydatos)
            {
                if (Svc.Condition[ConditionFlag.Mounted] && PlayerNotBusy())
                {
                    ActionManager.Instance()->UseAction(ActionType.GeneralAction, 23);
                }
            }
            return false;
        }
    }
}
