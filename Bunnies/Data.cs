using System.Numerics;

namespace FirstPlugin;

public static class Data
{
    // Main City
    public static uint Kugane = 628;
    public static uint KuganeAether = 111;
    public static uint KuganeInn = 629;
    public static Vector3 KuganeEurekaNpc = new(-133.165f, -5f, 149.749f);
    public static uint KuganeNpcObjectID = 1024517;
    public static Vector3 AethernetPier1 = new(-113.717f, -5.005f, 155.301f);
    public static Vector3 AethernetKogane = new(26.744f, 4f, 73.275f);
    public static Vector3 SummoningBell = new(19.51f, 4.05f, 53.133f);
    public static Vector3 KuganeAetherPosition = new(47.501f, 8.438f, -37.308f);
    public const string RSR = "https://raw.githubusercontent.com/FFXIV-CombatReborn/CombatRebornRepo/main/pluginmaster.json";
    public const string BMR = "https://raw.githubusercontent.com/FFXIV-CombatReborn/CombatRebornRepo/main/pluginmaster.json";
    public static class AethernetData
    {
        public static (float distance, Vector3 position, string description, float tolerance)[] Distances =>
        new[]
        {
                (GetDistanceToPlayer(KuganeAetherPosition), KuganeAetherPosition, "Going to Kugane Aetheryte", 9f),
                (GetDistanceToPlayer(AethernetPier1), AethernetPier1, "Going to Pier 1 Aethernet", 3f),
                (GetDistanceToPlayer(AethernetKogane), AethernetKogane, "Going to Kogane Aethernet", 3f)
        };
    }
    // Eureka
    public static uint Pagos = 763;
    public static Vector3 PagosRepairNpc = new(0f, 0f, 0f);
    public static uint Pyros = 795;
    public static Vector3 PyrosRepairNpc = new(-268.44547f, 680.81744f, 118.183235f);
    public static uint Hydatos = 827;
    public static Vector3 HydatrosRepairNpc = new(0f, 0f, 0f);
    // Status Bunny ID
    public static uint BunnyStatusID = 1531;
    public static uint CarrotKeyItem = 2002482;

    public static string AltBossMod = "BossModReborn";
    
    ///*
    public static readonly List<string> PyrosTargetTable = new List<string>
    {
        "Lost Snipper",
        "Lost Big Claw",
        "Karlabos of Pyros"
    };
    //*/
    /*
    public static readonly List<string> PyrosTargetTable = new List<string>
    {
        "Pyros Serpent",
        "Not Real",
        "Gibbon"
    };
    //*/

    public static Vector3 PyrosCenterFatePoint = new(124.108f, 707.305f, 256.507f);
    public static Vector3 PyrosRightFatePoint = new(131.498f, 706.524f, 216.308f);
    public static Vector3 PyrosLeftFatePoint = new(165.04338f, 710.722f, 237.734f);

    public static Vector3 cliff = new Vector3(-105.77034f, 757.8501f, 372.0822f);
    public static Vector3 cliff2 = new Vector3(-105.64111f, 757.7301f, 359.7527f);


    public static readonly List<string> CofferTable = new List<string>
    {
        "Bronze Coffer",
        "Silver Coffer",
        "Gold Coffer"
    };

 #region DIRECTION

    public class DirectionOptions
    {
        public static readonly Dictionary<string, string> AllOptions = new Dictionary<string, string>
        {
            { "CloseNorth", "You sense something to the north." },
            { "FarNorth", "You sense something far to the north." },
            { "FarFarNorth", "You sense something far, far to the north." },
            { "CloseNorthEast", "You sense something to the northeast." },
            { "FarNorthEast", "You sense something far to the northeast." },
            { "FarFarNorthEast", "You sense something far, far to the northeast." },
            { "CloseEast", "You sense something to the east." },
            { "FarEast", "You sense something far to the east." },
            { "FarFarEast", "You sense something far, far to the east." },
            { "CloseSouthEast", "You sense something to the southeast." },
            { "FarSouthEast", "You sense something far to the southeast." },
            { "FarFarSouthEast", "You sense something far, far to the southeast." },
            { "CloseSouth", "You sense something to the south." },
            { "FarSouth", "You sense something far to the south." },
            { "FarFarSouth", "You sense something far, far to the south." },
            { "CloseSouthWest", "You sense something to the southwest." },
            { "FarSouthWest", "You sense something far to the southwest." },
            { "FarFarSouthWest", "You sense something far, far to the southwest." },
            { "CloseWest", "You sense something to the west." },
            { "FarWest", "You sense something far to the west." },
            { "FarFarWest", "You sense something far, far to the west." },
            { "CloseNorthWest", "You sense something to the northwest." },
            { "FarNorthWest", "You sense something far to the northwest." },
            { "FarFarNorthWest", "You sense something far, far to the northwest." }
        };
        #region IGNORE
        /*
        public static readonly Dictionary<string, string> NorthOptions = new Dictionary<string, string>
        {
            { "CloseNorth", "You sense something to the north." },
            { "FarNorth", "You sense something far to the north." },
            { "FarFarNorth", "You sense something far, far to the north." }
        };
        public static readonly Dictionary<string, string> NorthEastOptions = new Dictionary<string, string>
        {
            { "CloseNorthEast", "You sense something to the northeast." },
            { "FarNorthEast", "You sense something far to the northeast." },
            { "FarFarNorthEast", "You sense something far, far to the northeast." }
        };
        public static readonly Dictionary<string, string> EastOptions = new Dictionary<string, string>
        {
            { "CloseEast", "You sense something to the east." },
            { "FarEast", "You sense something far to the east." },
            { "FarFarEast", "You sense something far, far to the east." }
        };
        public static readonly Dictionary<string, string> SouthEastOptions = new Dictionary<string, string>
        {
            { "CloseSouthEast", "You sense something to the southeast." },
            { "FarSouthEast", "You sense something far to the southeast." },
            { "FarFarSouthEast", "You sense something far, far to the southeast." }
        };
        public static readonly Dictionary<string, string> SouthOptions = new Dictionary<string, string>
        {
            { "CloseSouth", "You sense something to the south." },
            { "FarSouth", "You sense something far to the south." },
            { "FarFarSouth", "You sense something far, far to the south." }
        };
        public static readonly Dictionary<string, string> SouthWestOptions = new Dictionary<string, string>
        {
            { "CloseSouthWest", "You sense something to the southwest." },
            { "FarSouthWest", "You sense something far to the southwest." },
            { "FarFarSouthWest", "You sense something far, far to the southwest." }
        };
        public static readonly Dictionary<string, string> WestOptions = new Dictionary<string, string>
        {
            { "CloseWest", "You sense something to the west." },
            { "FarWest", "You sense something far to the west." },
            { "FarFarWest", "You sense something far, far to the west." }
        };
        public static readonly Dictionary<string, string> NorthWestOptions = new Dictionary<string, string>
        {
            { "CloseNorthWest", "You sense something to the northwest." },
            { "FarNorthWest", "You sense something far to the northwest." },
            { "FarFarNorthWest", "You sense something far, far to the northwest." }
        }; 
        */
        #endregion IGNORE
    }

    #endregion DIRECTION

    #region PYROSSEARCH

    public class InitialSearch
    {
        // These are the initial messages when using the carrot after completing the fate//
        public static readonly Dictionary<string, string> IntialSearchOptions = new Dictionary<string, string>
        {
            { "InitialEast", DirectionOptions.AllOptions["FarFarEast"] },
            { "InitialSouthEast", DirectionOptions.AllOptions["FarFarSouthEast"] },
            { "InitialWest", DirectionOptions.AllOptions["FarFarWest"] },
            { "InitialSouthWest", DirectionOptions.AllOptions["FarFarSouthWest"] },
            { "InitialSouth", DirectionOptions.AllOptions["FarFarSouth"] },
            { "InitialNorthEast", DirectionOptions.AllOptions["FarNorthEast"] }
        };
    }

    public class East
    {
        public static readonly string direction = "East";

        public static readonly List<Vector3> FarFarEastCoffers = new List<Vector3>
        {
            new Vector3(378.1864f, 724.90393f, 287.15256f),
            new Vector3(460.48206f, 723.12067f, 310.9085f)
        };

        // function for these locations
    }

    public class SouthEast
    {
        public static readonly string direction = "SouthEast";

        // First SouthEast CheckPoint
        public static readonly Vector3 CheckPoint1 = new Vector3(281.05173f, 736.18256f, 483.5108f);

        // Search options after reaching this point
        public static readonly Dictionary<string, string> CheckPoint1Search = new Dictionary<string, string>
        {
            { "CloseSouthCP1", DirectionOptions.AllOptions["CloseSouth"] },
            { "FarSouthEastCP1", DirectionOptions.AllOptions["FarSouthEast"]},
            { "CloseEastCP1", DirectionOptions.AllOptions["CloseEast"] },
            { "FarEastCP1", DirectionOptions.AllOptions["FarEast"] }
        };

        // Coffer locations south of CP1
        public class CloseSouthCP1
        {
            public static readonly string direction = "CloseSouth";
            public static readonly List<Vector3> CloseSouthCP1Coffers = new List<Vector3>
            {
                new Vector3(292.738f, 739.302f, 531.109f),
                new Vector3(309.592f, 741.897f, 565.690f)
            };
        }

        public class FarSouthEastCP1
        {
            public static readonly string direction = "FarSouthEast";
            public static readonly List<Vector3> FarSouthEastCP1Coffers = new List<Vector3>
            {
                new Vector3(368.032f, 744.048f, 639.172f),
                new Vector3(433.128f, 731.954f, 568.584f)
            };
        }

        public class CloseEastCP1
        {
            public static readonly string direction = "CloseEastEast";
            public static readonly List<Vector3> CloseEastCP1Coffers = new List<Vector3>
            {
                new Vector3(371.1314f, 737.9204f, 491.637f)
            };
        }

        public class RemainingCoffersCP1
        {
            public static readonly string direction = "Remaining";
            public static readonly List<Vector3> RemainingCP1Coffers = new List<Vector3>
            {
                new Vector3 (448.89133f, 725.0767f, 457.0396f),
                new Vector3 (469.44803f, 726.332f, 535.43036f)
            };
        }

        // function for these locations
    }

    public class South
    {
        public static readonly string direction = "South";
        // First South CheckPoint
        public static readonly Vector3 CheckPoint1 = new Vector3(151.63268f, 749.5586f, 612.72266f);

        // Search options after reaching this point
        public static readonly Dictionary<string, string> CheckPoint1Search = new Dictionary<string, string>
        {
            { "CloseEastCP1" , DirectionOptions.AllOptions["CloseEast"] },
            { "FarSouthEastCP1", DirectionOptions.AllOptions["FarSouthEast"] },
            { "FarSouthWestCP1", DirectionOptions.AllOptions["FarSouthWest"] },
            { "CloseSouthCP1", DirectionOptions.AllOptions["CloseSouth"] },
            { "FarSouthCP1", DirectionOptions.AllOptions["FarSouth"] },
            { "FarFarSouthCP1", DirectionOptions.AllOptions["FarFarSouth"] }
        };

        // Coffer locations east of CP1
        public class CloseEastCP1
        {
            public static readonly string direction = "CloseEast";
            public static readonly List<Vector3> CloseEastCP1Coffers = new List<Vector3>
            {
                new Vector3(184.47034f, 747.59955f, 617.30145f)
            };
        }

        public class FarSouthEastCP1
        {
            public static readonly string direction = "FarSouthEast";
            public static readonly List<Vector3> FarSouthCP1Coffers = new List<Vector3>
            {
                new Vector3(280.455f, 746.507f, 754.291f)
            };
        }

        public class FarSouthWestCP1
        {
            public static readonly string direction = "FarSouthWest";
            public static readonly List<Vector3> FarSouthWestCP1Coffers = new List<Vector3>
            {
                new Vector3(32.138092f, 754.25977f, 690.0067f)
            };
        }

        public class CloseSouthCP1
        {
            public static readonly string direction = "CloseSouth";
            public static readonly List<Vector3> CloseSouthCP1Coffers = new List<Vector3>
            {
                new Vector3(156.87155f, 751.11053f, 704.0944f)
            };
        }

        public class FarSouthCP1
        {
            public static readonly string direction = "FarSouth";
            public static readonly List<Vector3> FarSouthCP1Coffers = new List<Vector3>
            {
                new Vector3(146.42485f, 752.44586f, 755.91846f)
            };
        }

        public class RemainingCP1
        {
            public static readonly string direction = "Remaining";
            public static readonly List<Vector3> RemainingCP1Coffers = new List<Vector3>
            {
                new Vector3(157.19025f, 754.6542f, 841.3428f),
                new Vector3(92.84613f, 754.26074f, 825.04004f)
            };
        }

        // function for these locations
    }

    public class SouthWest
    {
        public static readonly string direction = "SouthWest";

        // First SouthWest CheckPoint
        public static readonly Vector3 CheckPoint1 = new Vector3(36.167213f, 774.47473f, 542.33673f);

        // Search options after reaching this point
        public static readonly Dictionary<string, string> CheckPoint1Search = new Dictionary<string, string>
        {
            { "FarNorthCP1", DirectionOptions.AllOptions["FarNorth"] },
            { "FarFarNorthCP1", DirectionOptions.AllOptions["FarFarNorth"] },
            { "CloseNorthWestCP1", DirectionOptions.AllOptions["CloseNorthWest"] },
            { "FarFarWestCP1", DirectionOptions.AllOptions["FarFarWest"] },
            { "CloseSouthWestCP1", DirectionOptions.AllOptions["CloseSouthWest"] },
            { "FarFarSouthWestCP1", DirectionOptions.AllOptions["FarFarSouthWest"] },
            { "FarFarNorthWestCP1", DirectionOptions.AllOptions["FarFarNorthWest"] }
        };

        // Coffer locations far north of CP1
        public class FarNorthCP1
        {
            public static readonly string direction = "FarNorth";
            public static readonly List<Vector3> FarNorthCP1Coffers = new List<Vector3>
            {
                new Vector3(2.400449f, 764.1738f, 411.12482f)
            };
        }

        public class FarFarNorthCP1
        {
            public static readonly string direction = "FarFarNorth";
            public static readonly List<Vector3> FarFarNorthCP1Coffers = new List<Vector3>
            {
                new Vector3(-38.306225f, 675.3618f, 354.35876f)
            };
        }

        public class CloseNorthWestCP1
        {
            public static readonly string direction = "CloseNorthWest";
            public static readonly List<Vector3> CloseNorthWestCP1Coffers = new List<Vector3>
            {
                new Vector3(-38.86991f, 769.8192f, 504.81125f)
            };
        }

        public class FarFarWestCP1
        {
            public static readonly string direction = "FarFarWest";
            public static readonly List<Vector3> FarFarWestCP1Coffers = new List<Vector3>
            {
                new Vector3(-198.28018f, 758.00037f, 477.8307f),
                new Vector3(-197.58304f, 759.48474f, 599.07623f)
            };
        }

        public class CloseSouthWestCP1
        {
            public static readonly string direction = "CloseSouthWest";
            public static readonly List<Vector3> CloseSouthWestCP1Coffers = new List<Vector3>
            {
                new Vector3(-11.707698f, 773.2758f, 601.38275f)
            };
        }

        public class FarFarSouthWestCP1
        {
            public static readonly string direction = "FarFarSouthWest";
            public static readonly List<Vector3> FarFarSouthWestCP1Coffers = new List<Vector3>
            {
                new Vector3(-105.74974f, 762.6743f, 686.0651f)
            };
        }

        public class RemainingCP1
        {
            public static readonly string direction = "Remaining";
            public static readonly List<Vector3> RemainingCP1Coffers = new List<Vector3>
            {
                new Vector3(-148.93141f, 760.50305f, 453.25677f)
            };
        }

        //function for these locations
    }

    public class West
    {
        public static readonly string direction = "West";

        // West CheckPoint
        public static readonly Vector3 CheckPoint1 = new Vector3(-162.67996f, 675.3378f, 233.13602f);
        public static readonly Vector3 CheckPoint2 = new Vector3(-350.61374f, 661.8533f, 331.88434f);

        // Search options after reaching first checkpoint
        public static readonly Dictionary<string, string> CheckPoint1Search = new Dictionary<string, string>
        {
            { "CloseSouthCP1", DirectionOptions.AllOptions["CloseSouth"] },
            { "FarFarSouthWestCP1", DirectionOptions.AllOptions["FarFarSouthWest"] }
        };

        public class CloseSouthCP1
        {
            public static readonly string direction = "CloseSouth";
            public static readonly List<Vector3> CloseSouthCP1Coffers = new List<Vector3>
            {
                new Vector3(-189.66103f, 671.67864f, 323.3722f)
            };
        }

        public static readonly Dictionary<string, string> CheckPoint2Search = new Dictionary<string, string>
        {
            { "CloseSouthCP2", DirectionOptions.AllOptions["CloseSouth"] },
            { "CloseSouthWestCP1", DirectionOptions.AllOptions["CloseSouthWest"] },
            { "FarSouthWestCP1", DirectionOptions.AllOptions["FarSouthWest"] }
        };

        public class CloseSouthCP2
        {
            public static readonly string direction = "CloseSouth";
            public static readonly List<Vector3> CloseSouthCP2Coffers = new List<Vector3>
            {
                new Vector3(-340.14093f, 660.3162f, 384.865f)
            };
        }

        public class RemainingCP2
        {
            public static readonly string direction = "Remaining";
            public static readonly List<Vector3> RemainingCP2Coffers = new List<Vector3>
            {
                new Vector3(-438.5698f, 660.76324f, 400.34637f),
                new Vector3(-464.59262f, 660.64594f, 419.0569f),
                new Vector3(-467.78552f, 659.18353f, 441.77567f)
            };
        }

        // function for these location
    }

    public class NorthEast
    {
        public static readonly string direction = "NorthEast";

        public static readonly List<Vector3> CloseNorthEastCoffers = new List<Vector3>
        {
            new Vector3(249.037f, 723.121f, 118.259f)
        };
    }


#endregion PYROSSEARCH
}
