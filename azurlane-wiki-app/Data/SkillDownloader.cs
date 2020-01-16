using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        public override async void Download()
        {
            string skillFields = "Num,Name,Detail,Remodel,Type,Icon";
            string responseJson = await GetData("ship_skills", skillFields);

            List<SkillJsonWrapper> wrappedSkills;

            try
            {
                wrappedSkills = JsonConvert.DeserializeObject<List<SkillJsonWrapper>>(responseJson);
            }
            catch
            {
                // TODO: Add error display
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
                        cargoContext.Skills.Add(wrappedSkill.Skill);
                    }
                }

                cargoContext.SaveChanges();
            }
        }

        public override string GetImageFolder(string imageName)
        {
            return SkillImagesFolderPath;
        }
    }
}
