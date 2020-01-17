using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using azurlane_wiki_app.Data.Tables;

namespace azurlane_wiki_app.Data
{
    class EquipmentDownloader : DataDownloader
    {
        private string EquipmentImagesFolderPath = ImagesFolderPath + "/Equipment";

        public EquipmentDownloader() : base()
        {
            if (!Directory.Exists(EquipmentImagesFolderPath))
            {
                Directory.CreateDirectory(EquipmentImagesFolderPath);
            }
        }

        public override string GetImageFolder(string imageName)
        {
            return EquipmentImagesFolderPath;
        }

        public override async Task Download()
        {
            Status = Statuses.InProgress;

            string equipFields = "Name,Image,Type,Stars,Nationality,Tech,Health,HealthMax,Torpedo,TorpMax,Firepower,FPMax," +
                                 "Aviation,AvMax,Evasion,EvasionMax,PlaneHP,PlaneHPMax,Reload,ReloadMax,ASW,ASWMax,Oxygen," +
                                 "OxygenMax,AA,AAMax,Luck,LuckMax,Acc,AccMax,Spd,SpdMax,Damage,DamageMax,RoF,RoFMax,Number," +
                                 "Spread,Angle,WepRange,Shells,Salvoes,Characteristic,PingFreq,VolleyTime,Coef,Ammo,AAGun1," +
                                 "AAGun2,Bombs1,Bombs2,DD,DDNote,CL,CLNote,CA,CANote,CB,CBNote,BM,BMNote,BB,BBNote,BC,BCNote," +
                                 "BBV,BBVNote,CV,CVNote,CVL,CVLNote,AR,ARNote,SS,SSNote,SSV,SSVNote,DropLocation,Notes";

            string responseJson = await GetData("equipment", equipFields);

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
            
            using (CargoContext cargoContext = new CargoContext())
            {
                TotalImageCount = wrappedEquipment.Count;
                foreach (EquipmentJsonWrapper wrpEquipment in wrappedEquipment)
                {
                    await DownloadImage(wrpEquipment.Equipment.Image);

                    if (cargoContext.ShipGirlsEquipment.Count(e => e.Name == wrpEquipment.Equipment.Name) == 0)
                    {
                        cargoContext.ShipGirlsEquipment.Add(wrpEquipment.Equipment);
                    }
                }

                cargoContext.SaveChanges();
            }

            Status = Statuses.DownloadComplete;
        }
    }
}
