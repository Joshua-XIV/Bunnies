using ECommons.DalamudServices;
using ECommons.Logging;
using FFXIVClientStructs.FFXIV.Component.GUI;
using FirstPlugin.Scheduler.Handlers;
using FirstPlugin.Scheduler.Tasks;
using FirstPlugin.Ui.MainWindow;
using System.Numerics;

namespace FirstPlugin.Scheduler
{
    internal static unsafe class SchedulerMain
    {
        public static bool HadAutoChestOn = false;
        public static bool HadAutoInteractOn = false;
        public static bool RunTurnin = false; // Used for Turnin Toggle
        public static bool BunniesRun = false; // Used for N-Raid Toggle
        public static bool hasEnqueuedDutyFinder = false; // used for enque throtle flag
        public static string BunniesTask = "idle";
        public static bool InitatedRotation = false;
        private static uint PreviousArea = 0;
        private static int ZoneSelected = 0;

        internal static bool AreWeTicking;
        internal static bool DoWeTick
        {
            get => AreWeTicking;
            private set => AreWeTicking = value;
        }

        internal static bool EnablePlugin()
        {
            if (P.pandora.GetFeatureEnabled("Automatically Open Chests"))
            {
                HadAutoChestOn = true;
                P.pandora.SetFeatureEnabled("Automatically Open Chests", false);
            }
            if (P.pandora.GetFeatureEnabled("Auto-interact with Objects in Instances"))
            {
                HadAutoInteractOn = true;
                P.pandora.SetFeatureEnabled("Auto-interact with Objects in Instances", false);
            }
            if (PluginInstalled("RotationSolver"))
            {
                RunCommand("rsr settings AutoOpenChest false");
            }
            StartBunnies.IsRunning = true;
            BunniesRun = true;
            DoWeTick = true;
            return true;
        }

        internal static bool DisablePlugin()
        {
            ToggleRotationAIOff();
            StartBunnies.IsRunning = false;
            DoWeTick= false;
            P.taskManager.Abort();
            P.navmesh.Stop();
            BunniesRun= false;
            P.stopwatch.Restart();
            P.stopwatch.Stop();
            BunniesTask = "idle";
            UpdateCurrentTask("idle");
            if (InitatedRotation)
            {
                ToggleRotation(false);
                InitatedRotation = false;
            }
            if (HadAutoChestOn)
                P.pandora.SetFeatureEnabled("Automatically Open Chests", false);
            if (HadAutoInteractOn)
                P.pandora.SetFeatureEnabled("Auto-interact with Objects in Instances", false);
            HadAutoChestOn = false;
            HadAutoChestOn = false;
            return true;
        }

        internal static void Tick()
        {
            if (DoWeTick)
            {
                if (!P.taskManager.IsBusy)
                {
                    if (BunniesRun)
                    {
                        if (IsInZone(Pagos) || IsInZone(Pyros) || IsInZone(Hydatos))
                        {
                            P.stopwatch.Start();
                            double timeElasped = P.stopwatch.Elapsed.TotalSeconds;
                            uint ZoneID = CurrentZoneID();
                            if (timeElasped > (C.hours * 3600 + C.minutes * 60) && !C.runInfinite && !HasBunnyStatus() && !IsInBunnyFate())
                            {
                                P.taskManager.Enqueue(() => UpdateCurrentTask("Leaving Duty"));
                                P.taskManager.Enqueue(LeaveDuty);
                                P.taskManager.Enqueue(() => IsInZone(Kugane) && PlayerNotBusy());
                                if (C.teleportToHouse)
                                    if (C.teleportToFC)
                                    {
                                        P.taskManager.Enqueue(() => UpdateCurrentTask("Going to FC"));
                                        TaskGoToFC.Enqueue();
                                    }
                                    else if (C.teleportToHouse)
                                    {
                                        P.taskManager.Enqueue(() => UpdateCurrentTask("Going to Personal"));
                                        TaskGoToHome.Enqueue();
                                    }
                                    else
                                        P.taskManager.Enqueue(() => PluginLog.LogInformation("Either House doesn't exist or the task failed."));

                                if (C.logoutAfter)
                                {
                                    P.taskManager.Enqueue(() => UpdateCurrentTask("Logging Out"));
                                    P.taskManager.Enqueue(() => RunCommand("logout"));
                                    P.taskManager.Enqueue(() => IsAddonActive("SelectYesno"));
                                    P.taskManager.Enqueue(() => GenericHandler.FireCallback("SelectYesno", true, 0));
                                }
                                P.taskManager.Enqueue(DisablePlugin);
                            }
                            else if (ARRetainersWaitingToBeProcessed() && C.enableRetainers && !HasBunnyStatus() && !IsInBunnyFate())
                            {
                                P.taskManager.Enqueue(() => UpdateCurrentTask("Resending Retainers"));
                                if (CurrentZoneID() == Pagos || CurrentZoneID() == Pyros || CurrentZoneID() == Hydatos)
                                    P.taskManager.Enqueue(LeaveDuty);
                                P.taskManager.Enqueue(() => CurrentZoneID() == Kugane);
                                P.taskManager.Enqueue(PlayerNotBusy);
                                P.taskManager.EnqueueDelay(1000);
                            }
                            else if (ZoneID == Pagos)
                            {
                                // Go To Bunny Location
                                // Wait for Fate
                                // Do Fate
                            }

                            else if (ZoneID == Pyros)
                            {
                                // Go To Bunny Location
                                // Wait for Fate
                                // Do Fate
                                if (HasBunnyStatus())
                                {
                                    ToggleRotationAIOff();
                                    if (!IsPlayerAtBossLocation(Svc.ClientState.LocalPlayer!.Position))
                                    {
                                        TaskPluginLog.Enqueue("Going to Boss Location");
                                        TaskMoveTo.Enqueue(new Vector3(161.120f, 710.682f, 259.266f), "Boss");
                                    }
                                    TaskMounting.Enqueue();
                                    P.taskManager.Enqueue(() => RunCommand("rsr off"));
                                    TaskPluginLog.Enqueue("Has Bunny");
                                    P.taskManager.Enqueue(() => PyrosMovementHandler.InitialStart());
                                }

                                else if (!HasBunnyStatus())
                                {
                                    if (C.enableRepair && NeedsRepair(C.repairSlider) && !IsInBunnyFate())
                                    {
                                        P.taskManager.Enqueue(() => PluginLog.Information("Need Repair"));
                                        if (C.selfRepair)
                                        {
                                            P.taskManager.Enqueue(() => UpdateCurrentTask("Self Repairing"));
                                            TaskDismount.Enqueue();
                                            TaskSelfRepair.Enqueue();
                                        }
                                        else
                                        {
                                            P.taskManager.Enqueue(() => UpdateCurrentTask("Going to Repair Mender"));
                                            TaskMounting.Enqueue();
                                            TaskMoveTo.Enqueue(PyrosRepairNpc, "Pyros Mender", 0.5f);
                                            TaskRepairNpc.Enqueue("Expedition Mender");
                                        }    
                                    }

                                    else if (IsInBunnyFate())
                                    {
                                        ToggleRotationAI();
                                        P.taskManager.Enqueue(Sync);
                                        TaskDismount.Enqueue();
                                        TaskPluginLog.Enqueue("Inside Bunny Fate");
                                        P.taskManager.Enqueue(() => UpdateCurrentTask("In Bunny Fate"));
                                        PyrosTargetingHandler.Enqueue();
                                        TaskMounting.Enqueue();
                                        P.taskManager.Enqueue(() => HasBunnyStatus());
                                        TaskPluginLog.Enqueue("Finidng Coffer");
                                    }

                                    else if (IsAtBunny())
                                    {
                                        P.taskManager.Enqueue(() => RunCommand("rsr off"));
                                        P.taskManager.Enqueue(() => UpdateCurrentTask("Waiting at Fate"));
                                        P.taskManager.EnqueueDelay(100);
                                    }

                                    else if (!IsAtBunny())
                                    {
                                        var x = RandomPointInTriangle(PyrosCenterFatePoint,PyrosRightFatePoint,PyrosLeftFatePoint);
                                        TaskMounting.Enqueue();
                                        TaskMoveTo.Enqueue(x, "Bunny Fate Location", 1f);
                                    }
                                }
                            }

                            else if (ZoneID == Hydatos)
                            {
                                // Run Hydatos Bunny
                            }

                            else
                            {
                                PluginLog.Information("Not in Zones, disabling plugin.");
                                DisablePlugin();
                            }
                        }
                        else if (!ARRetainersWaitingToBeProcessed() && (TryGetAddonByName<AtkUnitBase>("RetainerList", out var RetainerAddon) && IsAddonReady(RetainerAddon)))
                        {
                            TaskGetOut.Enqueue();
                        }
                        else if (IsInZone(Kugane) && ARRetainersWaitingToBeProcessed() && C.enableRetainers)
                        {
                            var closest = AethernetData.Distances.OrderBy(x => x.distance).First();
                            P.taskManager.Enqueue(() => UpdateCurrentTask("Resending Retainers"));
                            if (closest.position != AethernetKogane)
                                TaskUseAethernet.Enqueue("Kogane Dori Markets");
                            TaskMoveTo.Enqueue(SummoningBell, "SummoningBell", 1f);
                            TaskUseRetainer.Enqueue();
                        }
                        else if (IsInZone(Kugane))
                        {
                            UpdateCurrentTask("Going to Eureka");
                            var closest = AethernetData.Distances.OrderBy(x => x.distance).First();
                            if (closest.position != AethernetPier1)
                                TaskUseAethernet.Enqueue("Pier #1");
                            TaskMoveToKuganeNpc.Enqueue();
                            TaskTarget.Enqueue(KuganeNpcObjectID);
                            TaskInteract.Enqueue(KuganeNpcObjectID);
                            P.taskManager.Enqueue(() =>
                            {
                                if (C.zoneSelected == 0)
                                {
                                    // Go To Pagos
                                }
                                else if (C.zoneSelected == 1)
                                {
                                    // Go To Pyros
                                    UpdateCurrentTask("Entering Pyros");
                                    P.taskManager.Enqueue(() => GenericHandler.FireCallback("SelectString", true, 1));
                                    P.taskManager.Enqueue(() => GenericHandler.FireCallback("SelectYesno", true, 0));
                                    P.taskManager.EnqueueDelay(200);
                                    P.taskManager.Enqueue(() => GenericHandler.FireCallback("ContentsFinderConfirm", true, 8));
                                    P.taskManager.Enqueue(() => CurrentZoneID() == Pyros);
                                    P.taskManager.Enqueue(PlayerNotBusy);
                                    P.taskManager.Enqueue(P.navmesh.IsReady);

                                }
                                else if (C.zoneSelected == 2)
                                {
                                    // Go To Hydatos
                                }
                                else
                                {
                                    PluginLog.Information("Zone Selected Not Valid, disabling plugin");
                                    DisablePlugin();
                                }
                            });
                        }

                        else if (!IsInZone(Kugane))
                        {
                            TaskTeleportKugane.Enqueue();
                            if (ARRetainersWaitingToBeProcessed() && C.enableRetainers)
                            {
                                P.taskManager.Enqueue(() => UpdateCurrentTask("Resending Retainers"));
                                TaskUseAethernet.Enqueue("Kogane Dori Markets");
                                TaskMoveTo.Enqueue(SummoningBell, "SummoningBell", 1f);
                                TaskUseRetainer.Enqueue();
                            }
                        }
                    }
                }
            }
        }
    }
}
