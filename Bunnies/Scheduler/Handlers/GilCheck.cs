using FFXIVClientStructs.FFXIV.Component.GUI;
using ECommons.Automation;
using ECommons.Automation.LegacyTaskManager;
using FFXIVClientStructs.FFXIV.Client.UI;
using ECommons.UIHelpers.AddonMasterImplementations;
using ECommons.DalamudServices;
using ECommons.Throttlers;
using System.Diagnostics;


namespace FirstPlugin.Scheduler.Handlers
{
    internal static unsafe class GilCheck
    {
        internal static TaskManager taskManager = new();
        static TaskManager TaskManager => taskManager;
        private static int PreviousGil = (int)GetGil();
        private static int PreviousEldthurs = GetItemCount(24219);
        internal static void CheckEverything()
        {
            int currentGil = (int)GetGil();
            int currentEldthurs = GetItemCount(24219);

            if (currentGil > PreviousGil)
            {
                int earnedGil = currentGil - PreviousGil;
                C.UpdateStats(Stats =>
                {
                    if (earnedGil == 100_000)
                        Stats.goldCoffer++;
                    else if (earnedGil == 25_000)
                        Stats.silverCoffer++;
                    else if (earnedGil == 10_000)
                        Stats.bronzeCoffer++;

                    if (earnedGil == 100_000 || earnedGil == 25_000 || earnedGil == 10_000)
                        Stats.gilEarned += earnedGil;
                });
                C.Save();
            }

            if (currentEldthurs > PreviousEldthurs)
            {
                int earnedEldthurs = currentEldthurs - PreviousEldthurs;
                C.UpdateStats(Stats => { Stats.eldthursCounter++; });
            }
            PreviousGil = currentGil;
            PreviousEldthurs = currentEldthurs;
        }
    }
}
