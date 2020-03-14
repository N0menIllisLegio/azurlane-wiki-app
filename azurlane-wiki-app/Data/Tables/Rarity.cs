﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace azurlane_wiki_app.Data.Tables
{
    public class Rarity
    {
        [Key]
        public string Name { get; set; }
        public string Icon { get; set; }

        #region Relationships

        public Rarity()
        {
            ShipGirls = new List<ShipGirl>();
        }

        public virtual ICollection<ShipGirl> ShipGirls { get; set; }

        #endregion
    }
}
