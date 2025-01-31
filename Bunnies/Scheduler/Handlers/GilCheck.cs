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
        private static int PreviousGil = (int)GetGil();
        private static int PreviousEldthurs = GetItemCount(24219);
        private static int PreviousPryosHairStyle = GetItemCount(24233);
        internal static void CheckEverything()
        {
            int currentGil = (int)GetGil();
            int currentEldthurs = GetItemCount(24219);
            int currentPyroHairStyle = GetItemCount(24233);

            if (currentGil > PreviousGil)
            {
                int earnedGil = currentGil - PreviousGil;
                C.UpdateStats(Stats =>
                {
                    if (earnedGil == 100_000)
                    {
                        Stats.goldCoffer++;
                    }
                    else if (earnedGil == 25_000)
                    {
                        Stats.silverCoffer++;
                    }
                    else if (earnedGil == 10_000)
                    {
                        Stats.bronzeCoffer++;
                    }

                    if (earnedGil == 100_000 || earnedGil == 25_000 || earnedGil == 10_000)
                    {
                        Stats.gilEarned += earnedGil;
                    }
                });
                if (CurrentZoneID() == Pagos)
                {
                    C.UpdatePagosStats(Stats =>
                    {
                        if (earnedGil == 100_000)
                        {
                            Stats.goldCoffer++;
                        }
                        else if (earnedGil == 25_000)
                        {
                            Stats.silverCoffer++;
                        }
                        else if (earnedGil == 10_000)
                        {
                            Stats.bronzeCoffer++;
                        }

                        if (earnedGil == 100_000 || earnedGil == 25_000 || earnedGil == 10_000)
                        {
                            Stats.gilEarned += earnedGil;
                        }
                    });
                }
                else if (CurrentZoneID() == Pyros)
                {
                    C.UpdatePyrosStats(Stats =>
                    {
                        if (earnedGil == 100_000)
                        {
                            Stats.goldCoffer++;
                        }
                        else if (earnedGil == 25_000)
                        {
                            Stats.silverCoffer++;
                        }
                        else if (earnedGil == 10_000)
                        {
                            Stats.bronzeCoffer++;
                        }

                        if (earnedGil == 100_000 || earnedGil == 25_000 || earnedGil == 10_000)
                        {
                            Stats.gilEarned += earnedGil;
                        }
                    });
                }
                else if (CurrentZoneID() == Hydatos)
                {
                    C.UpdateHydatosStats(Stats =>
                    {
                        if (earnedGil == 100_000)
                        {
                            Stats.goldCoffer++;
                        }
                        else if (earnedGil == 25_000)
                        {
                            Stats.silverCoffer++;
                        }
                        else if (earnedGil == 10_000)
                        {
                            Stats.bronzeCoffer++;
                        }

                        if (earnedGil == 100_000 || earnedGil == 25_000 || earnedGil == 10_000)
                        {
                            Stats.gilEarned += earnedGil;
                        }
                    });
                }
            }

            if (currentEldthurs > PreviousEldthurs)
            {
                int earnedEldthurs = currentEldthurs - PreviousEldthurs;
                C.UpdateStats(Stats => { Stats.eldthursCounter++; });
                C.UpdatePyrosStats(PyrosStats => { PyrosStats.eldthursCounter++; });
            }

            if (currentPyroHairStyle > PreviousPryosHairStyle)
            {
                int earnedPyrosHairStyle = currentPyroHairStyle - PreviousPryosHairStyle;
                C.UpdateStats(Stats => { Stats.pyrosHairStyleCounter++; });
                C.UpdatePyrosStats(PyrosStats => { PyrosStats.pyrosHairStyleCounter++; });
            }
            PreviousGil = currentGil;
            PreviousEldthurs = currentEldthurs;
            PreviousPryosHairStyle = currentPyroHairStyle;
            C.Save();
        }
    }
}
