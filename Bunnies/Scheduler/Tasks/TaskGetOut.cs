using FFXIVClientStructs.FFXIV.Component.GUI;
using ECommons.Automation;

namespace FirstPlugin.Scheduler.Tasks
{
    internal class TaskGetOut
    {
        internal static void Enqueue()
        {
            TaskPluginLog.Enqueue("Getting out of NPC Menu's (Task GetOut)");
            P.taskManager.Enqueue(GetOut);
        }
        internal unsafe static bool GetOut()
        {
            if (PlayerNotBusy())
                return true;

            if (TryGetAddonByName<AtkUnitBase>("SelectString", out var addon) && IsAddonReady(addon))
                Callback.Fire(addon, true, -1);

            if (TryGetAddonByName<AtkUnitBase>("SelectIconString", out var addon2) && IsAddonReady(addon2))
                Callback.Fire(addon2, true, -1);

            if (TryGetAddonByName<AtkUnitBase>("RetainerList", out var addon3) && IsAddonReady(addon3))
                Callback.Fire(addon3, true, -1);

            if (TryGetAddonByName<AtkUnitBase>("Repair", out var addon4) && IsAddonReady(addon4))
                Callback.Fire(addon4, true, -1);

            if (TryGetAddonByName<AtkUnitBase>("ShopExchangeItem", out var addon5) && IsAddonReady(addon5))
                Callback.Fire(addon5, true, -1);

            return false;
        }
    }
}
