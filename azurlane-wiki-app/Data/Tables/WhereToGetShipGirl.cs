using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace azurlane_wiki_app.Data.Tables
{
    public class WhereToGetShipGirl
    {
        public WhereToGetShipGirl()
        {
            ShipGirls = new List<ShipGirlWhereToGetShipGirl>();
        }

        [Key]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<ShipGirlWhereToGetShipGirl> ShipGirls { get; set; }
    }
}
