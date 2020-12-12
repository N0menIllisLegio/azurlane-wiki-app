using azurlane_wiki_app.Data.Tables;

namespace azurlane_wiki_app.Data.Downloaders.ServerModels
{
    public enum Stats
    {
        Initial,
        Level100,
        Level120,
        RetrofitedInitial,
        Retrofited120
    }

    public enum Images
    {
        Standard,
        Retrofited
    }

    public class RequestShipGirlModel
    {
        public string ShipID { get; set; }
        public string Name { get; set; }
        public string ConstructTime { get; set; }
        public string Remodel { get; set; }
        public string Armor { get; set; }
        public int? Speed { get; set; }
        public int? Luck { get; set; }
        public string LB1 { get; set; }
        public string LB2 { get; set; }
        public string LB3 { get; set; }
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
        public string Eq1EffInit { get; set; }
        public string Eq1EffInitMax { get; set; }
        public string Eq1EffInitKai { get; set; }
        public string Eq2EffInit { get; set; }
        public string Eq2EffInitMax { get; set; }
        public string Eq2EffInitKai { get; set; }
        public string Eq3EffInit { get; set; }
        public string Eq3EffInitMax { get; set; }
        public string Eq3EffInitKai { get; set; }
        public string Image { get; set; }
        public string ImageShipyardIcon { get; set; }
        public string ImageChibi { get; set; }
        public string ImageIcon { get; set; }
        public string ImageBanner { get; set; }
        public string ImageKai { get; set; }
        public string ImageShipyardIconKai { get; set; }
        public string ImageChibiKai { get; set; }
        public string ImageIconKai { get; set; }
        public string ImageBannerKai { get; set; }
        public string Rarity { get; set; }
        public string ShipGroup { get; set; }
        public string Nationality { get; set; }
        public string Type { get; set; }
        public string SubtypeRetro { get; set; }
        public string Class { get; set; }
        public string Eq1Type { get; set; }
        public string Eq2Type { get; set; }
        public string Eq3Type { get; set; }

        public ShipGirlImages GetImages(Images images)
        {
            return images switch
            {
                Images.Standard => new ShipGirlImages
                {
                    Image = Image,
                    ImageIcon = ImageIcon,
                    ImageBanner = ImageBanner,
                    ImageChibi = ImageChibi,
                    ImageShipyardIcon = ImageShipyardIcon
                },

                Images.Retrofited => new ShipGirlImages
                {
                    Image = ImageKai,
                    ImageIcon = ImageIconKai,
                    ImageBanner = ImageBannerKai,
                    ImageChibi = ImageChibiKai,
                    ImageShipyardIcon = ImageShipyardIconKai
                },

                _ => null
            };
        }

        public ShipGirlStats GetStats(Stats statsLevel)
        {
            return statsLevel switch
            {
                Stats.Initial => new ShipGirlStats
                {
                    Health = HealthInitial ?? 0,
                    Fire = FireInitial ?? 0,
                    AA = AAInitial ?? 0,
                    Torp = TorpInitial ?? 0,
                    Air = AirInitial ?? 0,
                    Reload = ReloadInitial ?? 0,
                    Evade = EvadeInitial ?? 0,
                    Consumption = ConsumptionInitial ?? 0,
                    Accuracy = AccInitial ?? 0,
                    ASW = ASWInitial ?? 0,
                    Oxygen = OxygenInitial ?? 0,
                    Ammo = AmmoInitial ?? 0,

                    Eq1Efficiency = Eq1EffInit,
                    Eq2Efficiency = Eq2EffInit,
                    Eq3Efficiency = Eq3EffInit
                },

                Stats.Level100 => new ShipGirlStats
                {
                    Health = HealthMax ?? 0,
                    Fire = FireMax ?? 0,
                    AA = AAMax ?? 0,
                    Torp = TorpMax ?? 0,
                    Air = AirMax ?? 0,
                    Reload = ReloadMax ?? 0,
                    Evade = EvadeMax ?? 0,
                    Consumption = ConsumptionMax ?? 0,
                    Accuracy = AccMax ?? 0,
                    ASW = ASWMax ?? 0,
                    Oxygen = OxygenMax ?? 0,
                    Ammo = AmmoMax ?? 0,

                    Eq1Efficiency = Eq1EffInitMax,
                    Eq2Efficiency = Eq2EffInitMax,
                    Eq3Efficiency = Eq3EffInitMax
                },

                Stats.Level120 => new ShipGirlStats
                {
                    Health = Health120 ?? 0,
                    Fire = Fire120 ?? 0,
                    AA = AA120 ?? 0,
                    Torp = Torp120 ?? 0,
                    Air = Air120 ?? 0,
                    Reload = Reload120 ?? 0,
                    Evade = Evade120 ?? 0,
                    Consumption = Consumption120 ?? 0,
                    Accuracy = Acc120 ?? 0,
                    ASW = ASW120 ?? 0,
                    Oxygen = Oxygen120 ?? 0,
                    Ammo = Ammo120 ?? 0,

                    Eq1Efficiency = Eq1EffInitMax,
                    Eq2Efficiency = Eq2EffInitMax,
                    Eq3Efficiency = Eq3EffInitMax
                },

                Stats.RetrofitedInitial => new ShipGirlStats
                {
                    Health = HealthKai ?? 0,
                    Fire = FireKai ?? 0,
                    AA = AAKai ?? 0,
                    Torp = TorpKai ?? 0,
                    Air = AirKai ?? 0,
                    Reload = ReloadKai ?? 0,
                    Evade = EvadeKai ?? 0,
                    Consumption = ConsumptionKai ?? 0,
                    Accuracy = AccKai ?? 0,
                    ASW = ASWKai ?? 0,
                    Oxygen = OxygenKai ?? 0,
                    Ammo = AmmoKai ?? 0,

                    Eq1Efficiency = Eq1EffInitKai,
                    Eq2Efficiency = Eq2EffInitKai,
                    Eq3Efficiency = Eq3EffInitKai
                },

                Stats.Retrofited120 => new ShipGirlStats
                {
                    Health = HealthKai120 ?? 0,
                    Fire = FireKai120 ?? 0,
                    AA = AAKai120 ?? 0,
                    Torp = TorpKai120 ?? 0,
                    Air = AirKai120 ?? 0,
                    Reload = ReloadKai120 ?? 0,
                    Evade = EvadeKai120 ?? 0,
                    Consumption = ConsumptionKai120 ?? 0,
                    Accuracy = AccKai120 ?? 0,
                    ASW = ASWKai120 ?? 0,
                    Oxygen = OxygenKai120 ?? 0,
                    Ammo = AmmoKai120 ?? 0,

                    Eq1Efficiency = Eq1EffInitKai,
                    Eq2Efficiency = Eq2EffInitKai,
                    Eq3Efficiency = Eq3EffInitKai
                },

                _ => null
            };
        }
    }
}
