using ECommons.Configuration;
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
    public Stats sessionStats { get; set; } = new Stats();
    public bool hasUpdatedStats = false;
    public void UpdateStats(Action<Stats> updateAction)
    {
        updateAction(stats);
        updateAction(sessionStats);
    }

    // Save
    public void Save()
    {
        EzConfig.Save();
    }
}
