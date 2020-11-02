using azurlane_wiki_app.Data.Tables;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace azurlane_wiki_app.Data.Downloaders
{
    class ShipGirlJsonWrapper
    {
        [JsonProperty("title")]
        public ShipGirl ShipGirl { get; set; }
    }

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
            ["Aviation Battleship"] = "BBV",
            ["Munition Ship"] = "AE"
        };

        /// <summary>
        /// Coin, Oil, Medal
        /// </summary>
        private readonly Dictionary<string, (int?, int?, int?)> ScrapValues = new Dictionary<string, (int?, int?, int?)>
        {
            ["Ultra Rare"] = (4, 3, 30),
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
            DownloadTitle = "Downloading ShipGirls...";

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
            StatusDataMessage = "Downloading data.";
            StatusImageMessage = "Pending.";
            List<ShipGirlJsonWrapper> wrappedGirls;

            try
            {
                string responseJson = await GetData("ships", ShipFields, "");
                wrappedGirls = JsonConvert.DeserializeObject<List<ShipGirlJsonWrapper>>(responseJson);
            }
            catch(JsonException)
            {
                Status = Statuses.ErrorInDeserialization;
                Logger.Write($"Failed to desirialize shipgirls.", this.GetType().ToString());
                return;
            }
            catch
            {
                Status = Statuses.DownloadError;
                Logger.Write($"Failed to get data for shipgirls from server.", this.GetType().ToString());
                return;
            }

            TotalDataCount = wrappedGirls.Count;
            TotalImageCount = wrappedGirls.Count * 10;

            using (CargoContext cargoContext = new CargoContext())
            {
                StatusImageMessage = "Adding images to download queue.";

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

                StatusImageMessage = "Downloading images.";
                StatusDataMessage = "Saving data.";

                foreach (ShipGirlJsonWrapper wrappedGirl in wrappedGirls)
                {
                    if (await cargoContext.ShipGirls.FindAsync(wrappedGirl.ShipGirl.ShipID) == null)
                    {
                        CreateRelationships(wrappedGirl.ShipGirl, cargoContext);
                        SavePaths(wrappedGirl.ShipGirl, cargoContext);
                        wrappedGirl.ShipGirl.Name = Refactor(wrappedGirl.ShipGirl.Name);
                        cargoContext.ShipGirls.Add(wrappedGirl.ShipGirl);
                    }

                    lock (locker)
                    {
                        CurrentDataCount++;
                    }
                }

                await cargoContext.SaveChangesAsync();
            }

            StatusDataMessage = "Complete.";
            downloadBlock.Completion.Wait();
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
                Logger.Write($"Failed to desirialize shipgirl. ID: {id}", this.GetType().ToString());
                return;
            }
            catch
            {
                Status = Statuses.DownloadError;
                Logger.Write($"Failed to get data for shipgirl from server. ID: {id}", this.GetType().ToString());
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
                
                CreateRelationships(wrappedGirl.ShipGirl, cargoContext);
                SavePaths(wrappedGirl.ShipGirl, cargoContext);
                wrappedGirl.ShipGirl.Name = Refactor(wrappedGirl.ShipGirl.Name);

                if (await cargoContext.ShipGirls.FindAsync(wrappedGirl.ShipGirl.ShipID) == null)
                {
                    cargoContext.ShipGirls.Add(wrappedGirl.ShipGirl);
                    await cargoContext.SaveChangesAsync();
                }
                else
                {
                    await cargoContext.Update(wrappedGirl.ShipGirl);
                }

                downloadBlock.Complete();
            }

            downloadBlock.Completion.Wait();
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

            AddStats(shipGirl, cargoContext);

            cargoContext.SaveChanges();
        }

        /// <summary>
        /// Change ShipGirls image fields from image name to relative path to image
        /// </summary>
        /// <param name="shipGirl">ShipGirl for changing</param>
        private void SavePaths(ShipGirl shipGirl, CargoContext cargoContext)
        {
            var images = new ShipGirlImages();

            images.Image = shipGirl.Image = GetImageFolder(shipGirl.Image) + "/" + RefactorImageName(shipGirl.Image);
            images.ImageIcon = shipGirl.ImageIcon = GetImageFolder(shipGirl.ImageIcon) + "/" + RefactorImageName(shipGirl.ImageIcon);
            images.ImageBanner = shipGirl.ImageBanner = GetImageFolder(shipGirl.ImageBanner) + "/" + RefactorImageName(shipGirl.ImageBanner);
            images.ImageChibi = shipGirl.ImageChibi = GetImageFolder(shipGirl.ImageChibi) + "/" + RefactorImageName(shipGirl.ImageChibi);
            images.ImageShipyardIcon = shipGirl.ImageShipyardIcon = GetImageFolder(shipGirl.ImageShipyardIcon) + "/" + RefactorImageName(shipGirl.ImageShipyardIcon);

            shipGirl.FK_Images = images;
            cargoContext.ShipGirlsImages.Add(images);

            // Kai
            shipGirl.ImageShipyardIconKai = !string.IsNullOrEmpty(shipGirl.ImageShipyardIconKai) 
                ? GetImageFolder(shipGirl.ImageShipyardIconKai) + "/" + RefactorImageName(shipGirl.ImageShipyardIconKai)
                : null;
            shipGirl.ImageBannerKai = !string.IsNullOrEmpty(shipGirl.ImageBannerKai) 
                ? GetImageFolder(shipGirl.ImageBannerKai) + "/" + RefactorImageName(shipGirl.ImageBannerKai)
                : null;
            shipGirl.ImageKai = !string.IsNullOrEmpty(shipGirl.ImageKai) 
                ? GetImageFolder(shipGirl.ImageKai) + "/" + RefactorImageName(shipGirl.ImageKai)
                : null;
            shipGirl.ImageIconKai = !string.IsNullOrEmpty(shipGirl.ImageIconKai)
                ? GetImageFolder(shipGirl.ImageIconKai) + "/" + RefactorImageName(shipGirl.ImageIconKai)
                : null;
            shipGirl.ImageChibiKai = !string.IsNullOrEmpty(shipGirl.ImageChibiKai)
                ? GetImageFolder(shipGirl.ImageChibiKai) + "/" + RefactorImageName(shipGirl.ImageChibiKai)
                : null;

            if (shipGirl.Remodel == "t")
            {
                AddRetrofit(shipGirl, cargoContext);
            }

        }

        private string Refactor(string text)
        {
            string refactoredText = text.Replace(@"&amp;#39;", "\'");

            return refactoredText;
        }

        private void AddRetrofit(ShipGirl shipGirl, CargoContext cargoContext)
        {
            var retrofitStats = new ShipGirlStats
            {
                Health = shipGirl.HealthKai ?? 0,
                Fire = shipGirl.FireKai ?? 0,
                AA = shipGirl.AAKai ?? 0,
                Torp = shipGirl.TorpKai ?? 0,
                Air = shipGirl.AirKai ?? 0,
                Reload = shipGirl.ReloadKai ?? 0,
                Evade = shipGirl.EvadeKai ?? 0,
                Consumption = shipGirl.ConsumptionKai ?? 0,
                Accuracy = shipGirl.AccKai ?? 0,
                ASW = shipGirl.ASWKai ?? 0,
                Oxygen = shipGirl.OxygenKai ?? 0,
                Ammo = shipGirl.AmmoKai ?? 0,

                Eq1Efficiency = shipGirl.Eq1EffInitKai,
                Eq2Efficiency = shipGirl.Eq2EffInitKai,
                Eq3Efficiency = shipGirl.Eq3EffInitKai
            };

            var retrofit120Stats = new ShipGirlStats
            {
                Health = shipGirl.HealthKai120 ?? 0,
                Fire = shipGirl.FireKai120 ?? 0,
                AA = shipGirl.AAKai120 ?? 0,
                Torp = shipGirl.TorpKai120 ?? 0,
                Air = shipGirl.AirKai120 ?? 0,
                Reload = shipGirl.ReloadKai120 ?? 0,
                Evade = shipGirl.EvadeKai120 ?? 0,
                Consumption = shipGirl.ConsumptionKai120 ?? 0,
                Accuracy = shipGirl.AccKai120 ?? 0,
                ASW = shipGirl.ASWKai120 ?? 0,
                Oxygen = shipGirl.OxygenKai120 ?? 0,
                Ammo = shipGirl.AmmoKai120 ?? 0,

                Eq1Efficiency = shipGirl.Eq1EffInitKai,
                Eq2Efficiency = shipGirl.Eq2EffInitKai,
                Eq3Efficiency = shipGirl.Eq3EffInitKai
            };

            var images = new ShipGirlImages
            {
                Image = shipGirl.ImageKai,
                ImageIcon = shipGirl.ImageIconKai,
                ImageBanner = shipGirl.ImageBannerKai,
                ImageChibi = shipGirl.ImageChibiKai,
                ImageShipyardIcon = shipGirl.ImageShipyardIconKai
            };

            var retrofit = new ShipGirlRetrofit
            {
                FK_Images = images,
                FK_InitialStats = retrofitStats,
                FK_Level120Stats = retrofit120Stats
            };

            cargoContext.ShipGirlsStats.Add(retrofitStats);
            cargoContext.ShipGirlsStats.Add(retrofit120Stats);
            cargoContext.ShipGirlsImages.Add(images);
            cargoContext.ShipGirlsRetrofits.Add(retrofit);
        }

        private void AddStats(ShipGirl shipGirl, CargoContext cargoContext)
        {
            var initStats = new ShipGirlStats
            {
                Health = shipGirl.HealthInitial ?? 0,
                Fire = shipGirl.FireInitial ?? 0,
                AA = shipGirl.AAInitial ?? 0,
                Torp = shipGirl.TorpInitial ?? 0,
                Air = shipGirl.AirInitial ?? 0,
                Reload = shipGirl.ReloadInitial ?? 0,
                Evade = shipGirl.EvadeInitial ?? 0,
                Consumption = shipGirl.ConsumptionInitial ?? 0,
                Accuracy = shipGirl.AccInitial ?? 0,
                ASW = shipGirl.ASWInitial ?? 0,
                Oxygen = shipGirl.OxygenInitial ?? 0,
                Ammo = shipGirl.AmmoInitial ?? 0,

                Eq1Efficiency = shipGirl.Eq1EffInit,
                Eq2Efficiency = shipGirl.Eq2EffInit,
                Eq3Efficiency = shipGirl.Eq3EffInit
            };

            var stats100 = new ShipGirlStats
            {
                Health = shipGirl.HealthMax ?? 0,
                Fire = shipGirl.FireMax ?? 0,
                AA = shipGirl.AAMax ?? 0,
                Torp = shipGirl.TorpMax ?? 0,
                Air = shipGirl.AirMax ?? 0,
                Reload = shipGirl.ReloadMax ?? 0,
                Evade = shipGirl.EvadeMax ?? 0,
                Consumption = shipGirl.ConsumptionMax ?? 0,
                Accuracy = shipGirl.AccMax ?? 0,
                ASW = shipGirl.ASWMax ?? 0,
                Oxygen = shipGirl.OxygenMax ?? 0,
                Ammo = shipGirl.AmmoMax ?? 0,

                Eq1Efficiency = shipGirl.Eq1EffInitMax,
                Eq2Efficiency = shipGirl.Eq2EffInitMax,
                Eq3Efficiency = shipGirl.Eq3EffInitMax
            };

            var stats120 = new ShipGirlStats
            {
                Health = shipGirl.Health120 ?? 0,
                Fire = shipGirl.Fire120 ?? 0,
                AA = shipGirl.AA120 ?? 0,
                Torp = shipGirl.Torp120 ?? 0,
                Air = shipGirl.Air120 ?? 0,
                Reload = shipGirl.Reload120 ?? 0,
                Evade = shipGirl.Evade120 ?? 0,
                Consumption = shipGirl.Consumption120 ?? 0,
                Accuracy = shipGirl.Acc120 ?? 0,
                ASW = shipGirl.ASW120 ?? 0,
                Oxygen = shipGirl.Oxygen120 ?? 0,
                Ammo = shipGirl.Ammo120 ?? 0,

                Eq1Efficiency = shipGirl.Eq1EffInitMax,
                Eq2Efficiency = shipGirl.Eq2EffInitMax,
                Eq3Efficiency = shipGirl.Eq3EffInitMax
            };

            shipGirl.FK_InitialStats = initStats;
            shipGirl.FK_Level100Stats = stats100;
            shipGirl.FK_Level120Stats = stats120;

            cargoContext.ShipGirlsStats.Add(initStats);
            cargoContext.ShipGirlsStats.Add(stats100);
            cargoContext.ShipGirlsStats.Add(stats120);
        }
    }
}
