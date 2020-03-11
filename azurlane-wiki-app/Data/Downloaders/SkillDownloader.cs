using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using azurlane_wiki_app.Data.Tables;
using Newtonsoft.Json;

namespace azurlane_wiki_app.Data.Downloaders
{
    class SkillDownloader : DataDownloader
    {
        private readonly string SkillImagesFolderPath = ImagesFolderPath + "/SkillsIcons";
        private const string SkillFields = "ships.ShipID,ship_skills.Num,ship_skills.Name,ship_skills.Detail," +
                                           "ship_skills.Remodel,ship_skills.Type,ship_skills.Icon";

        public SkillDownloader(int ThreadsCount = 0) : base(ThreadsCount)
        {
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
                return;
            }
            catch
            {
                Status = Statuses.DownloadError;
                return;
            }

            TotalImageCount = wrappedSkills.Count;

            using (CargoContext cargoContext = new CargoContext())
            {
                foreach (SkillJsonWrapper wrappedSkill in wrappedSkills)
                {
                    downloadBlock.Post(wrappedSkill.Skill.Icon);

                    if (await cargoContext.Skills
                            .CountAsync(e => e.Name == wrappedSkill.Skill.Name) == 0)
                    {
                        wrappedSkill.Skill.FK_ShipGirl
                            = await cargoContext.ShipGirls.FindAsync(wrappedSkill.Skill.ShipID);
                        cargoContext.Skills.Add(wrappedSkill.Skill);
                    }
                }

                downloadBlock.Complete();
                await cargoContext.SaveChangesAsync();
            }

            downloadBlock.Completion.Wait();
            Status = Statuses.DownloadComplete;
        }

        /// <summary>
        /// Download one Skill by name and update it or save if it doesn't exist.
        /// </summary>
        /// <param name="id">Name of skill.</param>
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
                return;
            }
            catch
            {
                Status = Statuses.DownloadError;
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

                wrappedSkill.Skill.FK_ShipGirl
                    = await cargoContext.ShipGirls.FindAsync(wrappedSkill.Skill.ShipID);

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
                
                downloadBlock.Complete();
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
    }
}
