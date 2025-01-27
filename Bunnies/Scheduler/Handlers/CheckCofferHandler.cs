using Dalamud.Game.ClientState.Objects.Types;
using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using FirstPlugin.Scheduler.Tasks;
using FirstPlugin.Scheduler.Handlers;
using System.Data;
using ECommons.Throttlers;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using ECommons.Logging;

namespace FirstPlugin.Scheduler.Handlers
{
    internal class CheckCofferHandler
    {
        public static class CofferState
        {
            public static bool FoundCoffer = false;
        }
        public static void CheckCoffer()
        {//
            IGameObject gameObject;
            if (TryGetClosestCoffer(out gameObject) && gameObject != null)
            {
                CofferState.FoundCoffer = true;
                P.taskManager.Enqueue(() => UpdateCurrentTask("Grabbing Coffer"));
                P.taskManager.Enqueue(() => CofferState.FoundCoffer = true);
                P.taskManager.Enqueue(() => GilCheck.CheckEverything());
                P.taskManager.Enqueue(() => RunCommand("e Targeting Coffer"));
                TaskTargetNameDistance.Enqueue(gameObject.Name.ToString(), 5);
                TaskInteractName.Enqueue(gameObject.Name.ToString());
                P.taskManager.EnqueueDelay(100);
                P.taskManager.Enqueue(() => !PlayerNotBusy());
                P.taskManager.Enqueue(() => PlayerNotBusy());
                P.taskManager.Enqueue(() => GilCheck.CheckEverything());
            }
            /*
            P.taskManager.Enqueue(() =>
            {
                if (!CofferState.FoundCoffer)
                {
                    P.taskManager.Enqueue(() => RunCommand("e No Coffer Found"));
                }
                else
                {
                    P.taskManager.Enqueue(() => RunCommand("e Coffer Found"));
                }
            });
            P.taskManager.Enqueue(() => PluginLog.Information($"The bool is ${CofferState.FoundCoffer}"));
            */
        }
    }
}
