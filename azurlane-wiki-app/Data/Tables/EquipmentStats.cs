using System.ComponentModel.DataAnnotations;

namespace azurlane_wiki_app.Data.Tables
{
    public class EquipmentStats
    {
        [Key]
        public int ID { get; set; }
        public int? Health { get; set; }
        public int? Torpedo { get; set; }
        public int? Firepower { get; set; }
        public int? Aviation { get; set; }
        public int? Evasion { get; set; }
        public int? PlaneHP { get; set; }
        public int? Reload { get; set; }
        public int? ASW { get; set; }
        public int? Oxygen { get; set; }
        public int? AA { get; set; }
        public int? Luck { get; set; }
        public int? Acc { get; set; }
        public int? Spd { get; set; }
        public int? Damage { get; set; }
        public double? RoF { get; set; }
    }
}
