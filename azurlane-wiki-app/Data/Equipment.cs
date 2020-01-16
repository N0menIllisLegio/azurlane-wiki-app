using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace azurlane_wiki_app.Data
{
    class EquipmentJsonWrapper
    {
        [JsonProperty("title")]
        public Equipment Equipment { get; set; }
    }

    [Table("Equipment")]
    class Equipment
    {
        [Key]
        public int EquipmentId { get; set; }

        [Index]
        [MaxLength(250)]
        public string Name { get; set; }

        [MaxLength(250)]
        public string Image { get; set; }
        [MaxLength(50)]
        public string Type { get; set; }
        [MaxLength(50)]
        public string Nationality { get; set; }
        [MaxLength(5)]
        public string Tech { get; set; }

        public int? Stars { get; set; }
        public int? Health { get; set; }
        public int? HealthMax { get; set; }
        public int? Torpedo { get; set; }
        public int? TorpMax { get; set; }
        public int? Firepower { get; set; }
        public int? FPMax { get; set; }
        public int? Aviation { get; set; }
        public int? AvMax { get; set; }
        public int? Evasion { get; set; }
        public int? EvasionMax { get; set; }
        public int? PlaneHP { get; set; }
        public int? PlaneHPMax { get; set; }
        public int? Reload { get; set; }
        public int? ReloadMax { get; set; }
        public int? ASW { get; set; }
        public int? ASWMax { get; set; }
        public int? Oxygen { get; set; }
        public int? OxygenMax { get; set; }
        public int? AA { get; set; }
        public int? AAMax { get; set; }
        public int? Luck { get; set; }
        public int? LuckMax { get; set; }
        public int? Acc { get; set; }
        public int? AccMax { get; set; }
        public int? Spd { get; set; }
        public int? SpdMax { get; set; }
        public int? Damage { get; set; }
        public int? DamageMax { get; set; }
        public int? Number { get; set; }
        public int? Spread { get; set; }
        public int? Angle { get; set; }
        public int? WepRange { get; set; }
        public int? Shells { get; set; }
        public int? Salvoes { get; set; }
        public int? Coef { get; set; }

        public float? RoF { get; set; } 
        public float? RoFMax { get; set; } 
        public float? PingFreq { get; set; } 
        public float? VolleyTime { get; set; } 

        [MaxLength(50)]
        public string Characteristic { get; set; }
        [MaxLength(50)]
        public string Ammo { get; set; }
        [MaxLength(150)]
        public string AAGun1 { get; set; }
        [MaxLength(150)]
        public string AAGun2 { get; set; }
        [MaxLength(150)]
        public string Bombs1 { get; set; }
        [MaxLength(150)]
        public string Bombs2 { get; set; }
        [MaxLength(5)]
        public string DD { get; set; }
        [MaxLength(250)]
        public string DDNote { get; set; }
        [MaxLength(5)]
        public string CL { get; set; }
        [MaxLength(250)]
        public string CLNote { get; set; }
        [MaxLength(5)]
        public string CA { get; set; }
        [MaxLength(250)]
        public string CANote { get; set; }
        [MaxLength(5)]
        public string CB { get; set; }
        [MaxLength(250)]
        public string CBNote { get; set; }
        [MaxLength(5)]
        public string BM { get; set; }
        [MaxLength(250)]
        public string BMNote { get; set; }
        [MaxLength(5)]
        public string BB { get; set; }
        [MaxLength(250)]
        public string BBNote { get; set; }
        [MaxLength(5)]
        public string BC { get; set; }
        [MaxLength(250)]
        public string BCNote { get; set; }
        [MaxLength(5)]
        public string BBV { get; set; }
        [MaxLength(250)]
        public string BBVNote { get; set; }
        [MaxLength(5)]
        public string CV { get; set; }
        [MaxLength(250)]
        public string CVNote { get; set; }
        [MaxLength(5)]
        public string CVL { get; set; }
        [MaxLength(250)]
        public string CVLNote { get; set; }
        [MaxLength(5)]
        public string AR { get; set; }
        [MaxLength(250)]
        public string ARNote { get; set; }
        [MaxLength(5)]
        public string SS { get; set; }
        [MaxLength(250)]
        public string SSNote { get; set; }
        [MaxLength(5)]
        public string SSV { get; set; }
        [MaxLength(250)]
        public string SSVNote { get; set; }
        [MaxLength(250)]
        public string DropLocation { get; set; }
        [MaxLength(250)]
        public string Notes { get; set; }
    }
}
