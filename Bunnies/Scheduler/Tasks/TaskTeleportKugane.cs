using Dalamud.Game.ClientState.Conditions;
using ECommons.DalamudServices;
using ECommons.Throttlers;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game.MJI;


namespace FirstPlugin.Scheduler.Tasks
{
    internal static class TaskTeleportKugane
    {
        internal static unsafe void Enqueue()
        {
            TaskPluginLog.Enqueue("Teleporting to Kugane");
            P.taskManager.Enqueue(TeleportKugane, "Teleporting to Kugane");
        }

        internal static unsafe bool? TeleportKugane()
        {
            if (CurrentZoneID() == Pagos || CurrentZoneID() == Pyros || CurrentZoneID() == Hydatos)
                return false;
            else
                return TaskTeleport.TeleporttoAetheryte(KuganeAether, Kugane);
        }
    }
}
