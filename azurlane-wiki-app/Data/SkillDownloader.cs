using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using azurlane_wiki_app.Data.Tables;

namespace azurlane_wiki_app.Data
{
    class SkillDownloader : DataDownloader
    {
        private string SkillImagesFolderPath = ImagesFolderPath + "/SkillsIcons";

        public SkillDownloader() : base()
        {
            if (!Directory.Exists(SkillImagesFolderPath))
            {
                Directory.CreateDirectory(SkillImagesFolderPath);
            }
        }

        public override async Task Download()
        {
            Status = Statuses.InProgress;

            string skillFields = "ships.ShipID,ship_skills.Num,ship_skills.Name,ship_skills.Detail," +
                                 "ship_skills.Remodel,ship_skills.Type,ship_skills.Icon";

            string joinOn = "ship_skills._pageName=ships._pageName";

            string responseJson = await GetData("ship_skills, ships", skillFields, joinOn);

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

            using (CargoContext cargoContext = new CargoContext())
            {
                TotalImageCount = wrappedSkills.Count;
                foreach (SkillJsonWrapper wrappedSkill in wrappedSkills)
                {
                    await DownloadImage(wrappedSkill.Skill.Icon);

                    if (cargoContext.Skills.Count(e => e.Name == wrappedSkill.Skill.Name) == 0)
                    {
                        wrappedSkill.Skill.FK_ShipGirl 
                            = cargoContext.ShipGirls.Find(wrappedSkill.Skill.ShipID);
                        cargoContext.Skills.Add(wrappedSkill.Skill);
                    }
                }

                cargoContext.SaveChanges();
            }

            Status = Statuses.DownloadComplete;
        }

        public override string GetImageFolder(string imageName)
        {
            return SkillImagesFolderPath;
        }
    }
}
