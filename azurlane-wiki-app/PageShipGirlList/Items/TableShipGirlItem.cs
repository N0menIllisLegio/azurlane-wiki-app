using azurlane_wiki_app.Data.Tables;

namespace azurlane_wiki_app.PageShipGirlList.Items
{
    public class TableShipGirlItem : BaseShipGirlItem
    {
        public string ShipGirlIcon { get; set; }
        public bool Remodel { get; set; }
        public int Firepower { get; set; }
        public int Health { get; set; }
        public int Evasion { get; set; }
        public int Torpedo { get; set; }
        public int Aviation { get; set; }
        public int AA { get; set; }

        public TableShipGirlItem(ShipGirl shipGirl) : base(shipGirl)
        {
            ShipGirlIcon = shipGirl.ImageIcon;
            Remodel = shipGirl.Remodel == "t";
            Aviation = shipGirl.Air120 ?? 0;
            AA = shipGirl.AA120 ?? 0;
            Health = shipGirl.Health120 ?? 0;
            Evasion = shipGirl.Evade120 ?? 0;
            Torpedo = shipGirl.Torp120 ?? 0;
            Firepower = shipGirl.Fire120 ?? 0;
        }
    }
}
