using Dalamud.Game.ClientState.Objects.Types;

namespace FirstPlugin.Scheduler.Tasks
{
    internal static class TaskInteractName
    {
        public static void Enqueue(string name)
        {
            IGameObject? gameObject = null;
            P.taskManager.Enqueue(PlayerNotBusy);
            P.taskManager.Enqueue(() => TryGetObjectByName(name, out gameObject));
            TaskPluginLog.Enqueue($"Interacting w/ {gameObject?.Name}");
            P.taskManager.Enqueue(() => InteractWithObject(gameObject));
        }
    }
}
