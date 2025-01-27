using Dalamud.Game.ClientState.Objects.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstPlugin.Scheduler.Tasks
{
    internal static class TaskUseCarrot
    {
        public static void Enqueue()
        {
            uint thisAction = CarrotKeyItem;
            IGameObject? gameObject = null;

            P.taskManager.Enqueue(() => IsOffCooldownKey(thisAction), "Off Cooldown");
            P.taskManager.Enqueue(() => GetRecastElaspedKey(thisAction) == 0);
            P.taskManager.Enqueue(() => RunCommand("e Off CD"));
            P.taskManager.Enqueue(() => ExecuteKeyAction(thisAction));
        }
    }
}
