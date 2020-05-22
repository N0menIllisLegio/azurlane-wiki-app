﻿using azurlane_wiki_app.Data;
using azurlane_wiki_app.Data.Tables;
using System.Collections.Generic;

namespace azurlane_wiki_app.PageEquipmentList.Items
{
    public class ItemsListsStorage
    {
        private List<MainGun> MainGuns { get; set; } = new List<MainGun>();
        private List<AAGun> AAGuns { get; } = new List<AAGun>();
        private List<Torpedo> Torpedoes { get; } = new List<Torpedo>();
        private List<SubmarineTorpedo> SubmarineTorpedoes { get; } = new List<SubmarineTorpedo>();
        private List<Plane> Planes { get; } = new List<Plane>();
        private List<TorpedoBomberPlane> TorpedoBomberPlanes { get; } = new List<TorpedoBomberPlane>();
        private List<AuxiliaryItem> AuxiliaryItems { get; } = new List<AuxiliaryItem>();
        private List<ASWItem> AswItems { get; } = new List<ASWItem>();

        private List<string> GetDbTypes(string newType)
        {
            string[,] dictionary =
            {
                { "Anti-Air Guns", "AA Gun" },
                { "Destroyer Guns", "DD Gun" },
                { "Light Cruiser Guns", "CL Gun" },
                { "Battleship Guns", "BB Gun" },
                { "Heavy Cruiser Guns", "CA Gun" },
                { "Large Cruiser Guns", "CB Gun" },
                { "Heavy Cruiser Guns", "SS Gun" },
                { "Submarine Torpedoes", "Submarine Torpedo" },
                { "Anti-Submarine Equipment", "ASW Helicopter" },
                { "Anti-Submarine Equipment", "ASW Bomber" },
                { "Anti-Submarine Equipment", "Sonar" },
                { "Anti-Submarine Equipment", "Depth Charge" },
                { "Seaplanes", "Seaplane" },
                { "Torpedo Bomber Planes", "Torpedo Bomber" },
                { "Ship Torpedoes", "Torpedo" },
                { "Fighter Planes", "Fighter" },
                { "Dive Bomber Planes", "Dive Bomber" },
                { "Auxiliary Equipment", "Auxiliary" }
            };

            List<string> dbTypes = new List<string>();

            for (int i = 0; i < dictionary.GetLength(0); i++)
            {
                if (dictionary[i, 0] == newType)
                {
                    dbTypes.Add(dictionary[i, 1]);
                }
            }

            return dbTypes;
        }

        public object GetList(string newType)
        {
            switch (GetDbTypes(newType)[0])
            {
                case "AA Gun":
                    return AAGuns;
                case "Auxiliary":
                    return AuxiliaryItems;
                case "Submarine Torpedo":
                    return SubmarineTorpedoes;
                case "Torpedo":
                    return Torpedoes;
                case "Torpedo Bomber":
                    return TorpedoBomberPlanes;
                case string s when s.Contains("Gun"):
                    return MainGuns;
                case string s when s == "Fighter" || s == "Dive Bomber" || s == "Seaplane":
                    return Planes;
                default:
                    return AswItems;
            }
        }

        public void FillList(string newTypeFullName)
        {
            List<string> dbTypes = GetDbTypes(newTypeFullName);

            using (CargoContext cargoContext = new CargoContext())
            {
                List<Equipment> equipment = new List<Equipment>();

                foreach (string dbType in dbTypes)
                {
                    var temp = cargoContext.EquipmentTypes.Find(dbType)?.EquipmentList;

                    if (temp != null)
                    {
                        equipment.AddRange(temp);
                    }
                }

                foreach (Equipment equip in equipment)
                {
                    AddItemToList(equip, equip.FK_Type.Name);
                }
            }
        }

        private void AddItemToList(Equipment item, string newType)
        {
            switch (newType)
            {
                case "AA Gun":
                    AAGuns.Add(new AAGun(item));
                    break;
                case "Auxiliary":
                    AuxiliaryItems.Add(new AuxiliaryItem(item));
                    break;
                case "Submarine Torpedo":
                    SubmarineTorpedoes.Add(new SubmarineTorpedo(item));
                    break;
                case "Torpedo":
                    Torpedoes.Add(new Torpedo(item));
                    break;
                case "Torpedo Bomber":
                    TorpedoBomberPlanes.Add(new TorpedoBomberPlane(item));
                    break;
                case string s when s.Contains("Gun"):
                    MainGuns.Add(new MainGun(item));
                    break;
                case string s when s == "Fighter" || s == "Dive Bomber" || s == "Seaplane":
                    Planes.Add(new Plane(item));
                    break;
                default:
                    AswItems.Add(new ASWItem(item));
                    break;
            }
        }

        public void ClearLists()
        {
            //MainGuns = null;
            //MainGuns = new List<MainGun>();

            AAGuns.Clear();
            AuxiliaryItems.Clear();
            SubmarineTorpedoes.Clear();
            Torpedoes.Clear();
            TorpedoBomberPlanes.Clear();
            MainGuns.Clear();
            Planes.Clear();
            AswItems.Clear();
        }
    }
}
