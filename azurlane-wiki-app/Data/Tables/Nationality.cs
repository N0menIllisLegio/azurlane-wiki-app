using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace azurlane_wiki_app.Data.Tables
{
    public class Nationality
    {
        [Key]
        public string Name { get; set; }
        public string Icon { get; set; }
        [MaxLength(4000)]
        public string Description { get; set; }

        #region Relationships

        public Nationality()
        {
            ShipGirls = new List<ShipGirl>();
        }

        public virtual ICollection<ShipGirl> ShipGirls { get; set; }

        #endregion
    }
}