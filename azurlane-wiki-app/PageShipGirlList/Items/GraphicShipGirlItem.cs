using azurlane_wiki_app.Data.Tables;

namespace azurlane_wiki_app.PageShipGirlList.Items
{
    public class GraphicShipGirlItem : BaseShipGirlItem
    {
        public string ImageShipyardIcon { get; set; }
        public string ImageShipyardIconKai { get; set; }

        public GraphicShipGirlItem(ShipGirl shipGirl) : base(shipGirl)
        {
            ImageShipyardIcon = shipGirl.ImageShipyardIcon;
            ImageShipyardIconKai = shipGirl.ImageShipyardIconKai;
        }
    }
}
