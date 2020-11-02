using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace azurlane_wiki_app.Data.Tables
{
    [Table("ShipGirlsRetrofits")]
    public class ShipGirlRetrofit
    {
        [Key]
        public int ID { get; set; }

        public virtual ShipGirlStats FK_InitialStats { get; set; }
        public virtual ShipGirlStats FK_Level120Stats { get; set; }
        public virtual ShipGirlImages FK_Images { get; set; }
    }
}
