using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using System.Numerics;


namespace FirstPlugin.Scheduler.Tasks
{
    internal static class TaskMoveToKuganeNpc
    {
        internal unsafe static void Enqueue()
        {
            TaskPluginLog.Enqueue($"Moving to Kugane Npc");
            P.taskManager.Enqueue(() => UpdateCurrentTask($"Moving to Kugane Npc"), "Task Update");
            P.taskManager.Enqueue(() => MoveToKuganeNpc(), $"Kugane Npc");
            P.taskManager.Enqueue(() => UpdateCurrentTask("idle"), "Task Update");
        }
        internal unsafe static bool? MoveToKuganeNpc()
        {
            if (CurrentZoneID() == 628)
                return TaskMoveTo.MoveTo(KuganeEurekaNpc, 5);
            else
                return false;
        }
    }
}
