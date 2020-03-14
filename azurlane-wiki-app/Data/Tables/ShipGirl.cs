using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace azurlane_wiki_app.Data.Tables
{
    class ShipGirlJsonWrapper
    {
        [JsonProperty("title")]
        public ShipGirl ShipGirl { get; set; }
    }

    public class ShipGirl
    {
        [Key]
        [MaxLength(40)]
        public string ShipID { get; set; }
        [MaxLength(40)]
        public string Name { get; set; }
        //[MaxLength(10)]
        //public string Rarity { get; set; }
        //[MaxLength(40)]
        //public string ShipGroup { get; set; }
        //[MaxLength(20)]
        //public string Nationality { get; set; }
        [MaxLength(30)]
        public string ConstructTime { get; set; }
        //[MaxLength(40)]
        //public string Type { get; set; }
        //[MaxLength(20)]
        //public string SubtypeRetro { get; set; }
        //[MaxLength(20)]
        //public string Class { get; set; }
        [MaxLength(3)]
        public string Remodel { get; set; }
        public int? HealthInitial { get; set; }
        [MaxLength(10)]
        public string Armor { get; set; }
        public int? FireInitial { get; set; }
        public int? AAInitial { get; set; }
        public int? TorpInitial { get; set; }
        public int? AirInitial { get; set; }
        public int? ReloadInitial { get; set; }
        public int? EvadeInitial { get; set; }
        public int? ConsumptionInitial { get; set; }
        public int? Speed { get; set; }
        public int? Luck { get; set; }
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
        public string Eq1Type { get; set; }
        [MaxLength(50)]
        public string Eq1EffInit { get; set; }
        [MaxLength(50)]
        public string Eq1EffInitMax { get; set; }
        [MaxLength(50)]
        public string Eq1EffInitKai { get; set; }
        [MaxLength(50)]
        public string Eq2Type { get; set; }
        [MaxLength(50)]
        public string Eq2EffInit { get; set; }
        [MaxLength(50)]
        public string Eq2EffInitMax { get; set; }
        [MaxLength(50)]
        public string Eq2EffInitKai { get; set; }
        [MaxLength(50)]
        public string Eq3Type { get; set; }
        [MaxLength(50)]
        public string Eq3EffInit { get; set; }
        [MaxLength(50)]
        public string Eq3EffInitMax { get; set; }
        [MaxLength(50)]
        public string Eq3EffInitKai { get; set; }

        [JsonProperty("LB1")]
        [MaxLength(250)]
        public string LimitBreak1 { get; set; }
        [JsonProperty("LB2")]
        [MaxLength(250)]
        public string LimitBreak2 { get; set; }
        [JsonProperty("LB3")]
        [MaxLength(250)]
        public string LimitBreak3 { get; set; }
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
        public virtual SubtypeRetro FK_SubtypeRetro { get; set; }

        [MaxLength(20)]
        [ForeignKey("FK_ShipClass")]
        public string Class { get; set; }
        public virtual ShipClass FK_ShipClass { get; set; }

        #endregion
    }
}
