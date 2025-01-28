using ECommons.Logging;
using FirstPlugin.Scheduler.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FirstPlugin.Scheduler.Handlers.CheckCofferHandler;
using static FirstPlugin.Scheduler.Handlers.EurekaMovementHandler;

namespace FirstPlugin.Scheduler.Handlers
{
    internal class PyrosMovementHandler
    {
        internal static void InitialStart()
        {
            CofferState.FoundCoffer = false;
            TaskUseCarrot.Enqueue();
            P.taskManager.Enqueue(() => CheckDirection.CheckDirections(InitialSearch.IntialSearchOptions));
            P.taskManager.Enqueue(() =>
            {
                if (CheckDirection.direction == InitialSearch.IntialSearchOptions["InitialEast"])
                    P.taskManager.Enqueue(() => GoToEastLocation());
                else if (CheckDirection.direction == InitialSearch.IntialSearchOptions["InitialSouthEast"])
                    P.taskManager.Enqueue(() => GoToSouthEastLocation());
                else if (CheckDirection.direction == InitialSearch.IntialSearchOptions["InitialSouth"])
                    P.taskManager.Enqueue(() => GoToSouthLocation());
                else if (CheckDirection.direction == InitialSearch.IntialSearchOptions["InitialSouthWest"])
                    P.taskManager.Enqueue(() => GoToSouthWestLocation());
                else if (CheckDirection.direction == InitialSearch.IntialSearchOptions["InitialWest"])
                    P.taskManager.Enqueue(() => GoToWestLocation());
                else if (CheckDirection.direction == InitialSearch.IntialSearchOptions["InitialNorthEast"])
                    P.taskManager.Enqueue(() => GoToNorthEastLocation());
                else
                    P.taskManager.Enqueue(() => PluginLog.Information("Did not find directions associated with key")); //######################### Change Later
            });
        }
        internal static void GoToEastLocation()
        {
            MoveThroughLocations(East.FarFarEastCoffers, East.direction);
        }

        internal static void GoToSouthEastLocation()
        {
            GoToCheckPoint(SouthEast.CheckPoint1, SouthEast.direction);
            TaskUseCarrot.Enqueue();
            P.taskManager.Enqueue(() => CheckDirection.CheckDirections(SouthEast.CheckPoint1Search));
            P.taskManager.Enqueue(() =>
            {
                if (CheckDirection.direction == "")
                    P.taskManager.Enqueue(() => InitialStart()); //######################### Change Later
                else if (CheckDirection.direction == SouthEast.CheckPoint1Search["CloseSouthCP1"])
                    MoveThroughLocations(SouthEast.CloseSouthCP1.CloseSouthCP1Coffers, SouthEast.CloseSouthCP1.direction);
                else if (CheckDirection.direction == SouthEast.CheckPoint1Search["FarSouthEastCP1"])
                    MoveThroughLocations(SouthEast.FarSouthEastCP1.FarSouthEastCP1Coffers, SouthEast.FarSouthEastCP1.direction);
                else if (CheckDirection.direction == SouthEast.CheckPoint1Search["CloseEastCP1"])
                    MoveThroughLocations(SouthEast.CloseEastCP1.CloseEastCP1Coffers, SouthEast.CloseEastCP1.direction);
                else
                    MoveThroughLocations(SouthEast.RemainingCoffersCP1.RemainingCP1Coffers, SouthEast.RemainingCoffersCP1.direction);
            });
        }

        internal static void GoToSouthLocation()
        {
            GoToCheckPoint(South.CheckPoint1, South.direction);
            TaskUseCarrot.Enqueue();
            P.taskManager.Enqueue(() => CheckDirection.CheckDirections(South.CheckPoint1Search));
            P.taskManager.Enqueue(() =>
            {
                if (CheckDirection.direction == "")
                    P.taskManager.Enqueue(() => InitialStart()); //######################### Change Later
                else if (CheckDirection.direction == South.CheckPoint1Search["CloseEastCP1"])
                    MoveThroughLocations(South.CloseEastCP1.CloseEastCP1Coffers, South.CloseEastCP1.direction);
                else if (CheckDirection.direction == South.CheckPoint1Search["FarSouthEastCP1"])
                    MoveThroughLocations(South.FarSouthEastCP1.FarSouthCP1Coffers, South.FarSouthEastCP1.direction);
                else if (CheckDirection.direction == South.CheckPoint1Search["FarSouthWestCP1"])
                {
                    MoveThroughLocations(South.FarSouthWestCP1.FarSouthWestCP1Coffers, South.FarSouthWestCP1.direction);
                    P.taskManager.Enqueue(() => P.taskManager.Enqueue(() => TaskMoveTo.Enqueue(new System.Numerics.Vector3(73.68f, 754.02f, 704.99f), "Offset")));
                }

                else if (CheckDirection.direction == South.CheckPoint1Search["CloseSouthCP1"])
                    MoveThroughLocations(South.CloseSouthCP1.CloseSouthCP1Coffers, South.CloseSouthCP1.direction);
                else if (CheckDirection.direction == South.CheckPoint1Search["FarSouthCP1"])
                    MoveThroughLocations(South.FarSouthCP1.FarSouthCP1Coffers, South.FarSouthCP1.direction);
                else
                    MoveThroughLocations(South.RemainingCP1.RemainingCP1Coffers, South.RemainingCP1.direction);
            });
        }

        internal static void GoToSouthWestLocation()
        {
            GoToCheckPoint(SouthWest.CheckPoint1, SouthWest.direction);
            TaskUseCarrot.Enqueue();
            P.taskManager.Enqueue(() => CheckDirection.CheckDirections(SouthWest.CheckPoint1Search));
            P.taskManager.Enqueue(() =>
            {
                if (CheckDirection.direction == "")
                    P.taskManager.Enqueue(() => InitialStart()); //######################### Change Later
                else if (CheckDirection.direction == SouthWest.CheckPoint1Search["FarNorthCP1"])
                {
                    MoveThroughLocations(SouthWest.FarNorthCP1.FarNorthCP1Coffers, SouthWest.FarNorthCP1.direction);
                    P.taskManager.Enqueue(() => { P.taskManager.Enqueue(() => MoveToCliff()); });
                }
                else if (CheckDirection.direction == SouthWest.CheckPoint1Search["FarFarNorthCP1"])
                {
                    MoveToCliff();
                    MoveThroughLocations(SouthWest.FarFarNorthCP1.FarFarNorthCP1Coffers, SouthWest.FarFarNorthCP1.direction);
                    return;
                }
                else if (CheckDirection.direction == SouthWest.CheckPoint1Search["CloseNorthWestCP1"])
                {
                    MoveThroughLocations(SouthWest.CloseNorthWestCP1.CloseNorthWestCP1Coffers, SouthWest.CloseNorthWestCP1.direction);
                    P.taskManager.Enqueue(() => { P.taskManager.Enqueue(() => MoveToCliff()); });
                }
                else if (CheckDirection.direction == SouthWest.CheckPoint1Search["FarFarWestCP1"])
                {
                    MoveThroughLocations(SouthWest.FarFarWestCP1.FarFarWestCP1Coffers, SouthWest.FarFarWestCP1.direction);
                    P.taskManager.Enqueue(() => P.taskManager.Enqueue(() => { P.taskManager.Enqueue(() => MoveToCliff()); }));
                }
                else if (CheckDirection.direction == SouthWest.CheckPoint1Search["CloseSouthWestCP1"])
                {
                    MoveThroughLocations(SouthWest.CloseSouthWestCP1.CloseSouthWestCP1Coffers, SouthWest.CloseSouthWestCP1.direction);
                    P.taskManager.Enqueue(() => { P.taskManager.Enqueue(() => MoveToCliff()); });
                }
                else if (CheckDirection.direction == SouthWest.CheckPoint1Search["FarFarSouthWestCP1"])
                {
                    JumpLocation(SouthWest.FarFarSouthWestCP1.direction);
                    P.taskManager.Enqueue(() => { P.taskManager.Enqueue(() => MoveToCliff()); });
                }
                else
                {
                    MoveThroughLocations(SouthWest.RemainingCP1.RemainingCP1Coffers, SouthWest.RemainingCP1.direction);
                    P.taskManager.Enqueue(() => { P.taskManager.Enqueue(() => MoveToCliff()); });
                }
            });
        }

        internal static void GoToWestLocation()
        {
            GoToCheckPoint(West.CheckPoint1, West.direction);
            TaskUseCarrot.Enqueue();
            P.taskManager.Enqueue(() => CheckDirection.CheckDirections(West.CheckPoint1Search));
            P.taskManager.Enqueue(() =>
            {
                if (CheckDirection.direction == West.CheckPoint1Search["CloseSouthCP1"])
                {
                    MoveThroughLocations(West.CloseSouthCP1.CloseSouthCP1Coffers, West.CloseSouthCP1.direction);
                    return;
                }
            });
            P.taskManager.Enqueue(() => GoToCheckPoint(West.CheckPoint2, West.direction));
            P.taskManager.Enqueue(() =>
            {
                TaskUseCarrot.Enqueue();
                P.taskManager.Enqueue(() => CheckDirection.CheckDirections(West.CheckPoint2Search));
                P.taskManager.Enqueue(() =>
                {
                    if (CheckDirection.direction == "")
                        P.taskManager.Enqueue(() => InitialStart()); // ################ CHANGE LATER
                    else if (CheckDirection.direction == West.CheckPoint2Search["CloseSouthCP2"])
                        MoveThroughLocations(West.CloseSouthCP2.CloseSouthCP2Coffers, West.CloseSouthCP2.direction);
                    else
                        MoveThroughLocations(West.RemainingCP2.RemainingCP2Coffers, West.RemainingCP2.direction);
                });
            });
        }

        internal static void GoToNorthEastLocation()
        {
            MoveThroughLocations(NorthEast.CloseNorthEastCoffers, NorthEast.direction);
        }
    }
}
