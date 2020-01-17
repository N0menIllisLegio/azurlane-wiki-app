using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace azurlane_wiki_app.Data.Tables
{
    class SkillJsonWrapper
    {
        [JsonProperty("title")]
        public Skill Skill { get; set; }
    }

    public class Skill
    {
        [Key]
        public int SkillID { get; set; }

        public int Num { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(2000)]
        public string Detail { get; set; }
        [MaxLength(5)]
        public string Remodel { get; set; }
        [MaxLength(30)]
        public string Type { get; set; }
        [MaxLength(150)]
        public string Icon { get; set; }

        [Index]
        [MaxLength(40)]
        public string ShipID { get; set; }
        public virtual ShipGirl FK_ShipGirl { get; set; }
    }
}
