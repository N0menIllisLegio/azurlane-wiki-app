﻿using System.Collections.Generic;
using azurlane_wiki_app.Data.Tables;

namespace azurlane_wiki_app.PageEquipmentList.Items
{
    public abstract class BaseEquipmentItem
    {
        protected BaseEquipmentItem(Equipment equipment)
        {
            Dictionary<int, string> StarsRarities = new Dictionary<int, string>
            {
                [1] = "Normal",
                [2] = "Normal",
                [3] = "Rare",
                [4] = "Elite",
                [5] = "Super Rare",
                [6] = "Ultra Rare",
            };

            string rarity = "Normal";

            if (equipment.Stars != null && (equipment.Stars > 0 && equipment.Stars < 7))
            {
                rarity = StarsRarities[(int)equipment.Stars];
            }

            this.Id = equipment.EquipmentId;
            this.Name = equipment.Name;
            this.Nationality = equipment.FK_Nationality.Name;
            this.NationalityIcon = equipment.FK_Nationality.FK_Icon.FileName;
            this.Type = equipment.FK_Type.Name;
            this.Icon = equipment.Image;
            this.Tech = equipment.FK_Tech.Name;
            this.Rarity = rarity;
            this.Stars = equipment.Stars ?? 0;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Tech { get; set; }
        public string Rarity { get; set; }
        public int Stars { get; set; }
        public string Nationality { get; set; }
        public string NationalityIcon { get; set; }
        public string Type { get; set; }
    }
}
