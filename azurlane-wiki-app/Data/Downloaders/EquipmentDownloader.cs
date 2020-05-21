using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using azurlane_wiki_app.Data.Tables;
using Newtonsoft.Json;

namespace azurlane_wiki_app.Data.Downloaders
{
    class EquipmentDownloader : DataDownloader
    {
        private static readonly string EquipmentImagesFolderPath = ImagesFolderPath + "/Equipment";
        private const string EquipmentFields = "Name,Image,Type,Stars,Nationality,Tech,Health,HealthMax,Torpedo,TorpMax,Firepower,FPMax," +
                                               "Aviation,AvMax,Evasion,EvasionMax,PlaneHP,PlaneHPMax,Reload,ReloadMax,ASW,ASWMax,Oxygen," +
                                               "OxygenMax,AA,AAMax,Luck,LuckMax,Acc,AccMax,Spd,SpdMax,Damage,DamageMax,RoF,RoFMax,Number," +
                                               "Spread,Angle,WepRange,Shells,Salvoes,Characteristic,PingFreq,VolleyTime,Coef,Ammo,AAGun1," +
                                               "AAGun2,Bombs1,Bombs2,DD,DDNote,CL,CLNote,CA,CANote,CB,CBNote,BM,BMNote,BB,BBNote,BC,BCNote," +
                                               "BBV,BBVNote,CV,CVNote,CVL,CVLNote,AR,ARNote,SS,SSNote,SSV,SSVNote,DropLocation,Notes";


        public EquipmentDownloader(int threadsCount = 0) : base(threadsCount)
        {
            DownloadTitle = "Downloading Equipment...";

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
            StatusDataMessage = "Downloading data.";
            StatusImageMessage = "Pending.";
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

            TotalDataCount = wrappedEquipment.Count;
            TotalImageCount = wrappedEquipment.Count;

            using (CargoContext cargoContext = new CargoContext())
            {
                StatusImageMessage = "Adding images to download queue.";

                foreach (EquipmentJsonWrapper wrpEquipment in wrappedEquipment)
                {
                    downloadBlock.Post(wrpEquipment.Equipment.Image);
                }

                downloadBlock.Complete();

                StatusImageMessage = "Downloading images.";
                StatusDataMessage = "Saving data.";

                foreach (EquipmentJsonWrapper wrpEquipment in wrappedEquipment)
                {
                    if (await cargoContext.ShipGirlsEquipment
                            .CountAsync(e => e.Name == wrpEquipment.Equipment.Name 
                                             && e.Stars == wrpEquipment.Equipment.Stars) == 0)
                    {
                        Nationality nationality = cargoContext.Nationalities.Find(wrpEquipment.Equipment.Nationality);

                        if (nationality == null)
                        {
                            nationality = new Nationality
                            {
                                Name = wrpEquipment.Equipment.Nationality,
                                FK_Icon = cargoContext.Icons.Find(wrpEquipment.Equipment.Nationality)
                            };

                            cargoContext.Nationalities.Add(nationality);
                            cargoContext.SaveChanges();
                        }

                        wrpEquipment.Equipment.FK_Nationality = nationality;
                        CreateRelationships(wrpEquipment.Equipment, cargoContext);
                        SavePaths(wrpEquipment.Equipment);
                        wrpEquipment.Equipment.DropLocation = Refactor(wrpEquipment.Equipment.DropLocation);
                        wrpEquipment.Equipment.Notes = Refactor(wrpEquipment.Equipment.Notes);
                        wrpEquipment.Equipment.Name = Refactor(wrpEquipment.Equipment.Name);
                        cargoContext.ShipGirlsEquipment.Add(wrpEquipment.Equipment);
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
            // downloadBlock.Completion.ContinueWith(e => { Status = Statuses.DownloadComplete; });
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
                downloadBlock.Complete();

                Nationality nationality = cargoContext.Nationalities.Find(wrpEquipment.Equipment.Nationality);

                if (nationality == null)
                {
                    nationality = new Nationality
                    {
                        Name = wrpEquipment.Equipment.Nationality,
                        FK_Icon = cargoContext.Icons.Find(wrpEquipment.Equipment.Nationality)
                    };

                    cargoContext.Nationalities.Add(nationality);
                    cargoContext.SaveChanges();
                }

                wrpEquipment.Equipment.FK_Nationality = nationality;
                CreateRelationships(wrpEquipment.Equipment, cargoContext);
                wrpEquipment.Equipment.DropLocation = Refactor(wrpEquipment.Equipment.DropLocation);
                wrpEquipment.Equipment.Notes = Refactor(wrpEquipment.Equipment.Notes);
                wrpEquipment.Equipment.Name = Refactor(wrpEquipment.Equipment.Name);
                SavePaths(wrpEquipment.Equipment);

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
            }

            //downloadBlock.Completion.Wait();
            Status = Statuses.DownloadComplete;
        }

        /// <summary>
        /// Get path to image folder that stores Equipment images.
        /// </summary>
        /// <param name="imageName">Name of image for saving</param>
        /// <returns>Path</returns>
        public override string GetImageFolder(string imageName) => EquipmentImagesFolderPath;

        /// <summary>
        /// Change Equipment image fields from image name to relative path to image
        /// </summary>
        /// <param name="equipment">Equipment for changing</param>
        private void SavePaths(Equipment equipment)
        {
            equipment.Image = GetImageFolder(equipment.Image) + "/" + equipment.Image;
        }

        /// <summary>
        /// Create relationships depending on values of Equipment.{ Tech, Type, Stars }.
        /// If { Tech, Type, Stars } doesn't exists, creates it.
        /// </summary>
        /// <param name="equipment">Piece of equipment</param>
        /// <param name="cargoContext">CargoContext</param>
        private void CreateRelationships(Equipment equipment, CargoContext cargoContext)
        {
            EquipmentTech equipmentTech = cargoContext.EquipmentTeches.Find(equipment.Tech);
            EquipmentType equipmentType = cargoContext.EquipmentTypes.Find(equipment.Type);

            if (equipmentType == null)
            {
                equipmentType = new EquipmentType {Name = equipment.Type};
                cargoContext.EquipmentTypes.Add(equipmentType);
            }

            if (equipmentTech == null)
            {
                equipmentTech = new EquipmentTech {Name = equipment.Tech};
                cargoContext.EquipmentTeches.Add(equipmentTech);
            }

            equipment.FK_Tech = equipmentTech;
            equipment.FK_Type = equipmentType;

            cargoContext.SaveChanges();
        }

        private string Refactor(string data)
        {
            // &quot; - "
            data = data.Replace("&quot;", "\"");

            // [[File:...|...]] - remove
            Regex regex = new Regex(@"\[\[File:[^]]*?\]\]");
            data = regex.Replace(data, "");

            // &lt;...&gt;
            regex = new Regex(@"\&lt\;.*?\&gt\;");
            data = regex.Replace(data, "  ");

            data = Regex.Replace(data, @"\[\[[^]]*?\|([^]]*?)\]\]", "$1");

            data = data.Replace(":[[", "\t");

            data = data.Replace("[", "");
            data = data.Replace("]", "");

            return data;
        }
    }
}
