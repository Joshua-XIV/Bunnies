using Dalamud.Game.ClientState.Objects.Types;
using ECommons.DalamudServices;

namespace FirstPlugin.Scheduler.Tasks
{
    internal static class TaskTargetNameDistance
    {
        public static void Enqueue(string name, uint distance)
        {
            Svc.Log.Debug($"Targeting {name}");
            IGameObject? gameObject = null;//
            P.taskManager.Enqueue(() => TryGetObjectByName(name, out gameObject), "Getting Target By Name");
            TaskPluginLog.Enqueue($"Targeting By Name. Target is: {gameObject?.Name.ToString()}");
            P.taskManager.Enqueue(() => TargetByNameDistance(gameObject, name, distance), "Targeting Object");
        }
    }
}
