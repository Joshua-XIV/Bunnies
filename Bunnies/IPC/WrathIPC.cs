using ECommons.DalamudServices;
using ECommons.EzIpcManager;
using ECommons.Logging;
using ECommons.Reflection;
using System.ComponentModel;

namespace FirstPlugin.IPC;

internal class WrathIPC
{
#pragma warning disable CS8618
    private static EzIPCDisposalToken[] _disposalTokens =
        EzIPC.Init(typeof(WrathIPC), "WrathCombo", SafeWrapper.IPCException);
    public const string Name = "WrathCombo";
    internal static bool IsEnabled =>
        DalamudReflector.TryGetDalamudPlugin("WrathCombo", out _, false, true);
    public bool Installed => PluginInstalled(Name);
    internal static Guid? RoRLease;

    internal static Guid? CurrentLease
    {
        get
        {
            RoRLease ??= RegisterForLeaseWithCallback(
                "RootofRiches",
                "Root of Riches",
                "RoRIPC"
            );
            if (RoRLease is null)
                PluginLog.Warning("Failed to register for lease. " +
                                  "See logs from Wrath Como for why.");
            return RoRLease;
        }
    }

    /// <summary>
    ///     Why a lease was cancelled.
    /// </summary>
    public enum CancellationReason
    {
        [Description("The Wrath user manually elected to revoke your lease.")]
        WrathUserManuallyCancelled,

        [Description("Your plugin was detected as having been disabled, " +
                     "not that you're likely to see this.")]
        LeaseePluginDisabled,

        [Description("The Wrath plugin is being disabled.")]
        WrathPluginDisabled,

        [Description("Your lease was released by IPC call, " +
                     "theoretically this was done by you.")]
        LeaseeReleased,

        [Description("IPC Services have been disabled remotely. " +
                     "Please see the commit history for /res/ipc_status.txt. \n " +
                     "https://github.com/PunishXIV/WrathCombo/commits/main/res/ipc_status.txt")]
        AllServicesSuspended,
    }

    internal static void CancelActions(int reason, string s)
    {
        switch ((CancellationReason)reason)
        {
            case CancellationReason.WrathUserManuallyCancelled:
            case CancellationReason.LeaseePluginDisabled:
            case CancellationReason.WrathPluginDisabled:
            case CancellationReason.LeaseeReleased:
            case CancellationReason.AllServicesSuspended:
            default:
                break;
        }

        RoRLease = null;
        Svc.Log.Info($"Wrath lease cancelled via {(CancellationReason)reason} for: {s}");
    }

    [EzIPC] internal static readonly Func<string, string, Guid?> RegisterForLease;
    [EzIPC]
    internal static readonly Func<string, string, string?, Guid?>
        RegisterForLeaseWithCallback;
    [EzIPC] internal static readonly Action<Guid, bool> SetAutoRotationState;
    [EzIPC] internal static readonly Action<Guid> SetCurrentJobAutoRotationReady;
    [EzIPC]
    internal static readonly Action<Guid, AutoRotationConfigOption, object> SetAutoRotationConfigState;
    [EzIPC] internal static readonly Action<Guid> ReleaseControl;

    public enum AutoRotationConfigOption
    {
        InCombatOnly, //bool
        DPSRotationMode,
        HealerRotationMode,
        FATEPriority, //bool
        QuestPriority,//bool
        SingleTargetHPP,//int
        AoETargetHPP,//int
        SingleTargetRegenHPP,//int
        ManageKardia,//bool
        AutoRez,//bool
        AutoRezDPSJobs,//bool
        AutoCleanse,//bool
    }

    #pragma warning restore CS8618
}
