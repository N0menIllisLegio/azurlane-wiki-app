using System.Collections;
using System.Collections.Generic;
using azurlane_wiki_app.PageShipGirlList.Items;

namespace azurlane_wiki_app.PageShipGirlList
{
    public class ShipGirlNameSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            GraphicShipGirlItem shipGirlX = x as GraphicShipGirlItem;
            GraphicShipGirlItem shipGirlY = y as GraphicShipGirlItem;

            if (shipGirlY == null || shipGirlX == null)
            {
                return 0;
            }

            return shipGirlX.Name.CompareTo(shipGirlY.Name);
        }
    }
    
    public class ShipGirlIDSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            GraphicShipGirlItem shipGirlX = x as GraphicShipGirlItem;
            GraphicShipGirlItem shipGirlY = y as GraphicShipGirlItem;

            if (shipGirlY == null || shipGirlX == null)
            {
                return 0;
            }

            return shipGirlX.ShipID.CompareTo(shipGirlY.ShipID);
        }
    } 
    
    public class ShipGirlTypeSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            GraphicShipGirlItem shipGirlX = x as GraphicShipGirlItem;
            GraphicShipGirlItem shipGirlY = y as GraphicShipGirlItem;

            if (shipGirlY == null || shipGirlX == null)
            {
                return 0;
            }

            return shipGirlX.Type.CompareTo(shipGirlY.Type);
        }
    } 
    
    public class ShipGirlRaritySorter : IComparer
    {
        Dictionary<string, int> rarities = new Dictionary<string, int>
        {
            ["Unreleased"] = 0,
            ["Normal"] = 1,
            ["Rare"] = 2,
            ["Elite"] = 3,
            ["Super Rare"] = 4,
            ["Ultra Rare"] = 5,
            ["Priority"] = 6,
            ["Decisive"] = 7,
        };

        public int Compare(object x, object y)
        {
            GraphicShipGirlItem shipGirlX = x as GraphicShipGirlItem;
            GraphicShipGirlItem shipGirlY = y as GraphicShipGirlItem;

            if (shipGirlY == null || shipGirlX == null)
            {
                return 0;
            }

            return rarities[shipGirlX.Rarity] - rarities[shipGirlY.Rarity];
        }
    }
}
