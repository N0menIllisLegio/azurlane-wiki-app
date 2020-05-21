using azurlane_wiki_app.Data.Tables;

namespace azurlane_wiki_app.PageEquipmentList.Items
{
    public class Plane : BaseEquipmentItem
    {
        public int Aviation { get; set; }
        public string Ordnance { get; set; }
        public string Guns { get; set; }
        public float Rld { get; set; }

        public Plane(Equipment equipment) : base(equipment)
        {
            Aviation = equipment.Aviation ?? 0;
            Ordnance = $"{equipment.Bombs1}\n{equipment.Bombs2}".Trim();
            Guns = $"{equipment.AAGun1}\n{equipment.AAGun2}".Trim();
            Rld = equipment.RoFMax ?? 0;
        }
    }
}
