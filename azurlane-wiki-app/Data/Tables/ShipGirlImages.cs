using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace azurlane_wiki_app.Data.Tables
{
    [Table("ShipGirlsImages")]
    public class ShipGirlImages
    {
        [Key]
        public int ID { get; set; }

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
    }
}
