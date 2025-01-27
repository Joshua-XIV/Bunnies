using Dalamud.Game.ClientState.Objects.Types;
using ECommons.DalamudServices;

namespace FirstPlugin.Scheduler.Tasks
{
    internal static class TaskTargetName
    {
        public static void Enqueue(string name)
        {
            Svc.Log.Debug($"Targeting {name}");
            IGameObject? gameObject = null;
            P.taskManager.Enqueue(() => TryGetObjectByName(name, out gameObject), "Getting Target By Name");
            TaskPluginLog.Enqueue($"Targeting By Name. Target is: {gameObject?.Name}");
            P.taskManager.Enqueue(() => TargetByName(gameObject, name), "Targeting Object");
        }
    }
}
