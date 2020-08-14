using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace azurlane_wiki_app.Data.Tables
{
    public class EquipmentType
    {
        [Key]
        public string Name { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }

        public float? Cooldown { get; set; }

        #region Relationships

        public EquipmentType()
        {
            EquipmentList = new List<Equipment>();
        }

        public virtual ICollection<Equipment> EquipmentList { get; set; }

        #endregion
    }
}
