using System.ComponentModel.DataAnnotations;

namespace azurlane_wiki_app.Data.Tables
{
    public class ShipGirlWhereToGetShipGirl
    {
        [Key]
        public int Id { get; set; }

        public virtual ShipGirl FK_ShipGirl { get; set; }
        public virtual WhereToGetShipGirl FK_WhereToGetShipGirl { get; set; }

        [MaxLength(500)]
        public string Note { get; set; }
    }
}
