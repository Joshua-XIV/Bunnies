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
            if (Svc.ClientState.LocalPlayer!.IsDead)
                return true;

            if (!IsInFate())
                return true;

            if (Svc.Targets.Target.Name.ToString() == PyrosTargetTable[2] && Svc.Targets.Target.IsDead)
            {
                return true;
            }

            else if (Svc.Targets.Target != null)
            {
                if (GetDistanceToPlayer(Svc.Targets.Target.Position) > SetAIRange() && !Svc.Targets.Target.IsDead)
                {
                    if (!IsMoving())
                        P.navmesh.PathfindAndMoveTo(Svc.Targets.Target.Position, false);
                    else
                        P.navmesh.Stop();
                }
                else
                    P.navmesh.Stop();
                return false;
            }

            else if(Svc.Targets.Target == null)
            {
                TryGetClosestPyrosTarget(out gameObject);
                Svc.Targets.SetTarget(gameObject);
                return false;
            }

            return false;
        }
    }
}
