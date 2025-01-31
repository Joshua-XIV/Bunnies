using Dalamud.Game.ClientState.Conditions;
using ECommons.Automation;
using ECommons.DalamudServices;
using ECommons.GameFunctions;
using ECommons.Logging;
using ECommons.Throttlers;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Component.GUI;
using FirstPlugin.Scheduler.Tasks;
using Lumina.Excel.Sheets;
using System.Numerics;

namespace FirstPlugin.Scheduler.Handlers
{
    internal class EurekaMovementHandler
    {
        internal static void GoToCheckPoint(Vector3 checkpoint, string direction)
        {
            TaskMoveTo.Enqueue(checkpoint, $"{direction} Checkpoint", 1f);
        }

        internal static void GoToCofferLocation(Vector3 coffer, string direction)
        {
            TaskMoveTo.Enqueue(coffer, direction, 3f);
            TaskUseCarrot.Enqueue();
            P.taskManager.EnqueueDelay(2000);
            P.taskManager.Enqueue(() => RunCommand("e Looking for Coffer"));
            TaskPluginLog.Enqueue("Looking for Coffer");
            P.taskManager.Enqueue(() => CheckCofferHandler.CheckCoffer());
        }
        //
        internal static void MoveThroughLocations(List<Vector3> cofferList, string direction, int index = 0, int size = 0)
        {
            P.taskManager.Enqueue(() => PluginLog.Information("Start of MoveThroughLocationsFunction"));
            size = cofferList.Count;
            if (CheckCofferHandler.CofferState.FoundCoffer)
            {
                P.taskManager.Enqueue(() => RunCommand("e Found!"));
                return;
            }
            string message = $"e [Bunnies] Moving to location {index + 1}.";
            P.taskManager.Enqueue(() => RunCommand(message));
            GoToCofferLocation(cofferList[index], direction);
            TaskPluginLog.Enqueue("Before Index");
            P.taskManager.Enqueue(() => index++);
            TaskPluginLog.Enqueue("After Index");
            P.taskManager.Enqueue(() =>
            {
                if (CheckCofferHandler.CofferState.FoundCoffer)
                {
                    P.taskManager.Enqueue(() => PluginLog.Information("Coffer Found"));
                    return;
                }
                else if (index >= size)
                {
                    P.taskManager.Enqueue(() => PluginLog.Information("Size Exceeded"));
                    return;
                }
                else if (index < size && !CheckCofferHandler.CofferState.FoundCoffer)
                    P.taskManager.Enqueue(() => MoveThroughLocations(cofferList, direction, index, size));
                else
                    return;
            });
        }
        internal static void JumpLocation(string direction)
        {
            Vector3 firstjumpspot = new Vector3(-96.78077f, 759.02124f, 681.8835f);
            Vector3 secondjumpspot = new Vector3(-100.01212f, 760.8888f, 685.0386f);
            Vector3 finalLocation = SouthWest.FarFarSouthWestCP1.FarFarSouthWestCP1Coffers[0];
            string message = $"e [Bunnies] Moving to locaation {1}.";
            P.taskManager.Enqueue(() => RunCommand(message));
            TaskMoveTo.Enqueue(firstjumpspot, "first jump spot", 1f);
            TaskMoveToAndJump.Enqueue(secondjumpspot, "second jump spot", 0.5f);
            GoToCofferLocation(finalLocation, direction);
            CheckCofferHandler.CheckCoffer();
        }
        internal static void MoveToCliff()
        {
            TaskMounting.Enqueue();
            TaskMoveTo.Enqueue(cliff, "cliff", 0.5f);
            TaskMoveTo.Enqueue(cliff2, "cliff2", 0.5f);
            P.taskManager.Enqueue(() => RunCommand("vnav movedir 0 0 -5"));
            P.taskManager.Enqueue(() => PluginLog.Information("Waiting for Jump"));
            P.taskManager.Enqueue(() => Svc.Condition[ConditionFlag.Jumping]);
            P.taskManager.Enqueue(() => PluginLog.Information("Falling"));
            P.taskManager.Enqueue(() => !Svc.Condition[ConditionFlag.Jumping]);
            P.taskManager.Enqueue(() => PluginLog.Information("On Ground"));
        }
        internal class CheckDirection
        {
            internal static string direction = "";
            internal static bool CheckDirections(Dictionary<string, string> dict)
            {
                direction = "";
                foreach (KeyValuePair<string, string> pair in dict)
                {
                    if (MatchTextToast(pair.Value))
                    {
                        P.taskManager.Enqueue(() => PluginLog.Information($"{pair.Value}"));
                        direction = pair.Value;
                        return true;
                    }
                }
                return false;
            }
        }
    }
}
