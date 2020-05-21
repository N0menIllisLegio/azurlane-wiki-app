using azurlane_wiki_app.Data.Tables;

namespace azurlane_wiki_app.PageEquipmentList.Items
{
    public class TorpedoBomberPlane : Plane
    {
        public int Damage { get; set; }
        public TorpedoBomberPlane(Equipment equipment) : base(equipment)
        {
            Damage = equipment.DamageMax ?? 0;
        }
    }
}
