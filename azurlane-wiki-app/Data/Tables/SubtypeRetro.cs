using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace azurlane_wiki_app.Data.Tables
{
    public class SubtypeRetro
    {
        [Key]
        public string Name { get; set; }
        public string Abbreviation { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }

        #region Relationships

        public SubtypeRetro()
        {
            ShipGirls = new List<ShipGirl>();
        }

        public virtual ICollection<ShipGirl> ShipGirls { get; set; }

        public virtual Icon FK_Icon { get; set; }

        #endregion
    }
}