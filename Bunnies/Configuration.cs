using ECommons.Configuration;
using Newtonsoft.Json.Bson;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace FirstPlugin;

public class Config : IEzConfig
{
    [JsonIgnore]
    public const int CURRENT_VERSION = 0;
    public int version = CURRENT_VERSION;

    public class BattleTalkPattern
    {
        public Regex Pattern { get; set; }
        public bool ShowMessage { get; set; }

        public BattleTalkPattern(Regex pattern, bool showMessage)
        {
            this.Pattern = pattern;
            this.ShowMessage = showMessage;
        }
    }

    public List<Regex> Patterns { get; set; } = new();
    public List<BattleTalkPattern> BattleTalkPatterns { get; set; } = new();

    // General Config
    public bool teleportToHouse { get; set; } = false;
    public bool teleportToPersonal { get; set; } = false;
    public bool teleportToFC { get; set; } = false;
    public bool logoutAfter { get; set; } = false;

    // Time Config
    public bool runInfinite { get; set; } = true;
    public int hours { get; set; } = 1;
    public int minutes { get; set; } = 0;
    public double totalTimeSet { get; set; } = 0;

    // Repair Config
    public float repairSlider { get; set; } = 30f;
    public bool enableRepair { get; set; } = false;
    public bool selfRepair { get; set; } = false;

    // Zone Config
    public sbyte zoneSelected { get; set; } = 0;

    // AutoReatiner Config
    public bool enableRetainers { get; set; } = false;
    public bool enableSubs {  get; set; } = false;
    public bool enableMulti {  get; set; } = false;

    // Stats
    public Stats stats { get; set; } = new Stats();
    public PagosStats pagosStats { get; set; } = new PagosStats();
    public PagosStats pagosSessionStats { get; set; } = new PagosStats();
    public PyrosStats pyrosStats { get; set; } = new PyrosStats();
    public PyrosStats pyrosSessionStats { get; set; } = new PyrosStats();
    public HydatosStats hydatosStats { get; set; } = new HydatosStats();
    public HydatosStats hydatosSessionStats { get; set; } = new HydatosStats();
    public Stats sessionStats { get; set; } = new Stats();
    public bool hasUpdatedStats = false;

    // Update Stats
    public void UpdateStats(Action<Stats> updateAction)
    {
        updateAction(stats);
        updateAction(sessionStats);
    }

    public void UpdatePagosStats(Action<PagosStats> updatePagosAction)
    {
        updatePagosAction(pagosStats);
        updatePagosAction(pagosSessionStats);
    }

    public void UpdatePyrosStats(Action<PyrosStats> updatePyrosAction)
    {
        updatePyrosAction(pyrosStats);
        updatePyrosAction(pyrosSessionStats);
    }

    public void UpdateHydatosStats(Action<HydatosStats> updateHydatosAction)
    {
        updateHydatosAction(hydatosStats);
        updateHydatosAction(hydatosSessionStats);
    }
    // Debug bool
    public bool enableDebug { get; set; } = false;
    // Save
    public void Save()
    {
        EzConfig.Save();
    }
}
