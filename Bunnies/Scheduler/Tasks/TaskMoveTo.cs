using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using System.Numerics;

namespace FirstPlugin.Scheduler.Tasks
{
    internal static class TaskMoveTo
    {
        private static float Distance(this Vector3 v, Vector3 v2)
        {
            return new Vector2(v.X - v2.X, v.Z - v2.Z).Length();
        }

        private static unsafe bool IsMoving()
        {
            return AgentMap.Instance()->IsPlayerMoving == 1;
        }

        internal unsafe static void Enqueue(Vector3 targetPosition, string destination, float toleranceDistance = 3f)
        {
            TaskPluginLog.Enqueue($"Moving to {destination}");
            P.taskManager.Enqueue(() => UpdateCurrentTask($"Moving to {destination}"), "Task Update");
            P.taskManager.Enqueue(() => MoveTo(targetPosition, toleranceDistance), $"{destination}", configuration: DConfig);
            P.taskManager.Enqueue(() => UpdateCurrentTask("idle"), "Task Update");
        }

        internal unsafe static bool? MoveTo(Vector3 targetPosition, float toleranceDistance = 3f)
        {
            if (targetPosition.Distance(Player.GameObject->Position) <= toleranceDistance)
            {
                P.navmesh.Stop();
                return true;
            }
            
            if (!P.navmesh.IsReady()) { UpdateCurrentTask("Waiting Navmesh"); return false; }

            if (IsMoving() && targetPosition.Distance(Player.GameObject->Position) >= 6)
            {
                if (ActionManager.Instance()->GetActionStatus(ActionType.GeneralAction, 4) == 0 && ActionManager.Instance()->QueuedActionId != 4 && !Player.Character->IsCasting)
                    ActionManager.Instance()->UseAction(ActionType.GeneralAction, 4);
            }

            if (P.navmesh.PathfindInProgress() || P.navmesh.IsRunning() || IsMoving()) return false;

            P.navmesh.PathfindAndMoveTo(targetPosition, false);
            P.navmesh.SetAlignCamera(true);
            return false;
        }
    }
}
