﻿using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Threading.Tasks;
using azurlane_wiki_app.Data.Tables;
using Newtonsoft.Json;

namespace azurlane_wiki_app.Data.Downloaders
{
    class EquipmentDownloader : DataDownloader
    {
        private string EquipmentImagesFolderPath = ImagesFolderPath + "/Equipment";

        public EquipmentDownloader(int ThreadsCount = 0) : base(ThreadsCount)
        {
            if (!Directory.Exists(EquipmentImagesFolderPath))
            {
                Directory.CreateDirectory(EquipmentImagesFolderPath);
            }
        }

        public override Task Download(string id)
        {
            throw new System.NotImplementedException();
        }

        public override string GetImageFolder(string imageName)
        {
            return EquipmentImagesFolderPath;
        }

        public override async Task Download()
        {
            Status = Statuses.InProgress;
            string responseJson;

            try
            {
                string equipFields = "Name,Image,Type,Stars,Nationality,Tech,Health,HealthMax,Torpedo,TorpMax,Firepower,FPMax," +
                                     "Aviation,AvMax,Evasion,EvasionMax,PlaneHP,PlaneHPMax,Reload,ReloadMax,ASW,ASWMax,Oxygen," +
                                     "OxygenMax,AA,AAMax,Luck,LuckMax,Acc,AccMax,Spd,SpdMax,Damage,DamageMax,RoF,RoFMax,Number," +
                                     "Spread,Angle,WepRange,Shells,Salvoes,Characteristic,PingFreq,VolleyTime,Coef,Ammo,AAGun1," +
                                     "AAGun2,Bombs1,Bombs2,DD,DDNote,CL,CLNote,CA,CANote,CB,CBNote,BM,BMNote,BB,BBNote,BC,BCNote," +
                                     "BBV,BBVNote,CV,CVNote,CVL,CVLNote,AR,ARNote,SS,SSNote,SSV,SSVNote,DropLocation,Notes";

                responseJson = await GetData("equipment", equipFields, "");
            }
            catch
            {
                Status = Statuses.DownloadError;
                return;
            }

            List<EquipmentJsonWrapper> wrappedEquipment;

            try
            {
                wrappedEquipment = JsonConvert.DeserializeObject<List<EquipmentJsonWrapper>>(responseJson);
            }
            catch
            {
                Status = Statuses.ErrorInDeserialization;
                return;
            }
            
            TotalImageCount = wrappedEquipment.Count;
            using (CargoContext cargoContext = new CargoContext())
            {
                foreach (EquipmentJsonWrapper wrpEquipment in wrappedEquipment)
                {
                    if (await cargoContext.ShipGirlsEquipment
                            .CountAsync(e => e.Name == wrpEquipment.Equipment.Name) == 0)
                    {
                        cargoContext.ShipGirlsEquipment.Add(wrpEquipment.Equipment);
                    }

                    downloadBlock.Post(wrpEquipment.Equipment.Image);
                }

                downloadBlock.Complete();
                await cargoContext.SaveChangesAsync();
            }

            downloadBlock.Completion.Wait();

            Status = Statuses.DownloadComplete;
        }
    }
}
