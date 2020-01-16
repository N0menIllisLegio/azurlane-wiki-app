using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace azurlane_wiki_app.Data
{
    class EquipmentJsonWrapper
    {
        [JsonProperty("title")]
        public Equipment Equipment { get; set; }
    }

    class Equipment
    {

    }
}
