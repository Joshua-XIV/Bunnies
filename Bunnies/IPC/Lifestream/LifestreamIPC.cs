using ECommons.EzIpcManager;
using System.Numerics;


namespace FirstPlugin.IPC.Lifestream
{
    public class LifestreamIPC
    {
        public const string Name = "Lifestream";
        public const string Repo = "https://github.com/NightmareXIV/MyDalamudPlugins/raw/main/pluginmaster.json";
        public LifestreamIPC() => EzIPC.Init(this, Name, SafeWrapper.AnyException);
        public bool Installed => PluginInstalled(Name);

        [EzIPC] public Func<string, bool> AethernetTeleport;
        [EzIPC] public Func<uint, byte, bool> Teleport;
        [EzIPC] public Func<bool> TeleportToHome;
        [EzIPC] public Func<bool> TeleportToFC;
        [EzIPC] public Func<bool> TeleportToApartment;
        [EzIPC] public Func<bool> IsBusy;
        /// <summary>
        /// city aetheryte id
        /// </summary>
        [EzIPC] public Func<int, uint> GetResidentialTerritory;
        /// <summary>
        /// content id
        /// </summary>
        [EzIPC] public Func<ulong, (HousePathData Private, HousePathData FC)> GetHousePathData;
        /// <summary>
        /// territory, plot
        /// </summary>
        [EzIPC] public Func<uint, int, Vector3?> GetPlotEntrance;
        /// <summary>
        /// type(home=1, fc=2, apartment=3), mode(enter house=2)
        /// </summary>
        [EzIPC] public Action<int, int?> EnqueuePropertyShortcut;
        [EzIPC] public Func<(int Kind, int Ward, int Plot)?> GetCurrentPlotInfo;
        [EzIPC] public Action<string> ExecuteCommand;
        [EzIPC] public Func<bool?> HasApartment;
        [EzIPC] public Action<bool> EnterApartment;
        [EzIPC] public Func<bool?> HasPrivateHouse;
        [EzIPC] public Func<bool?> HasFreeCompanyHouse;
        [EzIPC] public Func<bool> CanMoveToWorkshop;
        [EzIPC] public Action MoveToWorkshop;
    }
}
