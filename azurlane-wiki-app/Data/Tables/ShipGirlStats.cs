using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace azurlane_wiki_app.Data.Tables
{
    [Table("ShipGirlsStats")]
    public class ShipGirlStats
    {
        [Key]
        public int ID { get; set; }
        public int Health { get; set; }
        public int Fire { get; set; }
        public int AA { get; set; }
        public int Torp { get; set; }
        public int Air { get; set; }
        public int Reload { get; set; }
        public int Evade { get; set; }
        public int Consumption { get; set; }
        public int Accuracy { get; set; }
        public int ASW { get; set; }
        public int Oxygen { get; set; }
        public int Ammo { get; set; }

        [MaxLength(50)]
        public string Eq1Efficiency { get; set; }
        [MaxLength(50)]
        public string Eq2Efficiency { get; set; }
        [MaxLength(50)]
        public string Eq3Efficiency { get; set; }
    }
}
