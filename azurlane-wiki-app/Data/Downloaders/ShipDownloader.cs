using azurlane_wiki_app.Data.Tables;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace azurlane_wiki_app.Data.Downloaders
{
    class ShipDownloader : DataDownloader
    {
        private static readonly string KaiImagesFolderPath = ImagesFolderPath + "/Ships/KaiImages";
        private static readonly string NonKaiImagesFolderPath = ImagesFolderPath + "/Ships/Images";
        private readonly Dictionary<string, string> Abbreviations = new Dictionary<string, string>
        {
            ["Destroyer"] = "DD",
            ["Light Cruiser"] = "CL",
            ["Heavy Cruiser"] = "CA",
            ["Aircraft Carrier"] = "CV",
            ["Light Aircraft Carrier"] = "CVL",
            ["Battleship"] = "BB",
            ["Submarine"] = "SS",
            ["Large Cruiser"] = "CB",
            ["Repair Ship"] = "AR",
            ["Battlecruiser"] = "BC",
            ["Monitor"] = "BM",
            ["Submarine Carrier"] = "SSV",
            ["Aviation Battleship"] = "BBV" 
        };

        /// <summary>
        /// Coin, Oil, Medal
        /// </summary>
        private readonly Dictionary<string, (int?, int?, int?)> ScrapValues = new Dictionary<string, (int?, int?, int?)>
        {
            ["Super Rare"] = (4, 3, 10),
            ["Elite"] = (4, 3, 4),
            ["Rare"] = (4, 3, 1),
            ["Normal"] = (4, 3, 0),
            ["Priority"] = (null, null, null),
            ["Decisive"] = (null, null, null),
            ["Unreleased"] = (null, null, null)
        };

        private const string ShipFields = "ShipGroup,ShipID,Name,Rarity,Nationality,ConstructTime,Type," +
                                          "SubtypeRetro,Class,Remodel,Image,ImageShipyardIcon,ImageChibi,ImageIcon," +
                                          "ImageBanner,ImageKai,ImageShipyardIconKai,ImageChibiKai," +
                                          "ImageIconKai,ImageBannerKai,HealthInitial,Armor,FireInitial,AAInitial,TorpInitial," +
                                          "AirInitial,ReloadInitial,EvadeInitial,ConsumptionInitial," +
                                          "Speed,Luck,AccInitial,ASWInitial,OxygenInitial,AmmoInitial,HealthMax,FireMax,AAMax," +
                                          "TorpMax,AirMax,ReloadMax,EvadeMax,ConsumptionMax,AccMax," +
                                          "ASWMax,OxygenMax,AmmoMax,HealthKai,ArmorKai,FireKai,AAKai,TorpKai,AirKai,ReloadKai," +
                                          "EvadeKai,ConsumptionKai,SpeedKai,ASWKai,AccKai,OxygenKai," +
                                          "AmmoKai,Health120,Fire120,AA120,Torp120,Air120,Reload120,Evade120,Consumption120," +
                                          "Acc120,ASW120,Oxygen120,Ammo120,HealthKai120,FireKai120," +
                                          "AAKai120,TorpKai120,AirKai120,ReloadKai120,EvadeKai120,ConsumptionKai120,AccKai120," +
                                          "ASWKai120,OxygenKai120,AmmoKai120,Eq1Type,Eq1EffInit," +
                                          "Eq1EffInitMax,Eq1EffInitKai,Eq2Type,Eq2EffInit,Eq2EffInitMax,Eq2EffInitKai,Eq3Type," +
                                          "Eq3EffInit,Eq3EffInitMax,Eq3EffInitKai,LB1,LB2,LB3";

        public ShipDownloader(int ThreadsCount = 0) : base(ThreadsCount)
        {
            if (!Directory.Exists(KaiImagesFolderPath))
            {
                Directory.CreateDirectory(KaiImagesFolderPath);
            }
            if (!Directory.Exists(NonKaiImagesFolderPath))
            {
                Directory.CreateDirectory(NonKaiImagesFolderPath);
            }
        }

        /// <summary>
        /// Download all Ship Girls and save them.
        /// </summary>
        public override async Task Download()
        {
            Status = Statuses.InProgress;
            List<ShipGirlJsonWrapper> wrappedGirls;

            try
            {
                string responseJson = await GetData("ships", ShipFields, "");
                wrappedGirls = JsonConvert.DeserializeObject<List<ShipGirlJsonWrapper>>(responseJson);
            }
            catch(JsonException)
            {
                Status = Statuses.ErrorInDeserialization;
                return;
            }
            catch
            {
                Status = Statuses.DownloadError;
                return;
            }
            
            TotalImageCount = wrappedGirls.Count * 10;

            using (CargoContext cargoContext = new CargoContext())
            {
                foreach (ShipGirlJsonWrapper wrappedGirl in wrappedGirls)
                {
                    // Add images in queue for download
                    downloadBlock.Post(wrappedGirl.ShipGirl.Image);
                    downloadBlock.Post(wrappedGirl.ShipGirl.ImageBanner);
                    downloadBlock.Post(wrappedGirl.ShipGirl.ImageChibi);
                    downloadBlock.Post(wrappedGirl.ShipGirl.ImageIcon);
                    downloadBlock.Post(wrappedGirl.ShipGirl.ImageKai);
                    downloadBlock.Post(wrappedGirl.ShipGirl.ImageBannerKai);
                    downloadBlock.Post(wrappedGirl.ShipGirl.ImageChibiKai);
                    downloadBlock.Post(wrappedGirl.ShipGirl.ImageIconKai);
                    downloadBlock.Post(wrappedGirl.ShipGirl.ImageShipyardIcon);
                    downloadBlock.Post(wrappedGirl.ShipGirl.ImageShipyardIconKai);
                }

                downloadBlock.Complete();

                foreach (ShipGirlJsonWrapper wrappedGirl in wrappedGirls)
                {
                    if (await cargoContext.ShipGirls.FindAsync(wrappedGirl.ShipGirl.ShipID) == null)
                    {
                        CreateRelationships(wrappedGirl.ShipGirl, cargoContext);
                        SavePaths(wrappedGirl.ShipGirl);
                        cargoContext.ShipGirls.Add(wrappedGirl.ShipGirl);
                    }
                }

                await cargoContext.SaveChangesAsync();
            }

            //downloadBlock.Completion.Wait();
            Status = Statuses.DownloadComplete;
        }

        /// <summary>
        /// Download one ShipGirl and update her or save if she doesn't exist.
        /// </summary>
        /// <param name="id">Ship Girl's id</param>
        public override async Task Download(string id)
        {
            Status = Statuses.InProgress;
            List<ShipGirlJsonWrapper> wrappedGirls;

            try
            {
                string responseJson = await GetData("ships", ShipFields, "ships.ShipID=\'" + id + "\'");
                wrappedGirls = JsonConvert.DeserializeObject<List<ShipGirlJsonWrapper>>(responseJson);
            }
            catch(JsonException)
            {
                Status = Statuses.ErrorInDeserialization;
                return;
            }
            catch
            {
                Status = Statuses.DownloadError;
                return;
            }
            
            ShipGirlJsonWrapper wrappedGirl = wrappedGirls.FirstOrDefault();

            if (wrappedGirl == null)
            {
                Status = Statuses.EmptyResponse;
                return;
            }

            TotalImageCount = wrappedGirls.Count * 10;

            using (CargoContext cargoContext = new CargoContext())
            {
                // Add images in queue for download
                downloadBlock.Post(wrappedGirl.ShipGirl.Image);
                downloadBlock.Post(wrappedGirl.ShipGirl.ImageBanner);
                downloadBlock.Post(wrappedGirl.ShipGirl.ImageChibi);
                downloadBlock.Post(wrappedGirl.ShipGirl.ImageIcon);
                downloadBlock.Post(wrappedGirl.ShipGirl.ImageKai);
                downloadBlock.Post(wrappedGirl.ShipGirl.ImageBannerKai);
                downloadBlock.Post(wrappedGirl.ShipGirl.ImageChibiKai);
                downloadBlock.Post(wrappedGirl.ShipGirl.ImageIconKai);
                downloadBlock.Post(wrappedGirl.ShipGirl.ImageShipyardIcon);
                downloadBlock.Post(wrappedGirl.ShipGirl.ImageShipyardIconKai);
                
                downloadBlock.Complete();

                CreateRelationships(wrappedGirl.ShipGirl, cargoContext);
                SavePaths(wrappedGirl.ShipGirl);

                if (await cargoContext.ShipGirls.FindAsync(wrappedGirl.ShipGirl.ShipID) == null)
                {
                    cargoContext.ShipGirls.Add(wrappedGirl.ShipGirl);
                    await cargoContext.SaveChangesAsync();
                }
                else
                {
                    await cargoContext.Update(wrappedGirl.ShipGirl);
                }
            }

            //downloadBlock.Completion.Wait();
            Status = Statuses.DownloadComplete;
        }

        /// <summary>
        /// Get path to image folder that stores ShipGirls images.
        /// </summary>
        /// <param name="imageName">Name of image for saving</param>
        /// <returns>Path</returns>
        public override string GetImageFolder(string imageName)
        {
            string folderName;

            if (imageName.Contains("Kai"))
            {
                // Retrofit folders
                folderName = KaiImagesFolderPath;

                if (imageName.Contains("ShipyardIcon"))
                {
                    folderName += "/KaiShipyardIcons";
                }
                else if (imageName.Contains("Chibi"))
                {
                    folderName += "/KaiChibis";
                }
                else if (imageName.Contains("Banner"))
                {
                    folderName += "/KaiBanners";
                }
                else if (imageName.Contains("Icon"))
                {
                    folderName += "/KaiIcons";
                }
                else
                {
                    folderName += "/KaiImages";
                }
            }
            else
            {
                folderName = NonKaiImagesFolderPath;

                if (imageName.Contains("ShipyardIcon"))
                {
                    folderName += "/ShipyardIcons";
                }
                else if (imageName.Contains("Chibi"))
                {
                    folderName += "/Chibis";
                }
                else if (imageName.Contains("Banner"))
                {
                    folderName += "/Banners";
                }
                else if (imageName.Contains("Icon"))
                {
                    folderName += "/Icons";
                }
                else
                {
                    folderName += "/Images";
                }
            }

            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            return folderName;
        }

        /// <summary>
        /// Create relationships depending on values of shipGirl.{ nationality, rarity, class, type, group }.
        /// If { nationality, rarity, class, type, group } doesn't exists, creates it.
        /// </summary>
        /// <param name="shipGirl">ShipGirl</param>
        /// <param name="cargoContext">CargoContext</param>
        private void CreateRelationships(ShipGirl shipGirl, CargoContext cargoContext)
        {
            Rarity rarity = cargoContext.Rarities.Find(shipGirl.Rarity);
            Nationality nationality = cargoContext.Nationalities.Find(shipGirl.Nationality);
            ShipClass shipClass = cargoContext.ShipClasses.Find(shipGirl.Class);
            ShipGroup shipGroup = cargoContext.ShipGroups.Find(shipGirl.ShipGroup);

            ShipType shipType = cargoContext.ShipTypes.Find(shipGirl.Type);
            ShipType subtypeRetro = cargoContext.ShipTypes.Find(shipGirl.SubtypeRetro);

            EquipmentType eq1Type = cargoContext.EquipmentTypes.Find(shipGirl.Eq1Type);
            EquipmentType eq2Type = cargoContext.EquipmentTypes.Find(shipGirl.Eq2Type);
            EquipmentType eq3Type = cargoContext.EquipmentTypes.Find(shipGirl.Eq3Type);

            if (rarity == null)
            {
                (int? coins, int? oil, int? medals) = ScrapValues[shipGirl.Rarity];

                rarity = new Rarity
                {
                    Name = shipGirl.Rarity,
                    FK_Icon = cargoContext.Icons.Find(shipGirl.Rarity),
                    ScrapCoins = coins,
                    ScrapOil = oil,
                    ScrapMedals = medals
                };

                cargoContext.Rarities.Add(rarity);
            }
            
            if (nationality == null)
            {
                nationality = new Nationality
                {
                    Name = shipGirl.Nationality,
                    FK_Icon = cargoContext.Icons.Find(shipGirl.Nationality)
                };

                cargoContext.Nationalities.Add(nationality);
            }

            if (shipClass == null)
            {
                shipClass = new ShipClass { Name = shipGirl.Class };
                cargoContext.ShipClasses.Add(shipClass);
            }

            if (shipGroup == null)
            {
                shipGroup = new ShipGroup { Name = shipGirl.ShipGroup };
                cargoContext.ShipGroups.Add(shipGroup);
            }

            if (shipType == null)
            {
                shipType = new ShipType
                {
                    Name = shipGirl.Type,
                    Abbreviation = Abbreviations[shipGirl.Type],
                    FK_Icon = cargoContext.Icons.Find(Abbreviations[shipGirl.Type])
                };

                cargoContext.ShipTypes.Add(shipType);
            }

            if (subtypeRetro == null)
            {
                if (shipGirl.SubtypeRetro.Trim() == "")
                {
                    shipGirl.SubtypeRetro = null;
                }
                else
                {
                    subtypeRetro = new ShipType
                    {
                        Name = shipGirl.SubtypeRetro,
                        Abbreviation = Abbreviations[shipGirl.SubtypeRetro],
                        FK_Icon = cargoContext.Icons.Find(Abbreviations[shipGirl.SubtypeRetro])
                    };

                    cargoContext.ShipTypes.Add(subtypeRetro);
                }
            }

            if (eq1Type == null && !string.IsNullOrEmpty(shipGirl.Eq1Type))
            {
                eq1Type = new EquipmentType {Name = shipGirl.Eq1Type};
                cargoContext.EquipmentTypes.Add(eq1Type);
            }

            if (eq2Type == null && !string.IsNullOrEmpty(shipGirl.Eq1Type))
            {
                if (shipGirl.Eq2Type == shipGirl.Eq1Type)
                {
                    eq2Type = eq1Type;
                }
                else
                {
                    eq2Type = new EquipmentType { Name = shipGirl.Eq2Type };
                    cargoContext.EquipmentTypes.Add(eq2Type);
                }
            }

            if (eq3Type == null && !string.IsNullOrEmpty(shipGirl.Eq1Type))
            {
                if (shipGirl.Eq3Type == shipGirl.Eq1Type)
                {
                    eq3Type = eq1Type;
                }
                else
                {
                    if (shipGirl.Eq3Type == shipGirl.Eq2Type)
                    {
                        eq3Type = eq2Type;
                    }
                    else
                    {
                        eq3Type = new EquipmentType { Name = shipGirl.Eq3Type };
                        cargoContext.EquipmentTypes.Add(eq3Type);
                    }
                }
            }

            shipGirl.FK_Rarity = rarity;
            shipGirl.FK_Nationality = nationality;
            shipGirl.FK_ShipClass = shipClass;
            shipGirl.FK_ShipGroup = shipGroup;
            shipGirl.FK_ShipType = shipType;
            shipGirl.FK_SubtypeRetro = subtypeRetro;

            shipGirl.FK_Eq1Type = eq1Type;
            shipGirl.FK_Eq2Type = eq2Type;
            shipGirl.FK_Eq3Type = eq3Type;

            cargoContext.SaveChanges();
        }

        /// <summary>
        /// Change ShipGirls image fields from image name to relative path to image
        /// </summary>
        /// <param name="shipGirl">ShipGirl for changing</param>
        private void SavePaths(ShipGirl shipGirl)
        {
            shipGirl.Image = GetImageFolder(shipGirl.Image) + "/" + shipGirl.Image;
            shipGirl.ImageIcon = GetImageFolder(shipGirl.ImageIcon) + "/" + shipGirl.ImageIcon;
            shipGirl.ImageBanner = GetImageFolder(shipGirl.ImageBanner) + "/" + shipGirl.ImageBanner;
            shipGirl.ImageChibi = GetImageFolder(shipGirl.ImageChibi) + "/" + shipGirl.ImageChibi;
            shipGirl.ImageShipyardIcon = GetImageFolder(shipGirl.ImageShipyardIcon) + "/" + shipGirl.ImageShipyardIcon;

            // Kai
            shipGirl.ImageShipyardIconKai = !string.IsNullOrEmpty(shipGirl.ImageShipyardIconKai) 
                ? GetImageFolder(shipGirl.ImageShipyardIconKai) + "/" + shipGirl.ImageShipyardIconKai
                : null;
            shipGirl.ImageBannerKai = !string.IsNullOrEmpty(shipGirl.ImageBannerKai) 
                ? GetImageFolder(shipGirl.ImageBannerKai) + "/" + shipGirl.ImageBannerKai
                : null;
            shipGirl.ImageKai = !string.IsNullOrEmpty(shipGirl.ImageKai) 
                ? GetImageFolder(shipGirl.ImageKai) + "/" + shipGirl.ImageKai
                : null;
            shipGirl.ImageIconKai = !string.IsNullOrEmpty(shipGirl.ImageIconKai)
                ? GetImageFolder(shipGirl.ImageIconKai) + "/" + shipGirl.ImageIconKai
                : null;
            shipGirl.ImageChibiKai = !string.IsNullOrEmpty(shipGirl.ImageChibiKai)
                ? GetImageFolder(shipGirl.ImageChibiKai) + "/" + shipGirl.ImageChibiKai
                : null;
        }
    }
}
