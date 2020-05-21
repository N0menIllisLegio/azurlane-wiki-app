using azurlane_wiki_app.Data.Tables;

namespace azurlane_wiki_app.PageEquipmentList.Items
{
    public class Torpedo : SubmarineTorpedo
    {
        //Preload DPS
        //public int Raw { get; set; }
        //public int L { get; set; }
        //public string M { get; set; }
        //public string H { get; set; }

        public Torpedo(Equipment equipment) : base(equipment)
        {
        }
    }
}
