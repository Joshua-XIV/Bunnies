using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Components;
using ECommons.Automation.NeoTaskManager;
using ECommons.DalamudServices;
using ECommons.DalamudServices.Legacy;
using ECommons.GameFunctions;
using ECommons.GameHelpers;
using ECommons.ImGuiMethods;
using ECommons.Logging;
using ECommons.Reflection;
using ECommons.Throttlers;
using ImGuiNET;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.Control;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using FFXIVClientStructs.FFXIV.Component.GUI;
using System.Numerics;
using System.Runtime.InteropServices;
using FirstPlugin.IPC;
using Lumina.Excel.Sheets;
using FFXIVClientStructs.FFXIV.Client.Game.Fate;

namespace FirstPlugin;

public static unsafe class Helpers
{
    public static void RunCommand(string command)
    {
        ECommons.Automation.Chat.Instance.ExecuteCommand($"/{command}");
    }

    // Returns the current amount of gil the player has in their inventory
    public static unsafe uint GetGil() => InventoryManager.Instance()->GetGil();

    // Returns closest object to player by name
    internal static IGameObject? GetObjectByName(string name) => Svc.Objects.OrderBy(GetDistanceToPlayer).FirstOrDefault(o => o.Name.TextValue.Equals(name, StringComparison.CurrentCultureIgnoreCase));

    public static byte GetPlayerGC() => UIState.Instance()->PlayerState.GrandCompany;

    // Instance of the player
    public static GameObject* LPlayer() => GameObjectManager.Instance()->Objects.IndexSorted[0].Value;

    public static uint GetClassJobID() => Svc.ClientState.LocalPlayer!.ClassJob.RowId;

    // Config for the timeout limit of task enqueueing
    public static TaskManagerConfiguration DConfig => new(timeLimitMS: 10 * 60 * 1000, abortOnTimeout: false);

    internal static Random random = new Random();

#region ZONES_WORLD
    public static uint GetCurrentWorld() => Svc.ClientState.LocalPlayer?.CurrentWorld.RowId ?? 0;

    // Returns the current player's home world
    public static uint GetHomeWorld() => Svc.ClientState.LocalPlayer?.HomeWorld.RowId ?? 0;

    public static uint CurrentZoneID() => Svc.ClientState.TerritoryType;

    public static bool IsInZone(uint zoneID) => Svc.ClientState.TerritoryType == zoneID;
#endregion

#region ACTIONS AND COOLDOWNS
    // Execute actions of their respective type in-game
    public static unsafe void ExecuteActionGeneral(uint actionID) => ActionManager.Instance()->UseAction(ActionType.GeneralAction, actionID);
    public static unsafe void ExecuteAction(uint actionID) => ActionManager.Instance()->UseAction(ActionType.Action, actionID);
    public static unsafe void ExecuteKeyAction(uint actionID) => ActionManager.Instance()->UseAction(ActionType.KeyItem, actionID);

    // Returns if the actions if off cooldown of their respective type
    public static unsafe bool IsOffCooldown(uint actionID) => ActionManager.Instance()->IsActionOffCooldown(ActionType.Action, actionID);
    public static unsafe bool IsOffCooldownKey(uint actionID) => ActionManager.Instance()->IsActionOffCooldown(ActionType.KeyItem, actionID);

    // Gets the recast timer of certain actions respective of their type
    public static unsafe float GetRecast(uint actionID) => ActionManager.Instance()->GetRecastTime(ActionType.Action, actionID);
    public static unsafe float GetRecastKey(uint actionID) => ActionManager.Instance()->GetRecastTime(ActionType.KeyItem, actionID);
    public static unsafe float GetRecastElasped(uint actionID) => ActionManager.Instance()->GetRecastTimeElapsed(ActionType.Action, actionID);
    public static unsafe float GetRecastElaspedKey(uint actionID) => ActionManager.Instance()->GetRecastTimeElapsed(ActionType.KeyItem, actionID);

#endregion ACTIONS AND COOLDOWNS

#region ABANDONDUTY
    // Leaves the current duty the player is in
    private static readonly AbandonDuty ExitDuty = Marshal.GetDelegateForFunctionPointer<AbandonDuty>(Svc.SigScanner.ScanText("E8 ?? ?? ?? ?? 48 8B 43 28 41 B2 01"));
    private delegate void AbandonDuty(bool a1);
    public static void LeaveDuty() => ExitDuty(false);
#endregion

#region STATUS_CONDITIONS
    // Returns if the player is currently available
    public static bool PlayerNotBusy()
    {
        return Player.Available
               && Player.Object.CastActionId == 0
               && !IsOccupied()
               && !Svc.Condition[ConditionFlag.Jumping]
               && Player.Object.IsTargetable
               && !Player.IsAnimationLocked;
    }

    public static bool IsAtBunny()
    {
        var x = new Vector3(GetPlayerRawXPos(), GetPlayerRawYPos(), GetPlayerRawZPos());
        if (IsPointInTriangle2D(x, PyrosCenterFatePoint, PyrosRightFatePoint, PyrosLeftFatePoint))
            return true;
        else
            return false;
    }

    // Returns if the player is currently within level range of the Bunny fate
    public static bool Sync()
    {
        if (AgentHUD.Instance()->ExpContentLevel > FateManager.Instance()->CurrentFate->MaxLevel)
        {
            RunCommand("lsync");
            return false;
        }
        else
            return true;
    }

    public static bool IsInFate()
    {
        if (FateManager.Instance()->CurrentFate != null)
            return true;
        return false;
    }

    public static string? NameFate()
    {
        if (IsInFate())
            return FateManager.Instance()->CurrentFate->Name.ToString();
        else
            return null;
    }

    public static bool IsInBunnyFate()
    {
        if (IsInFate())
        {
            if (NameFate() == "We're All Mad Here")
                return true;
            else
                return false;
        }
        return false;
    }

    // Returns if the player has a given status ID
    public static bool HasStatus(uint status) => Svc.ClientState.LocalPlayer!.BattleChara()->GetStatusManager()->HasStatus(status);

    public static bool HasBunnyStatus() => HasStatus(BunnyStatusID);

    // Returns if the player is currently swapping between zones
    public static bool IsBetweenAreas => (Svc.Condition[ConditionFlag.BetweenAreas] || Svc.Condition[ConditionFlag.BetweenAreas51]);

#endregion

#region LOGGING
    // Returns the log of a given chat box
    public static string GetChatLogText(AddonChatLogPanel* chatLogPanel)
    {
        if (chatLogPanel == null) return string.Empty;

        AtkTextNode* chatTextNode = chatLogPanel->ChatText;
        if (chatTextNode == null) return string.Empty;

        return chatTextNode->NodeText.ToString();
    }

    // Returns if the given text matches "chatLog 0"
    public static bool MatchText(string text)
    {
        var ourChatLogPanel = (AddonChatLogPanel*)RaptureAtkUnitManager.Instance()->GetAddonByName("ChatLogPanel_0");
        if (ourChatLogPanel == null)
            return false;
        string chatText = GetChatLogText(ourChatLogPanel);
        return chatText.Contains(text);
    }

    // Returns if the given text matches the toast currently on screen
    public static bool MatchTextToast(string text)
    {
        var toastText = P.filter.GetLastToast();
        if (toastText == null) return false;
        return toastText.Contains(text);
    }

    // Given message will be placed in the log
    public static void PLogInfo(string message)
    {
        PluginLog.Information(message);
    }

    // Given message will be placed in debug window
    public static void PLogDebug(string message)
    {
        PluginLog.Debug(message);
    }
#endregion LOGGING

#region AUTOROTATION
    // Toggles all automation commands in-game to be on
    public static void ToggleRotationAI()
    {
        if (PluginInstalled("RotationSolver"))
        {
            RunCommand("rsr manual");
            RunCommand("rotation settings HostileType 0");
        }
        RunCommand("bmrai on");
        RunCommand("bmrai followcombat on");
        RunCommand("bmrai followoutofcombat on");
        RunCommand($"bmrai maxdistancetarget {SetAIRange()}");
    }

    // Toggles all automation commands in-game to be off
    public static void ToggleRotationAIOff()
    {
        RunCommand("bmrai off");
        RunCommand("rsr off");
    }

    // Sets the distance for the AI to walk to a target
    public static float SetAIRange()
    {
        var x = GetClassJobID();
        float range = 2.5f;
        switch (x)
        {
            // All classes without close range AOE
            case 7 or 25 or 33 or 35 or 42 or 26 or 27:
                range = 15;
                break;

            default:
                range = 2.5f;
                break;
        }
        return range;
    }

    // Toggle Rotation using BM or BMR or Wrath instead of RSR
    public static void ToggleRotation(bool enable)
    {
        if (enable)
        {
            float range = 2.8f;
            int altrange = 2;
            var j = Player.JobId;
            if (Svc.Data.GetExcelSheet<ClassJob>().TryGetRow(j, out var row))
            {
                switch (row.ClassJobCategory.RowId)
                {
                    case 30:
                        // Physical DPS Class;
                        range = 2.8f;
                        altrange = 2;
                        break;
                    case 31:
                        // Magicic DPS Class
                        range = 15.0f;
                        altrange = 15;
                        break;
                    default:
                        range = 2.8f;
                        break;
                }
            }

            if (PluginInstalled("WrathCombo"))
            {
                EnableWrathAuto();

                if (PluginInstalled("BossMod")) // If you have Veyns BossMod and Wrath Installed at the same time
                {
                    P.bossmod.AddPreset("ROR Passive", Resources.BMRotations.rootPassive);
                    P.bossmod.SetPreset("ROR Passive");
                    P.bossmod.SetRange(range);
                    RunCommand("vbm ai on");
                }
                if (PluginInstalled(AltBossMod)) // If you have... alternative bossmod installed & also Wrath
                {
                    RunCommand($"vbmai maxdistancetarget {altrange}");
                    RunCommand("vbmai on");
                    RunCommand("vbmai followtarget on");
                    RunCommand("vbmai followcombat on");
                }
            }
            else if (P.bossmod.Installed) // If you have ONLY Veyn's BossMod
            {
                RunCommand("vbm ai on");
                P.bossmod.AddPreset("RoR Boss", Resources.BMRotations.rootBoss);
                P.bossmod.SetPreset("RoR Boss");
                P.bossmod.SetRange(range);
            }
        }
        else if (!enable)
        {
            if (PluginInstalled("WrathCombo"))
            {
                //RunCommand("wrath auto off");
                P.bossmod.DisablePresets();
                ReleaseWrathControl();
                RunCommand("vbm ai off");
                if (PluginInstalled(AltBossMod))
                {
                    RunCommand("vbmai off");
                }
            }
            else
            {
                RunCommand("vbm ai off");
                P.bossmod.DisablePresets();
            }
        }
    }

#endregion AUTOROTATION

#region WRATH

    public static void EnableWrathAuto()
    {
        if (!WrathIPC.IsEnabled) return;
        try
        {
            var lease = (Guid)WrathIPC.CurrentLease!;
            // enable Wrath Combo Auto-Rotation

            WrathIPC.SetAutoRotationState(lease, true);
            // make sure the job is ready for Auto-Rotation

            WrathIPC.SetCurrentJobAutoRotationReady(lease);
            // if the job is ready, all the user's settings are locked
            // if the job is not ready, it turns on the job's simple modes, or if those don't
            // exist, it turns on the job's advanced modes with all options enabled
        }
        catch (Exception e)
        {
            PluginLog.Error("Unknown Wrath IPC error," +
                            "probably inability to register a lease." +
                            "\n" + e.Message);
        }
    }

    public static void EnableWrathAutoAndConfigureIt()
    {
        if (!WrathIPC.IsEnabled) return;
        try
        {
            var lease = (Guid)WrathIPC.CurrentLease!;
            WrathIPC.SetAutoRotationState(lease, true);
            WrathIPC.SetCurrentJobAutoRotationReady(lease);
            WrathIPC.SetAutoRotationConfigState(lease,
                WrathIPC.AutoRotationConfigOption.InCombatOnly, false);
            WrathIPC.SetAutoRotationConfigState(lease,
                WrathIPC.AutoRotationConfigOption.AutoRez, true);
            WrathIPC.SetAutoRotationConfigState(lease,
                WrathIPC.AutoRotationConfigOption.SingleTargetHPP, 60);
        }
        catch (Exception e)
        {
            PluginLog.Error("Unknown Wrath IPC error," +
                            "probably inability to register a lease." +
                            "\n" + e.Message);
        }
    }

    public static void ReleaseWrathControl()
    {
        if (!WrathIPC.IsEnabled) return;
        try
        {
            WrathIPC.ReleaseControl((Guid)WrathIPC.CurrentLease!);
            WrathIPC.BunniesLease = null;
        }
        catch (Exception e)
        {
            PluginLog.Error("Unknown Wrath IPC error," +
                            "probably inability to register a lease." +
                            "\n" + e.Message);
        }
    }

    #endregion WRATH

#region REPAIR
    // Returns if the player's gear condition are under a certain threshold
    public static unsafe bool NeedsRepair(float below = 0)
    {
        var im = InventoryManager.Instance();
        if (im == null)
        {
            Svc.Log.Error("InventoryManager was null");
            return false;
        }

        var equipped = im->GetInventoryContainer(InventoryType.EquippedItems);
        if (equipped == null)
        {
            Svc.Log.Error("InventoryContainer was null");
            return false;
        }

        if (!equipped->IsLoaded)
        {
            Svc.Log.Error($"InventoryContainer is not loaded");
            return false;
        }

        for (var i = 0; i < equipped->Size; i++)
        {
            var item = equipped->GetInventorySlot(i);
            if (item == null)
                continue;

            var itemCondition = Convert.ToInt32(Convert.ToDouble(item->Condition) / 30000.0 * 100.0);

            if (itemCondition <= below)
                return true;
        }

        return false;
    }
#endregion  INVENTORY

#region AUTORETAINER

    // Time in Unix
    public static int ToUnixTimestamp(this DateTime value) => (int)Math.Truncate(value.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds);

    // Uses autoreatiner API to return all characters in autoreatiner
    private static unsafe ParallelQuery<ulong> GetAllEnabledCharacters() => P.autoRetainerApi.GetRegisteredCharacters().AsParallel().Where(c => P.autoRetainerApi.GetOfflineCharacterData(c).Enabled);

    // Returns if any retainers are completed on current character
    public static unsafe bool ARRetainersWaitingToBeProcessed(bool allCharacters = false)
    {
        return !allCharacters
            ? P.autoRetainerApi.GetOfflineCharacterData(Svc.ClientState.LocalContentId).RetainerData.AsParallel().Any(x => x.HasVenture && x.VentureEndsAt <= DateTime.Now.ToUnixTimestamp())
            : GetAllEnabledCharacters().Any(character => P.autoRetainerApi.GetOfflineCharacterData(character).RetainerData.Any(x => x.HasVenture && x.VentureEndsAt <= DateTime.Now.ToUnixTimestamp()));
    }

    // Returns if any submarines are completed on current character
    public static unsafe bool ARSubsWaitingToBeProcessed(bool allCharacters = false)
    {
        return !allCharacters
            ? P.autoRetainerApi.GetOfflineCharacterData(Svc.ClientState.LocalContentId).OfflineSubmarineData.AsParallel().Any(x => x.ReturnTime <= DateTime.Now.ToUnixTimestamp())
            : GetAllEnabledCharacters().Any(c => P.autoRetainerApi.GetOfflineCharacterData(c).OfflineSubmarineData.Any(x => x.ReturnTime <= DateTime.Now.ToUnixTimestamp()));
    }

#endregion AUTORETAINER

#region INVENTORY

    // Gets the total count of an object, accounts for NQ and HQ unless specified
    public static unsafe int GetItemCount(int itemID, bool includeHQ = true)
    => includeHQ ? InventoryManager.Instance()->GetInventoryItemCount((uint)itemID, true) + InventoryManager.Instance()->GetInventoryItemCount((uint)itemID) + InventoryManager.Instance()->GetInventoryItemCount((uint)itemID + 500_000)
   : InventoryManager.Instance()->GetInventoryItemCount((uint)itemID) + InventoryManager.Instance()->GetInventoryItemCount((uint)itemID + 500_000);

    // Gets the total number of free slots in a character's invetory
    public static unsafe int GetInventoryFreeSlotCount()
    {
        InventoryType[] types = [InventoryType.Inventory1, InventoryType.Inventory2, InventoryType.Inventory3, InventoryType.Inventory4];
        var slots = 0;
        foreach (var x in types)
        {
            var cont = InventoryManager.Instance()->GetInventoryContainer(x);
            for (var i = 0; i < cont->Size; i++)
                if (cont->Items[i].ItemId == 0)
                    slots++;
        }
        return slots;
    }

    // Gets the total number of free slots in a containter within a character's inventory
    public static int GetFreeSlotsInContainer(int container)
    {
        var inv = InventoryManager.Instance();
        var cont = inv->GetInventoryContainer((InventoryType)container);
        var slots = 0;
        for (var i = 0; i < cont->Size; i++)
            if (cont->Items[i].ItemId == 0)
                slots++;
        return slots;
    }

#endregion INVENTORY

#region PlayerPositioning

    // Returns the player's position in a 3D Vector
    public static Vector3 PlayerPosition()
    {
        var player = LPlayer();
        return player != null ? player->Position : default;
    }
    
    // Return the player's X position
    public static float GetPlayerRawXPos()
    {
        return Svc.ClientState.LocalPlayer!.Position.X;
    }

    // Return the player's Y position
    public static float GetPlayerRawYPos()
    {
        return Svc.ClientState.LocalPlayer!.Position.Y;
    }

    // Return the player's Z position
    public static float GetPlayerRawZPos()
    {
        return Svc.ClientState.LocalPlayer!.Position.Z;
    }

    // Determines whether the player is within range of a set position given
    public static bool PlayerInRange(Vector3 dest, float dist)
    {
        var d = dest - PlayerPosition();
        return d.X * d.X + d.Z * d.Z <= dist * dist;
    }

    // If the player is in combat, will move the player towards the current hostile targets
    internal unsafe static bool? MoveToCombat(Vector3 targetPosition, float toleranceDistance = 3f)
    {
        if (targetPosition.Distance(Player.GameObject->Position) <= toleranceDistance || Svc.Condition[ConditionFlag.InCombat])
        {
            P.navmesh.Stop();
            return true;
        }
        if (!P.navmesh.IsReady()) { UpdateCurrentTask("Waiting Navmesh"); return false; }
        if (P.navmesh.PathfindInProgress() || P.navmesh.IsRunning() || IsMoving()) return false;

        P.navmesh.PathfindAndMoveTo(targetPosition, false);
        P.navmesh.SetAlignCamera(true);
        return false;
    }

    // This checks if a player is within a certain trianular area, this ignores the player's elevation
    public static bool IsPointInTriangle2D(Vector3 p, Vector3 a, Vector3 b, Vector3 c)
    {
        // Ignore the Y component and only work with X and Z
        Vector2 p2D = new Vector2(p.X, p.Z);
        Vector2 a2D = new Vector2(a.X, a.Z);
        Vector2 b2D = new Vector2(b.X, b.Z);
        Vector2 c2D = new Vector2(c.X, c.Z);

        // Compute vectors
        Vector2 v0 = c2D - a2D;
        Vector2 v1 = b2D - a2D;
        Vector2 v2 = p2D - a2D;

        // Compute dot products
        float dot00 = Vector2.Dot(v0, v0);
        float dot01 = Vector2.Dot(v0, v1);
        float dot02 = Vector2.Dot(v0, v2);
        float dot11 = Vector2.Dot(v1, v1);
        float dot12 = Vector2.Dot(v1, v2);

        // Compute barycentric coordinates
        float denom = (dot00 * dot11 - dot01 * dot01);
        if (denom == 0) return false; // Prevent divide by zero (degenerate triangle)
        float invDenom = 1 / denom;
        float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
        float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

        // Check if point is in triangle
        return (u >= 0) && (v >= 0) && (u + v <= 1);
    }

    // When given 3 points, will return a 3D Vector within that triangular shape
    public static Vector3 RandomPointInTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float r1 = (float)random.NextDouble();
        float r2 = (float)random.NextDouble();

        // Ensure the point is inside the triangle
        if (r1 + r2 > 1)
        {
            r1 = 1 - r1;
            r2 = 1 - r2;
        }

        // Calculate the random point
        return (1 - r1 - r2) * p1 + r1 * p2 + r2 * p3;
    }

    // This checks if the player is at the final boss location for the bunny fate
    public static bool IsPlayerAtBossLocation(Vector3 playerLocation)
    {
        Vector3 bossPosition = new Vector3(161.120f, 710.682f, 259.266f);
        if (GetDistanceToPoint(bossPosition[0], bossPosition[1], bossPosition[2]) < 13)
            return true;
        else
            return false;
    }


#endregion PlayerPositioning

#region Distance
    // Finds the distance from two given positions
    private static float Distance(this Vector3 v, Vector3 v2)
    {
        return new Vector2(v.X - v2.X, v.Z - v2.Z).Length();
    }

    // Returns if the player is currently moving or not
    public static unsafe bool IsMoving()
    {
        return AgentMap.Instance()->IsPlayerMoving;
    }

    // Returns the distance from the player to a given 3D vector
    internal static unsafe float GetDistanceToPlayer(Vector3 v3) => Vector3.Distance(v3, Player.GameObject->Position);
    // Returns the distance from the player to a game object
    internal static unsafe float GetDistanceToPlayer(IGameObject gameObject) => GetDistanceToPlayer(gameObject.Position);
    // Returns the distance from the player to a position given X, Y, Z
    public static float GetDistanceToPoint(float x, float y, float z) => Vector3.Distance(Svc.ClientState.LocalPlayer?.Position ?? Vector3.Zero, new Vector3(x, y, z));
    // Checks the distance from the player to the edge of the hitbox of an object
    public static unsafe float DistanceToHitboxEdge(float hitboxRadius, IGameObject gameObject) => GetDistanceToPlayer(gameObject) - hitboxRadius;
    // Checks if the player is currently in melee range
    public static unsafe bool IsInMeleeRange(float hitboxRadius, IGameObject gameobject) => DistanceToHitboxEdge(hitboxRadius, gameobject) < 2.5;
    #endregion Distance

#region Targeting
    internal static bool TryGetObjectByName(string name, out IGameObject? gameobject) => (gameobject = Svc.Objects.OrderBy(GetDistanceToPlayer).FirstOrDefault(x => x.Name.ToString() == name)) != null;
    internal static bool TryGetObjectByDataId(ulong dataId, out IGameObject? gameObject) => (gameObject = Svc.Objects.OrderBy(GetDistanceToPlayer).FirstOrDefault(x => x.DataId == dataId)) != null;
    internal static bool TryGetClosestEnemy(out IGameObject? gameObject) => (gameObject = Svc.Objects.OrderBy(GetDistanceToPlayer).FirstOrDefault(x => x.IsTargetable == true && x.IsHostile() == true)) != null;
    internal static bool TryGetClosestCoffer(out IGameObject? gameObject) => (gameObject = Svc.Objects.OrderBy(GetDistanceToPlayer).FirstOrDefault(x => x.IsTargetable == true && 
                                                                             (x.Name.ToString() == CofferTable[0] || x.Name.ToString() == CofferTable[1] || x.Name.ToString() == CofferTable[2]))) != null;
    internal static bool TryGetClosestPyrosTarget(out IGameObject? gameObject)
    {
        // Attempting to see if boss is dead
        gameObject = Svc.Objects.OrderBy(GetDistanceToPlayer).FirstOrDefault(x => x.IsTargetable && x.IsDead && x.Name.ToString() == PyrosTargetTable[2]);

        // If no valid target is found, check for remaining targets
        if (gameObject == null)
        {
            gameObject = Svc.Objects.OrderBy(GetDistanceToPlayer).FirstOrDefault(x => x.IsTargetable && x.IsHostile() && !x.IsDead &&
            (x.Name.ToString() == PyrosTargetTable[0] ||x.Name.ToString() == PyrosTargetTable[1] ||x.Name.ToString() == PyrosTargetTable[2]));
        }

        return gameObject != null;
    }

    internal static unsafe void InteractWithObject(IGameObject? gameObject)
    {
        try
        {
            if (gameObject == null || !gameObject.IsTargetable)
                return;
            var gameObjectPointer = (GameObject*)gameObject.Address;
            TargetSystem.Instance()->InteractWithObject(gameObjectPointer, false);
        }
        catch (Exception ex)
        {
            Svc.Log.Info($"InteractWithObject: Exception: {ex}");
        }
    }

    internal static bool? TargetByID(IGameObject? gameObject)
    {
        var x = gameObject;
        if (Svc.Targets.Target != null && Svc.Targets.Target.DataId == x.DataId)
            return true;

        if (!IsOccupied())
        {
            if (x != null)
            {
                if (EzThrottler.Throttle($"Throttle Targeting {x.DataId}"))
                {
                    Svc.Targets.SetTarget(x);
                    ECommons.Logging.PluginLog.Information($"Setting the target to {x.DataId}");
                }
            }
        }
        return false;
    }

    internal static bool? TargetByNameDistance(IGameObject? gameObject, string name, uint distance)
    {
        var x = gameObject;
        if (Svc.Targets.Target != null && Svc.Targets.Target.Name.ToString() == name)
            return true;

        if (!IsOccupied() )
        {
            if (x != null)
            {
                if (EzThrottler.Throttle($"Throttle Targeting {x.Name}") && GetDistanceToPlayer(x) <= distance)
                {
                    Svc.Targets.SetTarget(x);
                    ECommons.Logging.PluginLog.Information($"Setting the target to {x.Name} and the current distance is {GetDistanceToPlayer(x)}");
                }
            }
        }
        return false;
    }

    internal static bool? TargetByName(IGameObject? gameObject, string name)
    {
        var x = gameObject;
        if (Svc.Targets.Target != null && Svc.Targets.Target.Name.ToString() == name)
            return true;

        if (!IsOccupied())
        {
            if (x != null)
            {
                if (EzThrottler.Throttle($"Throttle Targeting {x.Name}"))
                {
                    Svc.Targets.SetTarget(x);
                    ECommons.Logging.PluginLog.Information($"Setting the target to {x.Name}");
                }
            }
        }
        return false;
    }

    #endregion Targeting

#region UI
    // GUI, creates a checkbox with a marker tooltip
    public static bool CheckboxWithTooltip(string label, ref bool value, string tooltip)
    {
        bool checkbox = ImGui.Checkbox(label, ref value);
        if (tooltip != null)
        {
            ImGuiComponents.HelpMarker(tooltip);
        }
        return checkbox;
    }

    // Returns if a dalamud plugin is currently installed
    public static bool PluginInstalled(string name)
    {
        return DalamudReflector.TryGetDalamudPlugin(name, out _, false, true);
    }

    // Returns if a dalamud addon is currently active
    public static bool IsAddonActive(string AddonName)
    {
        var addon = RaptureAtkUnitManager.Instance()->GetAddonByName(AddonName);
        return addon != null && addon->IsVisible && addon->IsReady;
    }

    // A notification given of the required plugins to run Bunnies
    public static void NotifyPlugins()
    {
        var x = "";
        if (!PluginInstalled("vnavmesh"))
            x += "vnavmesh\n";
        if (!PluginInstalled("RotationSolver"))
            x += "Rotation Solver Reborn\n";
        if (!PluginInstalled("BossModReborn"))
            x += "BossModReborn\n";
        Notify.Error($"Missing Required Plugins to run Bunnies \nRequired Plugins are \n{x}");
    }

    // Starting task when loading the plugin
    public static string icurrentTask = "idle";

    // Will update the task in the plugin's UI
    public static void UpdateCurrentTask(string task) { icurrentTask = task; }

    // GUI, creates a check if a plugin is enabled and cross if a plugin is disabled.
    public static void FancyCheckmark(bool enabled)
    {
        if (!enabled)
        {
            FontAwesome.Print(ImGuiColors.DalamudRed, FontAwesome.Cross);
        }
        else if (enabled)
        {
            FontAwesome.Print(ImGuiColors.HealerGreen, FontAwesome.Check);
        }
    }

    // GUI, uses "FancyCheckmark" along with a tooltip for the cross and checks for the user to read
    public static void FancyPluginUiString(bool PluginInstalled, string Text, string Url)
    {
        FancyCheckmark(PluginInstalled);
        if (ImGui.IsItemHovered())
        {
            ImGui.BeginTooltip();
            ImGui.Text("The following plugins are required to be installed and enabled: ");
            PluginGreenRedText(PluginInstalled, Text);
            ImGui.Text("Click to Copy Repo URL");
            ImGui.EndTooltip();
            if (ImGui.IsItemClicked())
            {
                ImGui.SetClipboardText(Url);
                DuoLog.Information("Repo URL Copied");
                Notify.Info("Repo URL Copied");
            }
        }

        ImGui.SameLine();
    }

    // Displays the plugin as red if it isn't install and green if it is installed.
    public static void PluginGreenRedText(bool PluginInstalled, string text)
    {
        if (PluginInstalled)
            ImGui.TextColored(ImGuiColors.HealerGreen, $"- {text}");
        else
            ImGui.TextColored(ImGuiColors.DalamudRed, $"- {text}");
    }

    // GUI, created selectable dropdown menus centered within the window
    public static void DrawMainSelectables(string label, ref bool show, Vector2 vector, float textstart)
    {
        ImGui.SetCursorPosX(0);
        if (ImGui.Selectable("##" + label, show, ImGuiSelectableFlags.None, vector))
            show = !show;
        ImGui.SameLine();
        ImGui.SetCursorPosX(textstart);
        ImGui.Text(label);
        ImGui.Spacing();
    }
    #endregion UI
}
