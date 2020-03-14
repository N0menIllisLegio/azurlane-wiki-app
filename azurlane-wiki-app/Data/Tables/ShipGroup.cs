using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace azurlane_wiki_app.Data.Tables
{
    public class ShipGroup
    {
        [Key]
        public string Name { get; set; }
        [MaxLength(4000)]
        public string Description { get; set; }

        #region Relationships

        public ShipGroup()
        {
            ShipGirls = new List<ShipGirl>();
        }

        public virtual ICollection<ShipGirl> ShipGirls { get; set; }

        #endregion
    }
}