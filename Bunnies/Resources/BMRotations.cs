
namespace FirstPlugin.Resources
{
    internal static class BMRotations
    {
        public static string rootPassive = @"
        {
            ""Name"": ""RoR Passive"",
            ""Modules"": 
            {
                ""BossMod.Autorotation.MiscAI.StayCloseToTarget"": []
            }
        }";

        public static string rootBoss = @"
        {
          ""Name"": ""RoR Boss"",
          ""Modules"": {
            ""BossMod.Autorotation.xan.BLM"": [
              {
                ""Track"": ""Targeting"",
                ""Option"": ""Auto""
              },
              {
                ""Track"": ""Buffs"",
                ""Option"": ""Auto""
              }
            ],
            ""BossMod.Autorotation.xan.PCT"": [
              {
                ""Track"": ""Targeting"",
                ""Option"": ""Auto""
              },
              {
                ""Track"": ""Buffs"",
                ""Option"": ""Auto""
              }
            ],
            ""BossMod.Autorotation.xan.RDM"": [
              {
                ""Track"": ""Targeting"",
                ""Option"": ""Auto""
              },
              {
                ""Track"": ""Buffs"",
                ""Option"": ""Auto""
              }
            ],
            ""BossMod.Autorotation.xan.SMN"": [
              {
                ""Track"": ""Targeting"",
                ""Option"": ""Auto""
              },
              {
                ""Track"": ""Buffs"",
                ""Option"": ""Auto""
              }
            ],
            ""BossMod.Autorotation.xan.AST"": [
              {
                ""Track"": ""Targeting"",
                ""Option"": ""Auto""
              },
              {
                ""Track"": ""Buffs"",
                ""Option"": ""Auto""
              }
            ],
            ""BossMod.Autorotation.xan.SCH"": [
              {
                ""Track"": ""Targeting"",
                ""Option"": ""Auto""
              },
              {
                ""Track"": ""Buffs"",
                ""Option"": ""Auto""
              }
            ],
            ""BossMod.Autorotation.xan.SGE"": [
              {
                ""Track"": ""Targeting"",
                ""Option"": ""Auto""
              },
              {
                ""Track"": ""Buffs"",
                ""Option"": ""Auto""
              }
            ],
            ""BossMod.Autorotation.xan.WHM"": [
              {
                ""Track"": ""Targeting"",
                ""Option"": ""Auto""
              },
              {
                ""Track"": ""Buffs"",
                ""Option"": ""Auto""
              }
            ],
            ""BossMod.Autorotation.xan.DRG"": [
              {
                ""Track"": ""Targeting"",
                ""Option"": ""Auto""
              },
              {
                ""Track"": ""Buffs"",
                ""Option"": ""Auto""
              }
            ],
            ""BossMod.Autorotation.xan.MNK"": [
              {
                ""Track"": ""Targeting"",
                ""Option"": ""Auto""
              },
              {
                ""Track"": ""SixSidedStar"",
                ""Option"": ""Force""
              },
              {
                ""Track"": ""Meditate"",
                ""Option"": ""Use out of combat, during countdown, or if no enemies are targetable""
              },
              {
                ""Track"": ""FormShift"",
                ""Option"": ""Auto""
              },
              {
                ""Track"": ""FiresReply"",
                ""Option"": ""Use when out of melee range, or if about to expire""
              },
              {
                ""Track"": ""Nadi"",
                ""Option"": ""Lunar""
              },
              {
                ""Track"": ""RoF"",
                ""Option"": ""Force""
              },
              {
                ""Track"": ""RoW"",
                ""Option"": ""Force""
              },
              {
                ""Track"": ""PB"",
                ""Option"": ""Use ASAP after next Opo""
              },
              {
                ""Track"": ""BH"",
                ""Option"": ""Force""
              },
              {
                ""Track"": ""TC"",
                ""Option"": ""Use if outside melee range""
              },
              {
                ""Track"": ""Blitz"",
                ""Option"": ""Hold blitz until Riddle of Fire is active""
              },
              {
                ""Track"": ""Engage"",
                ""Option"": ""Sprint to melee range""
              }
            ],
            ""BossMod.Autorotation.xan.NIN"": [
              {
                ""Track"": ""Targeting"",
                ""Option"": ""Auto""
              },
              {
                ""Track"": ""Buffs"",
                ""Option"": ""Auto""
              }
            ],
            ""BossMod.Autorotation.xan.RPR"": [
              {
                ""Track"": ""Targeting"",
                ""Option"": ""Auto""
              },
              {
                ""Track"": ""Buffs"",
                ""Option"": ""Auto""
              }
            ],
            ""BossMod.Autorotation.xan.SAM"": [
              {
                ""Track"": ""Targeting"",
                ""Option"": ""Auto""
              },
              {
                ""Track"": ""Buffs"",
                ""Option"": ""Auto""
              }
            ],
            ""BossMod.Autorotation.xan.VPR"": [
              {
                ""Track"": ""Targeting"",
                ""Option"": ""Auto""
              },
              {
                ""Track"": ""Buffs"",
                ""Option"": ""Auto""
              }
            ],
            ""BossMod.Autorotation.xan.BRD"": [
              {
                ""Track"": ""Targeting"",
                ""Option"": ""Auto""
              },
              {
                ""Track"": ""Buffs"",
                ""Option"": ""Auto""
              }
            ],
            ""BossMod.Autorotation.xan.DNC"": [
              {
                ""Track"": ""Targeting"",
                ""Option"": ""Auto""
              },
              {
                ""Track"": ""Buffs"",
                ""Option"": ""Auto""
              }
            ],
            ""BossMod.Autorotation.xan.MCH"": [
              {
                ""Track"": ""Targeting"",
                ""Option"": ""Auto""
              },
              {
                ""Track"": ""Buffs"",
                ""Option"": ""Auto""
              }
            ],
            ""BossMod.Autorotation.xan.DRK"": [
              {
                ""Track"": ""Targeting"",
                ""Option"": ""Auto""
              },
              {
                ""Track"": ""Buffs"",
                ""Option"": ""Auto""
              }
            ],
            ""BossMod.Autorotation.xan.GNB"": [
              {
                ""Track"": ""Targeting"",
                ""Option"": ""Auto""
              },
              {
                ""Track"": ""Buffs"",
                ""Option"": ""Auto""
              }
            ],
            ""BossMod.Autorotation.xan.PLD"": [
              {
                ""Track"": ""Targeting"",
                ""Option"": ""Auto""
              },
              {
                ""Track"": ""Buffs"",
                ""Option"": ""Auto""
              }
            ],
            ""BossMod.Autorotation.StandardWAR"": [
              {
                ""Track"": ""AOE"",
                ""Option"": ""AutoFinishCombo""
              },
              {
                ""Track"": ""Burst"",
                ""Option"": ""Spend""
              },
              {
                ""Track"": ""Potion"",
                ""Option"": ""Manual""
              },
              {
                ""Track"": ""Infuriate"",
                ""Option"": ""ForceIfNoNC""
              },
              {
                ""Track"": ""IR"",
                ""Option"": ""Automatic""
              },
              {
                ""Track"": ""Upheaval"",
                ""Option"": ""Automatic""
              },
              {
                ""Track"": ""PR"",
                ""Option"": ""Automatic""
              },
              {
                ""Track"": ""Onslaught"",
                ""Option"": ""Force""
              },
              {
                ""Track"": ""Tomahawk"",
                ""Option"": ""Opener""
              },
              {
                ""Track"": ""Wrath"",
                ""Option"": ""Automatic""
              }
            ],
            ""BossMod.Autorotation.MiscAI.StayCloseToTarget"": [],
            ""BossMod.Autorotation.xan.HealerAI"": [
              {
                ""Track"": ""Raise"",
                ""Option"": ""Raise using Swiftcast only""
              },
              {
                ""Track"": ""RaiseTargets"",
                ""Option"": ""Alliance raid members""
              },
              {
                ""Track"": ""Heal"",
                ""Option"": ""Enabled""
              },
              {
                ""Track"": ""Esuna"",
                ""Option"": ""Enabled""
              }
            ],
            ""BossMod.Autorotation.xan.MeleeAI"": [
              {
                ""Track"": ""Second Wind"",
                ""Option"": ""Enabled""
              },
              {
                ""Track"": ""Bloodbath"",
                ""Option"": ""Enabled""
              },
              {
                ""Track"": ""Stun"",
                ""Option"": ""Enabled""
              }
            ],
            ""BossMod.Autorotation.xan.RangedAI"": [
              {
                ""Track"": ""Head Graze"",
                ""Option"": ""Enabled""
              },
              {
                ""Track"": ""Second Wind"",
                ""Option"": ""Enabled""
              }
            ],
            ""BossMod.Autorotation.xan.TankAI"": [
              {
                ""Track"": ""Stance"",
                ""Option"": ""Enabled""
              },
              {
                ""Track"": ""Ranged GCD"",
                ""Option"": ""Enabled""
              },
              {
                ""Track"": ""Interject"",
                ""Option"": ""Enabled""
              },
              {
                ""Track"": ""Low Blow"",
                ""Option"": ""Enabled""
              },
              {
                ""Track"": ""Arms\u0027 Length"",
                ""Option"": ""Enabled""
              },
              {
                ""Track"": ""Personal mits"",
                ""Option"": ""Enabled""
              },
              {
                ""Track"": ""Invuln"",
                ""Option"": ""Enabled""
              },
              {
                ""Track"": ""Protect party members"",
                ""Option"": ""Enabled""
              }
            ]
          }
        }";

    }
}
