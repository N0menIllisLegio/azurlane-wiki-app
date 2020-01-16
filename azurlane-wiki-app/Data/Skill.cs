using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace azurlane_wiki_app.Data
{
    class SkillJsonWrapper
    {
        [JsonProperty("title")]
        public Skill Skill { get; set; }
    }

    class Skill
    {
        [Key]
        public int SkillID { get; set; }
        public int Num { get; set; }
        [Index]
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
    }
}
