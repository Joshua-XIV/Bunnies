using Dalamud.Game.ClientState.Objects.Types;
using ECommons.DalamudServices;
using ECommons.DalamudServices.Legacy;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using FirstPlugin.Scheduler.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstPlugin.Scheduler.Handlers
{
    internal class PyrosTargetingHandler
    {
        internal static void Enqueue()
        {
            TaskPluginLog.Enqueue("Targeting Fate Targets");
            P.taskManager.Enqueue(PyrosFateTargeting, DConfig);
            TaskPluginLog.Enqueue("Done Targeting");
        }

        internal static bool PyrosFateTargeting()
        {
            IGameObject gameObject;
            TryGetClosestPyrosTarget(out gameObject);
            Svc.Targets.SetTarget(gameObject);
            if (Svc.ClientState.LocalPlayer!.IsDead)
                return true;

            if (!IsInFate())
                return true;
            
            if (gameObject.Name.ToString() == PyrosTargetTable[2] && gameObject.IsDead)
            {
                return true;
            }

            else if (DistanceToHitboxEdge(gameObject.HitboxRadius, gameObject) > SetAIRange() && !gameObject.IsDead)
            {
                if (!P.navmesh.PathfindInProgress() && !IsMoving() && !gameObject.IsDead && DistanceToHitboxEdge(gameObject.HitboxRadius, gameObject) > SetAIRange())
                    P.navmesh.PathfindAndMoveTo(gameObject.Position, false);
                return false;
            }

            else
            {
                P.navmesh.Stop();
            }

            return false;
        }
    }
}
