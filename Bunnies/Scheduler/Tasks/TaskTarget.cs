using Dalamud.Game.ClientState.Objects.Types;
using ECommons.DalamudServices;

namespace FirstPlugin.Scheduler.Tasks
{
    internal static class TaskTarget
    {
        public static void Enqueue(ulong objectID)
        {
            Svc.Log.Debug($"Targeting {objectID}");
            IGameObject? gameObject = null;
            P.taskManager.Enqueue(() => TryGetObjectByDataId(objectID, out gameObject), "Getting Object");
            TaskPluginLog.Enqueue($"Targeting By ID. Target is: {gameObject?.DataId}");
            P.taskManager.Enqueue(() => TargetByID(gameObject), "Targeting Object");
        }
    }
}
