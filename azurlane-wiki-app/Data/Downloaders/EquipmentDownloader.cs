using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using azurlane_wiki_app.Data.Tables;
using Newtonsoft.Json;

namespace azurlane_wiki_app.Data.Downloaders
{
    class EquipmentDownloader : DataDownloader
    {
        private readonly string EquipmentImagesFolderPath = ImagesFolderPath + "/Equipment";
        private const string EquipmentFields = "Name,Image,Type,Stars,Nationality,Tech,Health,HealthMax,Torpedo,TorpMax,Firepower,FPMax," +
                                               "Aviation,AvMax,Evasion,EvasionMax,PlaneHP,PlaneHPMax,Reload,ReloadMax,ASW,ASWMax,Oxygen," +
                                               "OxygenMax,AA,AAMax,Luck,LuckMax,Acc,AccMax,Spd,SpdMax,Damage,DamageMax,RoF,RoFMax,Number," +
                                               "Spread,Angle,WepRange,Shells,Salvoes,Characteristic,PingFreq,VolleyTime,Coef,Ammo,AAGun1," +
                                               "AAGun2,Bombs1,Bombs2,DD,DDNote,CL,CLNote,CA,CANote,CB,CBNote,BM,BMNote,BB,BBNote,BC,BCNote," +
                                               "BBV,BBVNote,CV,CVNote,CVL,CVLNote,AR,ARNote,SS,SSNote,SSV,SSVNote,DropLocation,Notes";

        public EquipmentDownloader(int ThreadsCount = 0) : base(ThreadsCount)
        {
            if (!Directory.Exists(EquipmentImagesFolderPath))
            {
                Directory.CreateDirectory(EquipmentImagesFolderPath);
            }
        }

        /// <summary>
        /// Download all Equipment and save it.
        /// </summary>
        public override async Task Download()
        {
            Status = Statuses.InProgress;
            List<EquipmentJsonWrapper> wrappedEquipment;

            try
            {
                string responseJson = await GetData("equipment", EquipmentFields, "");
                wrappedEquipment = JsonConvert.DeserializeObject<List<EquipmentJsonWrapper>>(responseJson);
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

        /// <summary>
        /// Download one piece of Equipment by name and update it or save if it doesn't exist.
        /// </summary>
        /// <param name="id">Name of equipment.</param>
        public override async Task Download(string id)
        {
            Status = Statuses.InProgress;
            List<EquipmentJsonWrapper> wrappedEquipment;

            try
            {
                string responseJson = await GetData("equipment", EquipmentFields, "equipment.Name=\'" + id + "\'");
                wrappedEquipment = JsonConvert.DeserializeObject<List<EquipmentJsonWrapper>>(responseJson);
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

            EquipmentJsonWrapper wrpEquipment = wrappedEquipment.FirstOrDefault();

            if (wrpEquipment == null)
            {
                Status = Statuses.EmptyResponse;
                return;
            }

            TotalImageCount = wrappedEquipment.Count;

            using (CargoContext cargoContext = new CargoContext())
            {
                downloadBlock.Post(wrpEquipment.Equipment.Image);

                if (await cargoContext.ShipGirlsEquipment
                        .CountAsync(e => e.Name == wrpEquipment.Equipment.Name) == 0)
                {
                    cargoContext.ShipGirlsEquipment.Add(wrpEquipment.Equipment);
                    await cargoContext.SaveChangesAsync();
                }
                else
                {
                    await cargoContext.Update(wrpEquipment.Equipment);
                }

                downloadBlock.Complete();
            }

            downloadBlock.Completion.Wait();
            Status = Statuses.DownloadComplete;
        }

        /// <summary>
        /// Get path to image folder that stores Equipment images.
        /// </summary>
        /// <param name="imageName">Name of image for saving</param>
        /// <returns>Path</returns>
        public override string GetImageFolder(string imageName) => EquipmentImagesFolderPath;
    }
}
