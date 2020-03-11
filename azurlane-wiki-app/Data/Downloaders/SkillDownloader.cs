using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Threading.Tasks;
using azurlane_wiki_app.Data.Tables;
using Newtonsoft.Json;

namespace azurlane_wiki_app.Data.Downloaders
{
    class SkillDownloader : DataDownloader
    {
        private string SkillImagesFolderPath = ImagesFolderPath + "/SkillsIcons";

        public SkillDownloader(int ThreadsCount = 0) : base(ThreadsCount)
        {
            if (!Directory.Exists(SkillImagesFolderPath))
            {
                Directory.CreateDirectory(SkillImagesFolderPath);
            }
        }

        /*
         * Надо разделить скачивание картинок и текстовых данных и связывание таблиц (хотя с ВТГ так не покатит). 
         */

        public override async Task Download()
        {
            Status = Statuses.InProgress;
            string responseJson;
            
            try
            {
                string skillFields = "ships.ShipID,ship_skills.Num,ship_skills.Name,ship_skills.Detail," +
                                     "ship_skills.Remodel,ship_skills.Type,ship_skills.Icon";
                string joinOn = "ship_skills._pageName=ships._pageName";

                responseJson = await GetData("ship_skills, ships", skillFields, joinOn, "");
            }
            catch
            {
                Status = Statuses.DownloadError;
                return;
            }

            List<SkillJsonWrapper> wrappedSkills;

            try
            {
                wrappedSkills = JsonConvert.DeserializeObject<List<SkillJsonWrapper>>(responseJson);
            }
            catch
            {
                Status = Statuses.ErrorInDeserialization;
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

        public override Task Download(string id)
        {
            throw new System.NotImplementedException();
        }

        public override string GetImageFolder(string imageName)
        {
            return SkillImagesFolderPath;
        }
    }
}
