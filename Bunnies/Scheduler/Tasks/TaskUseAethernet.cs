using ECommons.DalamudServices;
using ECommons.Throttlers;
using FirstPlugin.IPC.Lifestream;
using FirstPlugin.Scheduler.Handlers;
using System.Numerics;

namespace FirstPlugin.Scheduler.Tasks
{
    internal static class TaskUseAethernet
    {
        internal static void Enqueue(string name)
        {
            P.taskManager.Enqueue(PlayerNotBusy);
            var closest = AethernetData.Distances.OrderBy(x => x.distance).First();
            TaskMoveTo.Enqueue(closest.position, closest.description, closest.tolerance);
            P.taskManager.Enqueue(() => P.lifestream.AethernetTeleport(name));
            P.taskManager.Enqueue(() => P.lifestream.IsBusy());
            /*
            P.taskManager.Enqueue(() => GetDistanceToPoint(KuganeEurekaNpc.X, KuganeEurekaNpc.Y, KuganeEurekaNpc.Z) < 25);
            EzThrottler.Throttle("Lifestream Wait", 200);
            */
            P.taskManager.Enqueue(() => !P.lifestream.IsBusy());
            P.taskManager.Enqueue(PlayerNotBusy);
        }
    }
}
