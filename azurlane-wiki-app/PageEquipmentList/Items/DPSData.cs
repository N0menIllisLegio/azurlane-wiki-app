using azurlane_wiki_app.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace azurlane_wiki_app.PageEquipmentList.Items
{
    #region tables

    public class ArmourModifier
	{
		[Key]
		[Column(Order = 1)]
		public string WeaponName { get; set; }

		[Key]
		[Column(Order = 2)]
		public string AmmoType { get; set; }

		public double Light { get; set; }
		public double Medium { get; set; }
		public double Heavy { get; set; }
		public double ShellSpeed { get; set; }
	}

	[Table("BombsStats")]
	public class BombStats
	{
		[Key]
		[Column(Order = 1)]
		public string BombName { get; set; }

		[Key]
		[Column(Order = 2)]
		public string BombTech { get; set; }


		public int Dmg { get; set; }
		public int Dmg3 { get; set; }
		public int Dmg6 { get; set; }
		public int DmgMax { get; set; }
	}

	public class Bomb_Stats
	{
		public Dictionary<string, BombStats> Damages;

		public double ArmourModifierLight;
		public double ArmourModifierMedium;
		public double ArmourModifierHeavy;

		public int SplashRange;
	}

	[Table("GunsStats")]
	public class GunStats
	{
		[Key]
		[Column(Order = 1)]
		public string GunName { get; set; }

		[Key]
		[Column(Order = 2)]
		public string GunTech { get; set; }

		public double Rld { get; set; }
		public double Rld3 { get; set; }
		public double Rld6 { get; set; }
		public double RldMax { get; set; }

		public int Dmg { get; set; }
		public int Dmg3 { get; set; }
		public int Dmg6 { get; set; }
		public int DmgMax { get; set; }
	}

	#endregion

	public class DPSData
	{
		public readonly double Cooldown;

		private List<ArmourModifier> ArmourModifiers;
		private List<BombStats> PlanesBombs;
		private List<GunStats> PlanesGuns;

		public DPSData(string equipmentName)
        {
			using (CargoContext cargoContext = new CargoContext())
			{
				float? cooldown = 0;

				switch (equipmentName)
				{
					case "Destroyer Guns":
						ArmourModifiers = cargoContext.ArmourModifiers.Where(am => am.WeaponName == "DD Gun").ToList();
						cooldown = cargoContext.EquipmentTypes.Find("DD Gun")?.Cooldown;
						break;
					case "Light Cruiser Guns":
						ArmourModifiers = cargoContext.ArmourModifiers.Where(am => am.WeaponName == "CL Gun").ToList();
						cooldown = cargoContext.EquipmentTypes.Find("CL Gun")?.Cooldown;
						break;
					case "Battleship Guns":
						ArmourModifiers = cargoContext.ArmourModifiers.Where(am => am.WeaponName == "BB Gun").ToList();
						cooldown = cargoContext.EquipmentTypes.Find("BB Gun")?.Cooldown;
						break;
					case "Large Cruiser Guns":
					case "Heavy Cruiser Guns":
						ArmourModifiers = cargoContext.ArmourModifiers.Where(am => am.WeaponName == "CA Gun").ToList();
						cooldown = cargoContext.EquipmentTypes.Find("CA Gun")?.Cooldown;
						break;

					case "Submarine Torpedoes":
					case "Ship Torpedoes":
						ArmourModifiers = cargoContext.ArmourModifiers.Where(am => am.WeaponName == "Torpedo").ToList();
						break;
					case "Torpedo Bomber Planes":
						PlanesBombs = cargoContext.BombsStats.ToList();
						PlanesGuns = cargoContext.GunsStats.ToList();
						ArmourModifiers = cargoContext.ArmourModifiers.Where(am => am.WeaponName == "Torpedo Bomber").ToList();
						cooldown = cargoContext.EquipmentTypes.Find("Torpedo Bomber")?.Cooldown;
						break;
					case "Fighter Planes":
					case "Dive Bomber Planes":
					case "Seaplanes":
						ArmourModifiers = cargoContext.ArmourModifiers.Where(am => am.WeaponName == "Bomb").ToList();
						PlanesBombs = cargoContext.BombsStats.ToList();
						PlanesGuns = cargoContext.GunsStats.ToList();
						cooldown = cargoContext.EquipmentTypes.Find("Fighter")?.Cooldown;
						break;

					case "Anti-Air Guns":
					case "Anti-Submarine Equipment":
					case "Auxiliary Equipment":
						cooldown = cargoContext.EquipmentTypes.Find("AA Gun")?.Cooldown;
						break;
					default:
						Logger.Write($"Cannot distinguish equipmentName: {equipmentName}", this.GetType().ToString());
						// Load All?
						break;
				}

				Cooldown = cooldown ?? 0;
			}
		}

        // GUNS

        public ArmourModifier GetGunArmourModifier(string ammoType)
        {
			return ArmourModifiers.Find(am => am.AmmoType == ammoType);
			// return GunsAMs.ContainsKey(ammoType) ? GunsAMs[ammoType] : null;
		}
		
		// TORPEDOES | SUBMARINE TORPEDOES

		public ArmourModifier GetTorpedoArmourModifier(string torpedoType)
		{
			ArmourModifier armourModifier = ArmourModifiers.Find(am => am.AmmoType == torpedoType);

            if (armourModifier == null)
            {
				armourModifier = ArmourModifiers.Find(am => am.AmmoType == "All");
			}

			return armourModifier;
		}

		// PLANES

		public (ArmourModifier armourModifier, BombStats bombStats) GetPlaneBombStats(string bombName, string bombTech)
        {
			//Change
			var result = (armourModifier: (ArmourModifier) null, bombStats: (BombStats) null);

			result.armourModifier = ArmourModifiers.Find(am => bombName.Contains(am.AmmoType));
			result.bombStats = PlanesBombs.Find(bomb => bombName.Contains(bomb.BombName) && bomb.BombTech == bombTech);

			return result;
		}

		public GunStats GetPlaneGunStats(string gunType, string Tech)
        {
			GunStats gunStats = PlanesGuns.Find(gun => gun.GunName == gunType && gun.GunTech == Tech);

            if (gunStats == null)
            {
				gunStats = PlanesGuns.Find(gun => gunType.Contains(gun.GunName) && gun.GunTech == Tech);
            }

			return gunStats;
		}
	}
}

// CA Gun

//Dictionary<string, double> BurnChance = new Dictionary<string, double>
//{
//    { "HE",  .08},
//    { "HE*", .09},
//    { "HE+", .04},
//};

//// CB Gun

//Dictionary<string, double> BurnChance = new Dictionary<string, double>
//{
//    { "HE",  .08},
//    { "HE*", .09},
//    { "HE+", .04},
//};

//// BB Gun

//Dictionary<string, double> BurnChance = new Dictionary<string, double>
//{
//    { "AP",  .2},
//    { "AP*", .25},
//    { "AP^", .25},
//    { "AP+", .2},
//};

//Dictionary<string, int> SplashRange = new Dictionary<string, int>
//{
//    { "Normal",                     15},
//    { "HE",                         15},
//    { "AP",                         15},
//    { "AP*",                        15},
//    { "AP^",                        15},
//    { "AP+",                        16},
//    { "Type 3 Shell",               0}, // value at wiki = '-'
//    { "Type 3 Shell Fragments",     0}, // value at wiki = '-'
//    { "Super-Heavy Shell",          8},
//};

//public Dictionary<string, int> TorpedoSpeed = new Dictionary<string, int>
//{
//	  { "Normal", 30 },
//	  { "Magnetic", 20 },
//	  { "Oxygen (Sakura)", 40 },
//	  { "Oxygen (Type 95)", 30 } // + accelerating
//};