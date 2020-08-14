using azurlane_wiki_app.PageEquipmentList.Items;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace azurlane_wiki_app.Data.Downloaders
{
    class DPSDataDownloader
    {
        private List<ArmourModifier> ArmourModifiers = new List<ArmourModifier>();
        private List<BombStats> BombDLs = new List<BombStats>();
        private List<GunStats> GunsStats = new List<GunStats>();
        private Dictionary<string, float> Cooldowns = new Dictionary<string, float>();

        public async Task DownloadDPSData()
        {
            string page = await GetPage();
            ParsePage(page);
            SaveData();
            Seed();
        }

        private async Task<string> GetPage()
        {
            string requestUrl = @"https://azurlane.koumakan.jp/Module:EquipmentList";
            string responseHtml = "";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();

            using (Stream stream = response?.GetResponseStream())
            {
                if (stream != null)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        responseHtml = await reader.ReadToEndAsync();
                    }
                }
            }

            return responseHtml;
        }

        private void ParsePage(string page)
        {
            page = Regex.Replace(page, @"--.*?\n", "\n");
            page = page.Replace("\n", "");
            page = page.Replace("\t", "");

            Regex regex = new Regex(@"modifiers = {(.+),}bombDmg = {(.+),}gunTbl = {(.+),}local cooldown = {(.+?)}");
            Match match = regex.Match(page);

            string modifiers = match.Groups[1].Value;
            string bombs = match.Groups[2].Value;
            string guns = match.Groups[3].Value;
            string cooldowns = match.Groups[4].Value;

            Task[] tasks = new Task[4];

            tasks[0] = Task.Factory.StartNew(() => ParseModifiers(modifiers));
            tasks[1] = Task.Factory.StartNew(() => ParseBombs(bombs));
            tasks[2] = Task.Factory.StartNew(() => ParseGuns(guns));
            tasks[3] = Task.Factory.StartNew(() => ParseCooldowns(cooldowns));

            Task.WaitAll(tasks);
        }

        private void ParseModifiers(string modifiers)
        {
            Regex regex = new Regex(@"\[\'(.+?)\'\]\s*=\s*{(.*?),}");
            Match weapon = regex.Match(modifiers);

            while (weapon.Success)
            {
                string weaponName = weapon.Groups[1].Value;

                regex = new Regex(@"\[\'(.+?)\'\]=\s*{L\s*=\s*([0-9]{1,3}),\s*M\s*=\s*([0-9]{1,3}),\s*H\s*=\s*([0-9]{1,3})}");
                Match ammo = regex.Match(weapon.Groups[2].Value);

                while (ammo.Success)
                {
                    ArmourModifier armourModifier = new ArmourModifier();
                    armourModifier.WeaponName = weaponName;
                    armourModifier.AmmoType = ammo.Groups[1].Value;

                    armourModifier.Light = int.Parse(ammo.Groups[2].Value) / 100D;
                    armourModifier.Medium = int.Parse(ammo.Groups[3].Value) / 100D;
                    armourModifier.Heavy = int.Parse(ammo.Groups[4].Value) / 100D;

                    ArmourModifiers.Add(armourModifier);

                    ammo = ammo.NextMatch();
                }

                weapon = weapon.NextMatch();
            }

            if (ArmourModifiers.Count == 0)
            {
                Logger.Write($"Error in parsing ArmourModifiers for DPS calculations.", this.GetType().ToString());
            }
        }

        private void ParseBombs(string bombs)
        {
            Regex regex = new Regex(@"\[\'(.+?)\'\]\s*=\s*{(.+?),}");
            Match bomb = regex.Match(bombs);

            while (bomb.Success)
            {
                string bombName = bomb.Groups[1].Value;

                regex = new Regex(@"(T[0-9])\s*=\s*{Dmg\s*=\s*([0-9]{1,3}),\s*Dmg3\s*=\s*([0-9]{1,3}),\s*Dmg6\s*=\s*([0-9]{1,3}),\s*DmgMax\s*=\s*([0-9]{1,3})}");
                Match tier = regex.Match(bomb.Groups[2].Value);

                while (tier.Success)
                {
                    BombStats bombDamageLevels = new BombStats();
                    bombDamageLevels.BombName = bombName;
                    bombDamageLevels.BombTech = tier.Groups[1].Value;

                    bombDamageLevels.Dmg = int.Parse(tier.Groups[2].Value);
                    bombDamageLevels.Dmg3 = int.Parse(tier.Groups[3].Value);
                    bombDamageLevels.Dmg6 = int.Parse(tier.Groups[4].Value);
                    bombDamageLevels.DmgMax = int.Parse(tier.Groups[5].Value);

                    BombDLs.Add(bombDamageLevels);

                    tier = tier.NextMatch();
                }

                bomb = bomb.NextMatch();
            }

            if (BombDLs.Count == 0)
            {
                Logger.Write($"Error in parsing bombs for DPS calculations.", this.GetType().ToString());
            }
        }

        private void ParseGuns(string guns)
        {
            Regex regex = new Regex(@"\[\'(.+?)\'\]\s*=\s*{(.+?)}}");
            Match gun = regex.Match(guns);

            while (gun.Success)
            {
                string gunName = gun.Groups[1].Value;

                regex = new Regex(@"(T[0-9])\s*=\s*{Rld=\s*([0-9]{1,3}),\s*Rld3=\s*([0-9]{1,3}),\s*Rld6=\s*([0-9]{1,3}),\s*RldMax=\s*([0-9]{1,3}),\s*Dmg=\s*([0-9]{1,3}),\s*Dmg3=\s*([0-9]{1,3}),\s*Dmg6=\s*([0-9]{1,3}),\s*DmgMax=\s*([0-9]{1,3})}?");
                Match tier = regex.Match(gun.Groups[2].Value);

                while (tier.Success)
                {
                    GunStats gunStats = new GunStats();
                    gunStats.GunName = gunName;
                    gunStats.GunTech = tier.Groups[1].Value;

                    gunStats.Rld = int.Parse(tier.Groups[2].Value);
                    gunStats.Rld3 = int.Parse(tier.Groups[3].Value);
                    gunStats.Rld6 = int.Parse(tier.Groups[4].Value);
                    gunStats.RldMax = int.Parse(tier.Groups[5].Value);

                    gunStats.Dmg = int.Parse(tier.Groups[6].Value);
                    gunStats.Dmg3 = int.Parse(tier.Groups[7].Value);
                    gunStats.Dmg6 = int.Parse(tier.Groups[8].Value);
                    gunStats.DmgMax = int.Parse(tier.Groups[9].Value);

                    GunsStats.Add(gunStats);

                    tier = tier.NextMatch();
                }

                gun = gun.NextMatch();
            }

            if (GunsStats.Count == 0)
            {
                Logger.Write($"Error in parsing guns for DPS calculations.", this.GetType().ToString());
            }
        }

        private void ParseCooldowns(string cooldowns)
        {
            Regex regex = new Regex(@"\[\'(.+?)\'\]\s*=\s*([0-9\.]{4})");
            Match match = regex.Match(cooldowns);

            while (match.Success)
            {
                float cooldown = float.Parse(match.Groups[2].Value);
                Cooldowns.Add(match.Groups[1].Value, cooldown);

                match = match.NextMatch();
            }

            if (Cooldowns.Count == 0)
            {
                Logger.Write($"Error in parsing cooldowns for DPS calculations.", this.GetType().ToString());
            }
        }
    
        private void SaveData()
        {
            using (CargoContext cargoContext = new CargoContext())
            {
                cargoContext.ClearDPSDataTables();

                cargoContext.ArmourModifiers.AddRange(ArmourModifiers);
                cargoContext.BombsStats.AddRange(BombDLs);
                cargoContext.GunsStats.AddRange(GunsStats);

                foreach (var key in Cooldowns.Keys)
                {
                    var item = cargoContext.EquipmentTypes.Find(key);

                    if (item != null)
                    {
                        item.Cooldown = Cooldowns[key];
                    }
                }

                cargoContext.SaveChanges();
            }
        }

        private void Seed()
        {
            using (CargoContext cargoContext = new CargoContext())
            {
                List<GunStats> gunStats = cargoContext.GunsStats.Where(gs => gs.GunName == "2 x 7.7mm MG GL.2").ToList();

                if (gunStats == null || gunStats.Count == 0)
                {
                    GunStats gun = new GunStats();

                    gun.GunName = "2 x 7.7mm MG GL.2";
                    gun.GunTech = "T0";
                    gun.Rld = 75;
                    gun.Rld3 = 70;
                    gun.Rld6 = 66;
                    gun.RldMax = 60;

                    gun.Dmg = 9;
                    gun.Dmg3 = 12;
                    gun.Dmg6 = 17;
                    gun.DmgMax = 22;

                    cargoContext.GunsStats.Add(gun);
                }

                cargoContext.SaveChanges();
            }
        }
    }
}
