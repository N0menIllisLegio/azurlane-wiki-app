using System.ComponentModel.DataAnnotations;

namespace azurlane_wiki_app.Data.Tables
{
    public class Icon
    {
        [Key]
        public string Name { get; set; }

        public string FileName { get; set; }
    }
}
