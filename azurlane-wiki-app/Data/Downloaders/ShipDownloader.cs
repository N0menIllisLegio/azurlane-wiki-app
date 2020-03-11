﻿using azurlane_wiki_app.Data.Tables;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace azurlane_wiki_app.Data.Downloaders
{
    class ShipDownloader : DataDownloader
    {
        private readonly string KaiImagesFolderPath = ImagesFolderPath + "/Ships/KaiImages";
        private readonly string NonKaiImagesFolderPath = ImagesFolderPath + "/Ships/Images";

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

                    if (await cargoContext.ShipGirls.FindAsync(wrappedGirl.ShipGirl.ShipID) == null)
                    {
                        cargoContext.ShipGirls.Add(wrappedGirl.ShipGirl);
                    }
                }

                downloadBlock.Complete();
                await cargoContext.SaveChangesAsync();
            }

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
    }
}