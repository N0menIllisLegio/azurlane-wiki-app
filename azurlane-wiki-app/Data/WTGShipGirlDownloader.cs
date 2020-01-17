using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using azurlane_wiki_app.Data.Tables;

namespace azurlane_wiki_app.Data
{
    class WTGShipGirlDownloader : DataDownloader
    {
        #region Schemas

        private class WTGShipGirlJsonWrapper
        {
            [JsonProperty("title")]
            public WTGShipGirlJson WtgShipGirlJson { get; set; }
        }

        private class WTGShipGirlJson
        {
            // ShipID
            public string ID { get; set; }

            // Construction
            [JsonProperty("Light")]
            public string Light { get; set; }
            [JsonProperty("Heavy")]
            public string Heavy { get; set; }
            [JsonProperty("Aviation")]
            public string Aviation { get; set; }
            [JsonProperty("Limited")]
            public string Limited { get; set; }
            [JsonProperty("Exchange")]
            public string Exchange { get; set; }
            [JsonProperty("Collection")]
            public string Collection { get; set; }
            [JsonProperty("Event")]
            public string Event { get; set; }

            public string LightNote { get; set; }
            public string HeavyNote { get; set; }
            public string AviationNote { get; set; }
            public string LimitedNote { get; set; }
            public string ExchangeNote { get; set; }
            public string CollectionNote { get; set; }
            public string EventNote { get; set; }

            // Drop
            [JsonProperty("1-1")]
            public string node11 { get; set; }
            [JsonProperty("1-2")]
            public string node12 { get; set; }
            [JsonProperty("1-3")]
            public string node13 { get; set; }
            [JsonProperty("1-4")]
            public string node14 { get; set; }
            [JsonProperty("2-1")]
            public string node21 { get; set; }
            [JsonProperty("2-2")]
            public string node22 { get; set; }
            [JsonProperty("2-3")]
            public string node23 { get; set; }
            [JsonProperty("2-4")]
            public string node24 { get; set; }
            [JsonProperty("3-1")]
            public string node31 { get; set; }
            [JsonProperty("3-2")]
            public string node32 { get; set; }
            [JsonProperty("3-3")]
            public string node33 { get; set; }
            [JsonProperty("3-5")]
            public string node35 { get; set; }
            [JsonProperty("3-4")]
            public string node34 { get; set; }
            [JsonProperty("4-1")]
            public string node41 { get; set; }
            [JsonProperty("4-2")]
            public string node42 { get; set; }
            [JsonProperty("4-3")]
            public string node43 { get; set; }
            [JsonProperty("4-4")]
            public string node44 { get; set; }
            [JsonProperty("4-5")]
            public string node45 { get; set; }
            [JsonProperty("5-1")]
            public string node51 { get; set; }
            [JsonProperty("5-2")]
            public string node52 { get; set; }
            [JsonProperty("5-3")]
            public string node53 { get; set; }
            [JsonProperty("5-4")]
            public string node54 { get; set; }
            [JsonProperty("5-5")]
            public string node55 { get; set; }
            [JsonProperty("6-1")]
            public string node61 { get; set; }
            [JsonProperty("6-2")]
            public string node62 { get; set; }
            [JsonProperty("6-3")]
            public string node63 { get; set; }
            [JsonProperty("6-4")]
            public string node64 { get; set; }
            [JsonProperty("6-5")]
            public string node65 { get; set; }
            [JsonProperty("7-1")]
            public string node71 { get; set; }
            [JsonProperty("7-2")]
            public string node72 { get; set; }
            [JsonProperty("7-3")]
            public string node73 { get; set; }
            [JsonProperty("7-4")]
            public string node74 { get; set; }
            [JsonProperty("7-5")]
            public string node75 { get; set; }
            [JsonProperty("8-1")]
            public string node81 { get; set; }
            [JsonProperty("8-2")]
            public string node82 { get; set; }
            [JsonProperty("8-3")]
            public string node83 { get; set; }
            [JsonProperty("8-4")]
            public string node84 { get; set; }
            [JsonProperty("8-5")]
            public string node85 { get; set; }
            [JsonProperty("9-1")]
            public string node91 { get; set; }
            [JsonProperty("9-2")]
            public string node92 { get; set; }
            [JsonProperty("9-3")]
            public string node93 { get; set; }
            [JsonProperty("9-4")]
            public string node94 { get; set; }
            [JsonProperty("9-5")]
            public string node95 { get; set; }
            [JsonProperty("10-1")]
            public string node101 { get; set; }
            [JsonProperty("10-2")]
            public string node102 { get; set; }
            [JsonProperty("10-3")]
            public string node103 { get; set; }
            [JsonProperty("10-4")]
            public string node104 { get; set; }
            [JsonProperty("10-5")]
            public string node105 { get; set; }
            [JsonProperty("11-1")]
            public string node111 { get; set; }
            [JsonProperty("11-2")]
            public string node112 { get; set; }
            [JsonProperty("11-3")]
            public string node113 { get; set; }
            [JsonProperty("11-4")]
            public string node114 { get; set; }
            [JsonProperty("12-1")]
            public string node121 { get; set; }
            [JsonProperty("12-2")]
            public string node122 { get; set; }
            [JsonProperty("12-3")]
            public string node123 { get; set; }
            [JsonProperty("12-4")]
            public string node124 { get; set; }
            [JsonProperty("13-1")]
            public string node131 { get; set; }
            [JsonProperty("13-2")]
            public string node132 { get; set; }
            [JsonProperty("13-3")]
            public string node133 { get; set; }
            [JsonProperty("13-4")]
            public string node134 { get; set; }

            [JsonProperty("1-1note")]
            public string note11 { get; set; }
            [JsonProperty("1-2note")]
            public string note12 { get; set; }
            [JsonProperty("1-3note")]
            public string note13 { get; set; }
            [JsonProperty("1-4note")]
            public string note14 { get; set; }
            [JsonProperty("2-1note")]
            public string note21 { get; set; }
            [JsonProperty("2-2note")]
            public string note22 { get; set; }
            [JsonProperty("2-3note")]
            public string note23 { get; set; }
            [JsonProperty("2-4note")]
            public string note24 { get; set; }
            [JsonProperty("3-1note")]
            public string note31 { get; set; }
            [JsonProperty("3-2note")]
            public string note32 { get; set; }
            [JsonProperty("3-3note")]
            public string note33 { get; set; }
            [JsonProperty("3-4note")]
            public string note34 { get; set; }
            [JsonProperty("3-5note")]
            public string note35 { get; set; }
            [JsonProperty("4-1note")]
            public string note41 { get; set; }
            [JsonProperty("4-2note")]
            public string note42 { get; set; }
            [JsonProperty("4-3note")]
            public string note43 { get; set; }
            [JsonProperty("4-4note")]
            public string note44 { get; set; }
            [JsonProperty("4-5note")]
            public string note45 { get; set; }
            [JsonProperty("5-1note")]
            public string note51 { get; set; }
            [JsonProperty("5-2note")]
            public string note52 { get; set; }
            [JsonProperty("5-3note")]
            public string note53 { get; set; }
            [JsonProperty("5-4note")]
            public string note54 { get; set; }
            [JsonProperty("5-5note")]
            public string note55 { get; set; }
            [JsonProperty("6-1note")]
            public string note61 { get; set; }
            [JsonProperty("6-2note")]
            public string note62 { get; set; }
            [JsonProperty("6-3note")]
            public string note63 { get; set; }
            [JsonProperty("6-4note")]
            public string note64 { get; set; }
            [JsonProperty("6-5note")]
            public string note65 { get; set; }
            [JsonProperty("7-1note")]
            public string note71 { get; set; }
            [JsonProperty("7-2note")]
            public string note72 { get; set; }
            [JsonProperty("7-3note")]
            public string note73 { get; set; }
            [JsonProperty("7-4note")]
            public string note74 { get; set; }
            [JsonProperty("7-5note")]
            public string note75 { get; set; }
            [JsonProperty("8-1note")]
            public string note81 { get; set; }
            [JsonProperty("8-2note")]
            public string note82 { get; set; }
            [JsonProperty("8-3note")]
            public string note83 { get; set; }
            [JsonProperty("8-4note")]
            public string note84 { get; set; }
            [JsonProperty("8-5note")]
            public string note85 { get; set; }
            [JsonProperty("9-1note")]
            public string note91 { get; set; }
            [JsonProperty("9-2note")]
            public string note92 { get; set; }
            [JsonProperty("9-3note")]
            public string note93 { get; set; }
            [JsonProperty("9-4note")]
            public string note94 { get; set; }
            [JsonProperty("9-5note")]
            public string note95 { get; set; }
            [JsonProperty("10-1note")]
            public string note101 { get; set; }
            [JsonProperty("10-2note")]
            public string note102 { get; set; }
            [JsonProperty("10-3note")]
            public string note103 { get; set; }
            [JsonProperty("10-4note")]
            public string note104 { get; set; }
            [JsonProperty("10-5note")]
            public string note105 { get; set; }
            [JsonProperty("11-1note")]
            public string note111 { get; set; }
            [JsonProperty("11-2note")]
            public string note112 { get; set; }
            [JsonProperty("11-3note")]
            public string note113 { get; set; }
            [JsonProperty("11-4note")]
            public string note114 { get; set; }
            [JsonProperty("12-1note")]
            public string note121 { get; set; }
            [JsonProperty("12-2note")]
            public string note122 { get; set; }
            [JsonProperty("12-3note")]
            public string note123 { get; set; }
            [JsonProperty("12-4note")]
            public string note124 { get; set; }
            [JsonProperty("13-1note")]
            public string note131 { get; set; }
            [JsonProperty("13-2note")]
            public string note132 { get; set; }
            [JsonProperty("13-3note")]
            public string note133 { get; set; }
            [JsonProperty("13-4note")]
            public string note134 { get; set; }


            /// <summary>
            /// Get names and notes of shipgirl's drop location or construction pool.
            /// </summary>
            /// <returns>Key = location, value = note</returns>
            public Dictionary<string, string> GetDrops()
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();

                var properties = GetType()
                    .GetProperties()
                    .Where(pi => pi.PropertyType == typeof(string) && pi.GetGetMethod() != null)
                    .Select(pi =>
                    {
                        string attributeName = "";

                        if (pi.CustomAttributes.Any())
                        {
                            attributeName = pi.CustomAttributes.ElementAt(0)?.ConstructorArguments[0].Value
                                .ToString() ?? "";
                        }

                        return new
                        {
                            pi.Name,
                            Attribute = attributeName,
                            Value = pi.GetGetMethod().Invoke(this, null)
                        };
                    });

                foreach (var property in properties)
                {
                    if (property.Value.ToString().Equals("t"))
                    {
                        string searchField = property.Name.Replace("node", "");

                        var notes = properties.Where(p => p.Name.Contains(searchField) 
                                                                && (p.Name.Contains("note") || p.Name.Contains("Note")));

                        string note = notes.ElementAt(0)?.Value.ToString();

                        dictionary.Add(property.Attribute, note ?? "");
                    }
                }

                return dictionary;
            }
        }

        #endregion

        public override async Task Download()
        {
            string fields = "ID,Light,Heavy,Aviation,Limited,Exchange,Collection,Event,1-1,1-2,1-3,1-4,2-1,2-2,2-3," +
                            "2-4,3-1,3-2,3-3,3-5,3-4,4-1,4-2,4-3,4-4,4-5,5-1,5-2,5-3,5-4,5-5,6-1,6-2,6-3,6-4,6-5,7-1," +
                            "7-2,7-3,7-4,7-5,8-1,8-2,8-3,8-4,8-5,9-1,9-2,9-3,9-4,9-5,10-1,10-2,10-3,10-4,10-5,11-1," +
                            "11-2,11-3,11-4,12-1,12-2,12-3,12-4,13-1,13-2,13-3,13-4,LightNote,HeavyNote,AviationNote," +
                            "LimitedNote,ExchangeNote,CollectionNote,EventNote,1-1note,1-2note,1-3note,1-4note,2-1note," +
                            "2-2note,2-3note,2-4note,3-1note,3-2note,3-3note,3-4note,3-5note,4-1note,4-2note,4-3note," +
                            "4-4note,4-5note,5-1note,5-2note,5-3note,5-4note,5-5note,6-1note,6-2note,6-3note,6-4note," +
                            "6-5note,7-1note,7-2note,7-3note,7-4note,7-5note,8-1note,8-2note,8-3note,8-4note,8-5note," +
                            "9-1note,9-2note,9-3note,9-4note,9-5note,10-1note,10-2note,10-3note,10-4note,10-5note," +
                            "11-1note,11-2note,11-3note,11-4note,12-1note,12-2note,12-3note,12-4note,13-1note,13-2note," +
                            "13-3note,13-4note";
            string responseJson = await GetData("shipDrops", fields);

            List<WTGShipGirlJsonWrapper> wrappedDrops;

            try
            {
                wrappedDrops = JsonConvert.DeserializeObject<List<WTGShipGirlJsonWrapper>>(responseJson);
            }
            catch
            {
                // TODO: Add error display
                return;
            }

            using (CargoContext cargoContext = new CargoContext())
            {
                foreach (WTGShipGirlJsonWrapper wrappedDrop in wrappedDrops)
                {
                    ShipGirl shipGirl = cargoContext.ShipGirls.Find(wrappedDrop.WtgShipGirlJson.ID);

                    if (shipGirl != null)
                    {
                        Dictionary<string,string> dictionary = wrappedDrop.WtgShipGirlJson.GetDrops();

                        if (dictionary.Count > 0)
                        {
                            foreach (string key in dictionary.Keys)
                            {
                                WhereToGetShipGirl whereToGetShipGirl = cargoContext.WhereToGetShipGirls.Find(key);
                                cargoContext.CreateRelationshipGirlDrop(whereToGetShipGirl, shipGirl, dictionary[key]);
                            }
                        }
                    }
                }
            }
        }



        public override string GetImageFolder(string imageName)
        {
            return ImagesFolderPath;
        }
    }
}
