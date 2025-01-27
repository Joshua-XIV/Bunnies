using Dalamud.Game.ClientState.Conditions;
using ECommons.DalamudServices;
using ECommons.Throttlers;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using System.Xml.Serialization;

namespace FirstPlugin.Scheduler.Tasks
{
    internal static class TaskTeleport
    {
        internal static unsafe void Enqueue(uint aetheryteID, uint targetTerritoryId, uint secondZoneID = 0, bool useSecondID = false)
        {
            P.taskManager.Enqueue(() => TeleporttoAetheryte(aetheryteID, targetTerritoryId, secondZoneID, useSecondID), "Teleporting to Destination");
        }

        internal static unsafe bool? TeleporttoAetheryte(uint aetheryteID, uint targetTerritoryId, uint secondZoneID = 0, bool useSecondID = false)
        {
            if (IsInZone(targetTerritoryId) && PlayerNotBusy())
                return true;
            else if (useSecondID && IsInZone(secondZoneID) && PlayerNotBusy())
                return true;

            if (!Svc.Condition[ConditionFlag.Casting] && PlayerNotBusy() && !IsBetweenAreas && !IsInZone(targetTerritoryId))
            {
                if (EzThrottler.Throttle("Teleport Throttle", 1100))
                {
                    PLogInfo($"Teleporting to {aetheryteID} at {targetTerritoryId}");
                    Telepo.Instance()->Teleport(aetheryteID, 0);
                    return false;
                }
            }
            return false;
        }
    }
}
