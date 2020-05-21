﻿using azurlane_wiki_app.Data.Tables;

namespace azurlane_wiki_app.PageShipGirlList.Items
{
    public class BaseShipGirlItem
    {
        public string Name { get; set; }
        public string ShipID { get; set; }
        public string TypeIcon { get; set; }
        public string Type { get; set; }
        public string Rarity { get; set; }
        public string RarityIcon { get; set; }
        public string NationalityIcon { get; set; }
        public string Nationality { get; set; }

        public BaseShipGirlItem(ShipGirl shipGirl)
        {
            ShipID = shipGirl.ShipID;
            Name = shipGirl.Name;
            Rarity = shipGirl.FK_Rarity.Name;
            RarityIcon = shipGirl.FK_Rarity.FK_Icon.FileName;
            NationalityIcon = shipGirl.FK_Nationality.FK_Icon.FileName;
            Nationality = shipGirl.FK_Nationality.Name;
            TypeIcon = shipGirl.FK_ShipType.FK_Icon.FileName;
            Type = shipGirl.FK_ShipType.Name;
        }
    }
}