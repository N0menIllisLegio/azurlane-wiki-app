using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using azurlane_wiki_app.Data.Downloaders.ServerModels;
using Newtonsoft.Json;

namespace azurlane_wiki_app.Data.Tables
{
    public class ShipGirl
    {
        private readonly Dictionary<string, string> Abbreviations = new Dictionary<string, string>
        {
            ["Destroyer"] = "DD",
            ["Light Cruiser"] = "CL",
            ["Heavy Cruiser"] = "CA",
            ["Aircraft Carrier"] = "CV",
            ["Light Aircraft Carrier"] = "CVL",
            ["Battleship"] = "BB",
            ["Submarine"] = "SS",
            ["Large Cruiser"] = "CB",
            ["Repair Ship"] = "AR",
            ["Battlecruiser"] = "BC",
            ["Monitor"] = "BM",
            ["Submarine Carrier"] = "SSV",
            ["Aviation Battleship"] = "BBV",
            ["Munition Ship"] = "AE"
        };

        /// <summary>
        /// Coin, Oil, Medal
        /// </summary>
        private readonly Dictionary<string, (int?, int?, int?)> ScrapValues = new Dictionary<string, (int?, int?, int?)>
        {
            ["Ultra Rare"] = (4, 3, 30),
            ["Super Rare"] = (4, 3, 10),
            ["Elite"] = (4, 3, 4),
            ["Rare"] = (4, 3, 1),
            ["Normal"] = (4, 3, 0),
            ["Priority"] = (null, null, null),
            ["Decisive"] = (null, null, null),
            ["Unreleased"] = (null, null, null)
        };

        private string Refactor(string text)
        {
            string refactoredText = text.Replace(@"&amp;#39;", "\'");

            return refactoredText;
        }

        public ShipGirl(RequestShipGirlModel shipGirlModel, CargoContext cargoContext)
        {
            WhereToGetShipGirl = new List<ShipGirlWhereToGetShipGirl>();
            Skills = new List<Skill>();

            ShipID = shipGirlModel.ShipID;
            Name = Refactor(shipGirlModel.Name);
            ConstructTime = shipGirlModel.ConstructTime;
            Remodel = shipGirlModel.Remodel;
            Armor = shipGirlModel.Armor;
            Speed = shipGirlModel.Speed;
            Luck = shipGirlModel.Luck;
            LimitBreak1 = shipGirlModel.LB1;
            LimitBreak2 = shipGirlModel.LB2;
            LimitBreak3 = shipGirlModel.LB3;

            var initStats = shipGirlModel.GetStats(Stats.Initial);
            var level100Stats = shipGirlModel.GetStats(Stats.Level100);
            var level120Stats = shipGirlModel.GetStats(Stats.Level120);

            cargoContext.ShipGirlsStats.Add(initStats);
            cargoContext.ShipGirlsStats.Add(level100Stats);
            cargoContext.ShipGirlsStats.Add(level120Stats);

            var standardImages = shipGirlModel.GetImages(Images.Standard);
            cargoContext.ShipGirlsImages.Add(standardImages);

            if (shipGirlModel.Remodel == "t")
            {
                var initRetrofitedStats = shipGirlModel.GetStats(Stats.RetrofitedInitial);
                var level120RetrofitedStats = shipGirlModel.GetStats(Stats.Retrofited120);
                var retrofitedImages = shipGirlModel.GetImages(Images.Retrofited);

                var retrofit = new ShipGirlRetrofit
                {
                    FK_Images = retrofitedImages,
                    FK_InitialStats = initRetrofitedStats,
                    FK_Level120Stats = level120RetrofitedStats
                };

                cargoContext.ShipGirlsStats.Add(initRetrofitedStats);
                cargoContext.ShipGirlsStats.Add(level120RetrofitedStats);
                cargoContext.ShipGirlsImages.Add(retrofitedImages);
                cargoContext.ShipGirlsRetrofits.Add(retrofit);

                FK_ShipGirlRetrofit = retrofit;
            }

            FK_Images = standardImages;
            FK_InitialStats = initStats;
            FK_Level100Stats = level100Stats;
            FK_Level120Stats = level120Stats;

            Rarity rarity = cargoContext.Rarities.Find(shipGirlModel.Rarity);
            Nationality nationality = cargoContext.Nationalities.Find(shipGirlModel.Nationality);
            ShipClass shipClass = cargoContext.ShipClasses.Find(shipGirlModel.Class);
            ShipGroup shipGroup = cargoContext.ShipGroups.Find(shipGirlModel.ShipGroup);
            ShipType shipType = cargoContext.ShipTypes.Find(shipGirlModel.Type);
            ShipType subtypeRetro = cargoContext.ShipTypes.Find(shipGirlModel.SubtypeRetro);
            EquipmentType eq1Type = cargoContext.EquipmentTypes.Find(shipGirlModel.Eq1Type);
            EquipmentType eq2Type = cargoContext.EquipmentTypes.Find(shipGirlModel.Eq2Type);
            EquipmentType eq3Type = cargoContext.EquipmentTypes.Find(shipGirlModel.Eq3Type);

            if (rarity == null)
            {
                (int? coins, int? oil, int? medals) = ScrapValues[shipGirlModel.Rarity];

                rarity = new Rarity
                {
                    Name = shipGirlModel.Rarity,
                    FK_Icon = cargoContext.Icons.Find(shipGirlModel.Rarity),
                    ScrapCoins = coins,
                    ScrapOil = oil,
                    ScrapMedals = medals
                };

                cargoContext.Rarities.Add(rarity);
            }

            if (nationality == null)
            {
                nationality = new Nationality
                {
                    Name = shipGirlModel.Nationality,
                    FK_Icon = cargoContext.Icons.Find(shipGirlModel.Nationality)
                };

                cargoContext.Nationalities.Add(nationality);
            }

            if (shipClass == null)
            {
                shipClass = new ShipClass { Name = shipGirlModel.Class };
                cargoContext.ShipClasses.Add(shipClass);
            }

            if (shipGroup == null)
            {
                shipGroup = new ShipGroup { Name = shipGirlModel.ShipGroup };
                cargoContext.ShipGroups.Add(shipGroup);
            }

            if (shipType == null)
            {
                shipType = new ShipType
                {
                    Name = shipGirlModel.Type,
                    Abbreviation = Abbreviations[shipGirlModel.Type],
                    FK_Icon = cargoContext.Icons.Find(Abbreviations[shipGirlModel.Type])
                };

                cargoContext.ShipTypes.Add(shipType);
            }

            if (subtypeRetro == null)
            {
                if (shipGirlModel.SubtypeRetro.Trim() == "")
                {
                    shipGirlModel.SubtypeRetro = null;
                }
                else
                {
                    subtypeRetro = new ShipType
                    {
                        Name = shipGirlModel.SubtypeRetro,
                        Abbreviation = Abbreviations[shipGirlModel.SubtypeRetro],
                        FK_Icon = cargoContext.Icons.Find(Abbreviations[shipGirlModel.SubtypeRetro])
                    };

                    cargoContext.ShipTypes.Add(subtypeRetro);
                }
            }

            if (eq1Type == null && !string.IsNullOrEmpty(shipGirlModel.Eq1Type))
            {
                eq1Type = new EquipmentType { Name = shipGirlModel.Eq1Type };
                cargoContext.EquipmentTypes.Add(eq1Type);
            }

            if (eq2Type == null && !string.IsNullOrEmpty(shipGirlModel.Eq1Type))
            {
                if (shipGirlModel.Eq2Type == shipGirlModel.Eq1Type)
                {
                    eq2Type = eq1Type;
                }
                else
                {
                    eq2Type = new EquipmentType { Name = shipGirlModel.Eq2Type };
                    cargoContext.EquipmentTypes.Add(eq2Type);
                }
            }

            if (eq3Type == null && !string.IsNullOrEmpty(shipGirlModel.Eq1Type))
            {
                if (shipGirlModel.Eq3Type == shipGirlModel.Eq1Type)
                {
                    eq3Type = eq1Type;
                }
                else
                {
                    if (shipGirlModel.Eq3Type == shipGirlModel.Eq2Type)
                    {
                        eq3Type = eq2Type;
                    }
                    else
                    {
                        eq3Type = new EquipmentType { Name = shipGirlModel.Eq3Type };
                        cargoContext.EquipmentTypes.Add(eq3Type);
                    }
                }
            }

            FK_Rarity = rarity;
            FK_Nationality = nationality;
            FK_ShipClass = shipClass;
            FK_ShipGroup = shipGroup;
            FK_ShipType = shipType;
            FK_SubtypeRetro = subtypeRetro;

            FK_Eq1Type = eq1Type;
            FK_Eq2Type = eq2Type;
            FK_Eq3Type = eq3Type;
        }


        [Key]
        [MaxLength(40)]
        public string ShipID { get; set; }
        [MaxLength(40)]
        public string Name { get; set; }
        [MaxLength(30)]
        public string ConstructTime { get; set; }
        [MaxLength(3)]
        public string Remodel { get; set; }
        [MaxLength(10)]
        public string Armor { get; set; }
        public int? Speed { get; set; }
        public int? Luck { get; set; }

        [MaxLength(1000)]
        public string LimitBreak1 { get; set; }
        [MaxLength(1000)]
        public string LimitBreak2 { get; set; }
        [MaxLength(1000)]
        public string LimitBreak3 { get; set; }

        #region Stats
        public int? HealthInitial { get; set; }
        public int? FireInitial { get; set; }
        public int? AAInitial { get; set; }
        public int? TorpInitial { get; set; }
        public int? AirInitial { get; set; }
        public int? ReloadInitial { get; set; }
        public int? EvadeInitial { get; set; }
        public int? ConsumptionInitial { get; set; }
        public int? AccInitial { get; set; }
        public int? ASWInitial { get; set; }
        public int? OxygenInitial { get; set; }
        public int? AmmoInitial { get; set; }
        public int? HealthMax { get; set; }
        public int? FireMax { get; set; }
        public int? AAMax { get; set; }
        public int? TorpMax { get; set; }
        public int? AirMax { get; set; }
        public int? ReloadMax { get; set; }
        public int? EvadeMax { get; set; }
        public int? ConsumptionMax { get; set; }
        public int? AccMax { get; set; }
        public int? ASWMax { get; set; }
        public int? OxygenMax { get; set; }
        public int? AmmoMax { get; set; }
        public int? HealthKai { get; set; }
        [MaxLength(50)]
        public string ArmorKai { get; set; }
        public int? FireKai { get; set; }
        public int? AAKai { get; set; }
        public int? TorpKai { get; set; }
        public int? AirKai { get; set; }
        public int? ReloadKai { get; set; }
        public int? EvadeKai { get; set; }
        public int? ConsumptionKai { get; set; }
        public int? SpeedKai { get; set; }
        public int? ASWKai { get; set; }
        public int? AccKai { get; set; }
        public int? OxygenKai { get; set; }
        public int? AmmoKai { get; set; }
        public int? Health120 { get; set; }
        public int? Fire120 { get; set; }
        public int? AA120 { get; set; }
        public int? Torp120 { get; set; }
        public int? Air120 { get; set; }
        public int? Reload120 { get; set; }
        public int? Evade120 { get; set; }
        public int? Consumption120 { get; set; }
        public int? Acc120 { get; set; }
        public int? ASW120 { get; set; }
        public int? Oxygen120 { get; set; }
        public int? Ammo120 { get; set; }
        public int? HealthKai120 { get; set; }
        public int? FireKai120 { get; set; }
        public int? AAKai120 { get; set; }
        public int? TorpKai120 { get; set; }
        public int? AirKai120 { get; set; }
        public int? ReloadKai120 { get; set; }
        public int? EvadeKai120 { get; set; }
        public int? ConsumptionKai120 { get; set; }
        public int? AccKai120 { get; set; }
        public int? ASWKai120 { get; set; }
        public int? OxygenKai120 { get; set; }
        public int? AmmoKai120 { get; set; }
        [MaxLength(50)]
        public string Eq1EffInit { get; set; }
        [MaxLength(50)]
        public string Eq1EffInitMax { get; set; }
        [MaxLength(50)]
        public string Eq1EffInitKai { get; set; }
        [MaxLength(50)]
        public string Eq2EffInit { get; set; }
        [MaxLength(50)]
        public string Eq2EffInitMax { get; set; }
        [MaxLength(50)]
        public string Eq2EffInitKai { get; set; }
        [MaxLength(50)]
        public string Eq3EffInit { get; set; }
        [MaxLength(50)]
        public string Eq3EffInitMax { get; set; }
        [MaxLength(50)]
        public string Eq3EffInitKai { get; set; }

        #endregion

        // Images
        [MaxLength(250)]
        public string Image { get; set; }
        [MaxLength(250)]
        public string ImageShipyardIcon { get; set; }
        [MaxLength(250)]
        public string ImageChibi { get; set; }
        [MaxLength(250)]
        public string ImageIcon { get; set; }
        [MaxLength(250)]
        public string ImageBanner { get; set; }
        [MaxLength(250)]
        public string ImageKai { get; set; }
        [MaxLength(250)]
        public string ImageShipyardIconKai { get; set; }
        [MaxLength(250)]
        public string ImageChibiKai { get; set; }
        [MaxLength(250)]
        public string ImageIconKai { get; set; }
        [MaxLength(250)]
        public string ImageBannerKai { get; set; }

        #region Relationships

        public ShipGirl()
        {
            WhereToGetShipGirl = new List<ShipGirlWhereToGetShipGirl>();
            Skills = new List<Skill>();
        }

        public virtual ICollection<ShipGirlWhereToGetShipGirl> WhereToGetShipGirl { get; set; }
        public virtual ICollection<Skill> Skills { get; set; }

        [MaxLength(40)]
        [ForeignKey("FK_Rarity")]
        public string Rarity { get; set; }
        public virtual Rarity FK_Rarity { get; set; }

        [MaxLength(40)]
        [ForeignKey("FK_ShipGroup")]
        public string ShipGroup { get; set; }
        public virtual ShipGroup FK_ShipGroup { get; set; }

        [MaxLength(20)]
        [ForeignKey("FK_Nationality")]
        public string Nationality { get; set; }
        public virtual Nationality FK_Nationality { get; set; }

        [MaxLength(40)]
        [ForeignKey("FK_ShipType")]
        public string Type { get; set; }
        public virtual ShipType FK_ShipType { get; set; }

        [MaxLength(20)]
        [ForeignKey("FK_SubtypeRetro")]
        public string SubtypeRetro { get; set; }
        public virtual ShipType FK_SubtypeRetro { get; set; }

        [MaxLength(20)]
        [ForeignKey("FK_ShipClass")]
        public string Class { get; set; }
        public virtual ShipClass FK_ShipClass { get; set; }



        [ForeignKey("FK_Eq1Type")]
        public string Eq1Type { get; set; }
        public virtual EquipmentType FK_Eq1Type { get; set; }

        [ForeignKey("FK_Eq2Type")]
        public string Eq2Type { get; set; }
        public virtual EquipmentType FK_Eq2Type { get; set; }

        [ForeignKey("FK_Eq3Type")]
        public string Eq3Type { get; set; }
        public virtual EquipmentType FK_Eq3Type { get; set; }



        public virtual ShipGirlImages FK_Images { get; set; }
        public virtual ShipGirlStats FK_InitialStats { get; set; }
        public virtual ShipGirlStats FK_Level100Stats { get; set; }
        public virtual ShipGirlStats FK_Level120Stats { get; set; }
        public virtual ShipGirlRetrofit FK_ShipGirlRetrofit { get; set; }
        #endregion
    }
}
