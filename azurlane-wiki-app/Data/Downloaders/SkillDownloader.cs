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
    class SkillJsonWrapper
    {
        [JsonProperty("title")]
        public Skill Skill { get; set; }
    }

    class SkillDownloader : DataDownloader
    {
        private static readonly string SkillImagesFolderPath = ImagesFolderPath + "/SkillsIcons";
        private const string SkillFields = "ships.ShipID,ship_skills.Num,ship_skills.Name,ship_skills.Detail," +
                                           "ship_skills.Remodel,ship_skills.Type,ship_skills.Icon";

        public SkillDownloader(int ThreadsCount = 0) : base(ThreadsCount)
        {
            DownloadTitle = "Downloading Skills...";

            if (!Directory.Exists(SkillImagesFolderPath))
            {
                Directory.CreateDirectory(SkillImagesFolderPath);
            }
        }

        /// <summary>
        ///  Download all Skills and save them.
        /// </summary>
        public override async Task Download()
        {
            Status = Statuses.InProgress;
            StatusDataMessage = "Downloading data.";
            StatusImageMessage = "Pending.";
            List<SkillJsonWrapper> wrappedSkills;

            try
            {
                string responseJson = await GetData("ship_skills, ships", SkillFields, 
                    "ship_skills._pageName=ships._pageName", "");
                wrappedSkills = JsonConvert.DeserializeObject<List<SkillJsonWrapper>>(responseJson);
            }
            catch(JsonException)
            {
                Status = Statuses.ErrorInDeserialization;
                Logger.Write($"Failed to desirialize skills.", this.GetType().ToString());
                return;
            }
            catch
            {
                Status = Statuses.DownloadError;
                Logger.Write($"Failed to get data for skills from server.", this.GetType().ToString());
                return;
            }

            TotalImageCount = wrappedSkills.Count;
            TotalDataCount = wrappedSkills.Count;

            using (CargoContext cargoContext = new CargoContext())
            {
                StatusImageMessage = "Adding icons to download queue.";

                foreach (SkillJsonWrapper wrappedSkill in wrappedSkills)
                {
                    downloadBlock.Post(wrappedSkill.Skill.Icon);
                }

                downloadBlock.Complete();

                StatusImageMessage = "Downloading icons.";
                StatusDataMessage = "Saving data.";

                foreach (SkillJsonWrapper wrappedSkill in wrappedSkills)
                {
                    if (await cargoContext.Skills
                            .CountAsync(e => e.Name == wrappedSkill.Skill.Name) == 0)
                    {
                        wrappedSkill.Skill.FK_ShipGirl
                            = await cargoContext.ShipGirls.FindAsync(wrappedSkill.Skill.ShipID);
                        wrappedSkill.Skill.Detail = RemoveHTML(wrappedSkill.Skill.Detail);
                        SavePaths(wrappedSkill.Skill);
                        cargoContext.Skills.Add(wrappedSkill.Skill);
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
        /// Download one Skill by name and update it or save if it doesn't exist.
        /// </summary>
        /// <param name="id">Name of skill.</param>s
        public override async Task Download(string id)
        {
            Status = Statuses.InProgress;
            List<SkillJsonWrapper> wrappedSkills;

            try
            {
                string responseJson = await GetData("ship_skills, ships", SkillFields,
                    "ship_skills._pageName=ships._pageName", "ship_skills.Name=\'" + id + "\'");
                wrappedSkills = JsonConvert.DeserializeObject<List<SkillJsonWrapper>>(responseJson);
            }
            catch (JsonException)
            {
                Status = Statuses.ErrorInDeserialization;
                Logger.Write($"Failed to desirialize skill. ID: {id}", this.GetType().ToString());
                return;
            }
            catch
            {
                Status = Statuses.DownloadError;
                Logger.Write($"Failed to get data for skill from server. ID: {id}", this.GetType().ToString());
                return;
            }

            SkillJsonWrapper wrappedSkill = wrappedSkills.FirstOrDefault();

            if (wrappedSkill == null)
            {
                Status = Statuses.EmptyResponse;
                return;
            }

            TotalImageCount = wrappedSkills.Count;

            using (CargoContext cargoContext = new CargoContext())
            {
                downloadBlock.Post(wrappedSkill.Skill.Icon);
                downloadBlock.Complete();

                wrappedSkill.Skill.FK_ShipGirl
                    = await cargoContext.ShipGirls.FindAsync(wrappedSkill.Skill.ShipID);
                wrappedSkill.Skill.Detail = RemoveHTML(wrappedSkill.Skill.Detail);
                SavePaths(wrappedSkill.Skill);

                if (await cargoContext.Skills
                        .CountAsync(e => e.Name == wrappedSkill.Skill.Name) == 0)
                {
                    cargoContext.Skills.Add(wrappedSkill.Skill);
                    await cargoContext.SaveChangesAsync();
                }
                else
                {
                    await cargoContext.Update(wrappedSkill.Skill);
                }
            }

            //downloadBlock.Completion.Wait();
            Status = Statuses.DownloadComplete;
        }


        /// <summary>
        /// Download all Skills of ShipGirl and update them or save if they doesn't exist.
        /// </summary>
        /// <param name="shipGirl">ShipGirl which skills need update.</param>
        /// <param name="cargoContext">DB context</param>
        public async Task Download(ShipGirl shipGirl, CargoContext cargoContext)
        {
            Status = Statuses.InProgress;
            List<SkillJsonWrapper> wrappedSkills;
            
            try
            {
                string responseJson = await GetData("ship_skills, ships", SkillFields,
                    "ship_skills._pageName=ships._pageName", "ships.ShipID=\'" + shipGirl.ShipID + "\'");
                wrappedSkills = JsonConvert.DeserializeObject<List<SkillJsonWrapper>>(responseJson);
            }
            catch (JsonException)
            {
                Status = Statuses.ErrorInDeserialization;
                Logger.Write($"Failed to desirialize skills for shipgirl. Shipirl ID: {shipGirl.ShipID}", this.GetType().ToString());
                return;
            }
            catch
            {
                Status = Statuses.DownloadError;
                Logger.Write($"Failed to get skills data for shipgir from server. Shipgirl ID: {shipGirl.ShipID}", this.GetType().ToString());
                return;
            }

            if (wrappedSkills.Count == 0)
            {
                Status = Statuses.EmptyResponse;
                return;
            }

            TotalImageCount = wrappedSkills.Count;

            foreach (Skill skill in shipGirl.Skills)
            {
                string imagePath = GetImageFolder(skill.Icon) + "/" + skill.Icon;

                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
            }

            cargoContext.Skills.RemoveRange(shipGirl.Skills);
            await cargoContext.SaveChangesAsync();

            foreach (SkillJsonWrapper wrappedSkill in wrappedSkills)
            {
                downloadBlock.Post(wrappedSkill.Skill.Icon);
            }

            downloadBlock.Complete();

            foreach (SkillJsonWrapper wrappedSkill in wrappedSkills)
            {
                wrappedSkill.Skill.FK_ShipGirl = shipGirl;
                wrappedSkill.Skill.Detail = RemoveHTML(wrappedSkill.Skill.Detail);
                SavePaths(wrappedSkill.Skill);

                if (await cargoContext.Skills
                        .CountAsync(e => e.Name == wrappedSkill.Skill.Name) == 0)
                {
                    cargoContext.Skills.Add(wrappedSkill.Skill);
                    await cargoContext.SaveChangesAsync();
                }
                else
                {
                    await cargoContext.Update(wrappedSkill.Skill);
                }
            }

            downloadBlock.Completion.Wait();
            Status = Statuses.DownloadComplete;
        }

        /// <summary>
        /// Get path to image folder that stores skills icons.
        /// </summary>
        /// <param name="imageName">Name of image for saving</param>
        /// <returns>Path</returns>
        public override string GetImageFolder(string imageName) => SkillImagesFolderPath;

        /// <summary>
        /// Change Skill icon fields from image name to relative path to icon
        /// </summary>
        /// <param name="skill">Skill for changing</param>
        private void SavePaths(Skill skill)
        {
            skill.Icon = GetImageFolder(skill.Icon) + "/" + skill.Icon;
        }

        private string RemoveHTML(string skillDetails)
        {
            // ''' ... ''' - bold
            skillDetails = skillDetails.Replace("'''", "");

            // [[:File:...|...]] - remove
            Regex regex = new Regex(@"\[\[:File:[^]]*?\]\]");
            skillDetails = regex.Replace(skillDetails, "");

            // &lt;...&gt;
            regex = new Regex(@"\&lt\;.*?\&gt\;");
            skillDetails = regex.Replace(skillDetails, "  ");

            skillDetails = Regex.Replace(skillDetails, @"\[\[[^]]*?\|([^]]*?)\]\]", "$1");

            skillDetails = skillDetails.Replace("[", "");
            skillDetails = skillDetails.Replace("]", "");

            return skillDetails;
        }
    }
}
