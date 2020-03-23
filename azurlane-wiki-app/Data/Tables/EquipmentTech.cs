using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace azurlane_wiki_app.Data.Tables
{
    public class EquipmentTech
    {
        [Key]
        public string Name { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }

        #region Relationships

        public EquipmentTech()
        {
            EquipmentList = new List<Equipment>();
        }

        public virtual ICollection<Equipment> EquipmentList { get; set; }

        #endregion
    }
}
