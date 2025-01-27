using ECommons.EzIpcManager;
using GCInfo = (uint ShopDataID, uint ExchangeDataID, System.Numerics.Vector3 Position);

namespace FirstPlugin.IPC
{
    public class AutoRetainerIPC
    {
        public const string Name = "AutoRetainer";
        public const string Repo = "https://love.puni.sh/ment.json";
        #pragma warning disable CS8618 
        public AutoRetainerIPC() => EzIPC.Init(this, Name);
        #pragma warning restore CS8618 
        public bool Installed => PluginInstalled(Name);

        [EzIPC] public readonly Func<bool> GetMultiModeEnabled;
        [EzIPC] public readonly Action<bool> SetMultiModeEnabled;

        [EzIPC("PluginState.%m")] public readonly Func<bool> IsBusy;
        [EzIPC("PluginState.%m")] public readonly Func<int> GetInventoryFreeSlotCount;
        [EzIPC("PluginState.%m")] public readonly Func<Dictionary<ulong, HashSet<string>>> GetEnabledRetainers;
        [EzIPC("PluginState.%m")] public readonly Func<bool> AreAnyRetainersAvailableForCurrentChara;
        [EzIPC("PluginState.%m")] public readonly Action AbortAllTasks;
        [EzIPC("PluginState.%m")] public readonly Action DisableAllFunctions;
        [EzIPC("PluginState.%m")] public readonly Action EnableMultiMode;
        /// <summary>
        /// Action onFailure
        /// </summary>
        [EzIPC("PluginState.%m")] public readonly Action<Action> EnqueueHET;
        [EzIPC("PluginState.%m")] public readonly Func<bool> CanAutoLogin;
        /// <summary>
        /// string charaNameWithWorld
        /// </summary>
        [EzIPC("PluginState.%m")] public readonly Func<string, bool> Relog;
        [EzIPC("PluginState.%m")] public readonly Func<bool> GetOptionRetainerSense;
        [EzIPC("PluginState.%m")] public readonly Action<bool> SetOptionRetainerSense;
        [EzIPC("PluginState.%m")] public readonly Func<int> GetOptionRetainerSenseThreshold;
        [EzIPC("PluginState.%m")] public readonly Action<int> SetOptionRetainerSenseThreshold;
        /// <summary>
        /// ulong CID
        /// </summary>
        [EzIPC("PluginState.%m")] public readonly Func<ulong, long?> GetClosestRetainerVentureSecondsRemaining;

        [EzIPC("GC.%m")] public readonly Action EnqueueInitiation;
        [EzIPC("GC.%m")] public readonly Func<GCInfo?> GetGCInfo;
    }
}
