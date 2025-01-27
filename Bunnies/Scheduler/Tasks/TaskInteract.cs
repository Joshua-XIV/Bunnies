using Dalamud.Game.ClientState.Objects.Types;

namespace FirstPlugin.Scheduler.Tasks
{
    internal static class TaskInteract
    {
        public static void Enqueue(ulong dataID)
        {
            IGameObject? gameObject = null;
            P.taskManager.Enqueue(PlayerNotBusy);
            P.taskManager.Enqueue(() => TryGetObjectByDataId(dataID, out gameObject));
            TaskPluginLog.Enqueue($"Interacting w/ {gameObject?.Name}");
            P.taskManager.Enqueue(() => InteractWithObject(gameObject));
        }
    }
}
