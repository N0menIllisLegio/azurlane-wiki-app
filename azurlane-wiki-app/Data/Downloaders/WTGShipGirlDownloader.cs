using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using azurlane_wiki_app.Data.Tables;
using Newtonsoft.Json;

namespace azurlane_wiki_app.Data.Downloaders
{
    /// <summary>
    /// Where To Get Ship Girl
    /// </summary>
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

            [JsonProperty("LightNote")]
            public string LightNote { get; set; }
            [JsonProperty("HeavyNote")]
            public string HeavyNote { get; set; }
            [JsonProperty("AviationNote")]
            public string AviationNote { get; set; }
            [JsonProperty("LimitedNote")]
            public string LimitedNote { get; set; }
            [JsonProperty("ExchangeNote")]
            public string ExchangeNote { get; set; }
            [JsonProperty("CollectionNote")]
            public string CollectionNote { get; set; }
            [JsonProperty("EventNote")]
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
                // Dictionary<string, string> dictionary1 = new Dictionary<string, string>();

                string[,] data = 
                {
                    {"Light", Light, LightNote},
                    {"Heavy", Heavy, HeavyNote},
                    {"Aviation", Aviation, AviationNote},
                    {"Limited", Limited, LimitedNote},
                    {"Exchange", Exchange, ExchangeNote},
                    {"Collection", Collection, CollectionNote},
                    {"Event", Event, EventNote},

                    {"1-1", node11, note11 },
                    {"1-2", node12, note12 },
                    {"1-3", node13, note13 },
                    {"1-4", node14, note14 },
                    {"2-1", node21, note21 },
                    {"2-2", node22, note22 },
                    {"2-3", node23, note23 },
                    {"2-4", node24, note24 },
                    {"3-1", node31, note31 },
                    {"3-2", node32, note32 },
                    {"3-3", node33, note33 },
                    {"3-4", node34, note34 },
                    {"3-5", node35, note35 },
                    {"4-1", node41, note41 },
                    {"4-2", node42, note42 },
                    {"4-3", node43, note43 },
                    {"4-4", node44, note44 },
                    {"4-5", node45, note45 },
                    {"5-1", node51, note51 },
                    {"5-2", node52, note52 },
                    {"5-3", node53, note53 },
                    {"5-4", node54, note54 },
                    {"5-5", node55, note55 },
                    {"6-1", node61, note61 },
                    {"6-2", node62, note62 },
                    {"6-3", node63, note63 },
                    {"6-4", node64, note64 },
                    {"6-5", node65, note65 },
                    {"7-1", node71, note71 },
                    {"7-2", node72, note72 },
                    {"7-3", node73, note73 },
                    {"7-4", node74, note74 },
                    {"7-5", node75, note75 },
                    {"8-1", node81, note81 },
                    {"8-2", node82, note82 },
                    {"8-3", node83, note83 },
                    {"8-4", node84, note84 },
                    {"8-5", node85, note85 },
                    {"9-1", node91, note91 },
                    {"9-2", node92, note92 },
                    {"9-3", node93, note93 },
                    {"9-4", node94, note94 },
                    {"9-5", node95, note95 },
                    {"10-1", node101, note101 },
                    {"10-2", node102, note102 },
                    {"10-3", node103, note103 },
                    {"10-4", node104, note104 },
                    {"10-5", node105, note105 },
                    {"11-1", node111, note111 },
                    {"11-2", node112, note112 },
                    {"11-3", node113, note113 },
                    {"11-4", node114, note114 },
                    {"12-1", node121, note121 },
                    {"12-2", node122, note122 },
                    {"12-3", node123, note123 },
                    {"12-4", node124, note124 },
                    {"13-1", node131, note131 },
                    {"13-2", node132, note132 },
                    {"13-3", node133, note133 },
                    {"13-4", node134, note134 }
                };

                //var properties = GetType()
                //    .GetProperties()
                //    .Where(pi => pi.PropertyType == typeof(string) && pi.GetGetMethod() != null)
                //    .Select(pi =>
                //    {
                //        string attributeName = "";

                //        if (pi.CustomAttributes.Any())
                //        {
                //            attributeName = pi.CustomAttributes.ElementAt(0)?.ConstructorArguments[0].Value
                //                .ToString() ?? "";
                //        }

                //        return new
                //        {
                //            pi.Name,
                //            Attribute = attributeName,
                //            Value = pi.GetGetMethod().Invoke(this, null)
                //        };
                //    });

                //foreach (var property in properties)
                //{
                //    if (!property.Name.Equals("ID"))
                //    {
                //        string searchField = property.Name.Replace("node", "");

                //        var notes = properties.Where(p => p.Name.Contains(searchField)
                //                                          && (p.Name.Contains("note") || p.Name.Contains("Note")));

                //        string note = notes.ElementAt(0)?.Value.ToString();

                //        // if its field value equals t(true) (means its node property) 
                //        // or its note is not empty (check Akagi, she drops on 3-4, but there are no t on 3-4, only note)
                //        // also dictionary should not contain it already
                //        // and it should be clearly node (or construction) NOT note!
                //        if ((property.Value.ToString().Equals("t") || !string.IsNullOrEmpty(note)) 
                //            && !dictionary.ContainsKey(property.Attribute) && !property.Attribute.ToLower().Contains("note"))
                //        {
                //            dictionary.Add(property.Attribute, note ?? "");
                //        }
                //    }
                //}

                for (int i = 0; i < data.GetLength(0); i++)
                {
                    if ((data[i, 1].Equals("t") || !string.IsNullOrEmpty(data[i, 2])) && !dictionary.ContainsKey(data[i, 0]))
                    {
                        dictionary.Add(data[i, 0], data[i, 2]);
                    }
                }

                //foreach (var key in dictionary.Keys)
                //{
                //    if (dictionary1.ContainsKey(key))
                //    {
                //        if (!dictionary1[key].Equals(dictionary[key]))
                //        {
                //            Logger.Write($"Values are not equal. ID: {ID}; Key: {key}");
                //        }
                //    }
                //    else
                //    {
                //        Logger.Write($"No key. ID: {ID}; Key: {key}");
                //    }
                //}

                return dictionary;
            }
        }

        #endregion

        // if adding new locations don't forget to expand schema and also add to drop class in viewmodel of shipgirlpage.xaml
        string DropFields = "ID,Light,Heavy,Aviation,Limited,Exchange,Collection,Event,1-1,1-2,1-3,1-4,2-1,2-2,2-3," +
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

        public WTGShipGirlDownloader(int ThreadsCount = 0) : base(ThreadsCount)
        {
            DownloadTitle = "Downloading Maps...";
        }

        /// <summary>
        /// Download all cases of getting a ship girl and save them
        /// </summary>
        public override async Task Download()
        {
            Status = Statuses.InProgress;
            StatusDataMessage = "Downloading data.";
            List<WTGShipGirlJsonWrapper> wrappedDrops;

            try
            {
                string responseJson = await GetData("shipDrops", DropFields, "");
                wrappedDrops = JsonConvert.DeserializeObject<List<WTGShipGirlJsonWrapper>>(responseJson);
            }
            catch(JsonException)
            {
                Status = Statuses.ErrorInDeserialization;
                Logger.Write($"Failed to desirialize WTG.", this.GetType().ToString());
                return;
            }
            catch
            {
                Status = Statuses.DownloadError;
                Logger.Write($"Failed to get data for WTG from server.", this.GetType().ToString());
                return;
            }

            TotalDataCount = wrappedDrops.Count;
            StatusDataMessage = "Placing ShipGirls on Maps...";
            // Adding location if didn't exists and creating connection between drop location and ship girl
            using (CargoContext cargoContext = new CargoContext())
            {
                foreach (WTGShipGirlJsonWrapper wrappedDrop in wrappedDrops)
                {
                    // get dropped ship girl
                    ShipGirl shipGirl = await cargoContext.ShipGirls.FindAsync(wrappedDrop.WtgShipGirlJson.ID);

                    if (shipGirl != null)
                    {
                        // get all drops locations
                        Dictionary<string, string> dictionary = wrappedDrop.WtgShipGirlJson.GetDrops();

                        if (dictionary.Count > 0)
                        {
                            foreach (string key in dictionary.Keys)
                            {
                                // get location
                                WhereToGetShipGirl whereToGetShipGirl =
                                    await cargoContext.WhereToGetShipGirls.FindAsync(key);

                                // check if it exists, if not create
                                if (whereToGetShipGirl == null)
                                {
                                    whereToGetShipGirl = cargoContext.WhereToGetShipGirls.Add(new WhereToGetShipGirl{ Name = key });
                                    cargoContext.SaveChanges();
                                }

                                //  create connection
                                await CreateRelationshipGirlDrop(whereToGetShipGirl, shipGirl, dictionary[key], cargoContext);
                            }
                        }
                    }

                    lock (locker)
                    {
                        CurrentDataCount++;
                    }
                }
            }

            StatusDataMessage = "Complete.";
            Status = Statuses.DownloadComplete;
        }

        /// <summary>
        /// Download ShipGirl's drop locations and update them or save if they doesn't exist.
        /// </summary>
        /// <param name="id">ShipGirl's id</param>
        public override async Task Download(string id)
        {
            Status = Statuses.InProgress;
            List<WTGShipGirlJsonWrapper> wrappedDrops;

            try
            {
                string responseJson = await GetData("shipDrops", DropFields, "shipDrops.ID=\'" + id + "\'");
                wrappedDrops = JsonConvert.DeserializeObject<List<WTGShipGirlJsonWrapper>>(responseJson);
            }
            catch (JsonException)
            {
                Status = Statuses.ErrorInDeserialization;
                Logger.Write($"Failed to desirialize WTG for shipgril. Shipgirl ID: {id}", this.GetType().ToString());
                return;
            }
            catch
            {
                Status = Statuses.DownloadError;
                Logger.Write($"Failed to get WTG data for shipgirl from server. Shipgirl ID: {id}", this.GetType().ToString());
                return;
            }

            WTGShipGirlJsonWrapper wrappedDrop = wrappedDrops.FirstOrDefault();

            if (wrappedDrop == null)
            {
                Status = Statuses.EmptyResponse;
                return;
            }

            // Adding location if didn't exists and creating connection between drop location and ship girl
            using (CargoContext cargoContext = new CargoContext())
            {
                // get dropped ship girl
                ShipGirl shipGirl = await cargoContext.ShipGirls.FindAsync(wrappedDrop.WtgShipGirlJson.ID);

                if (shipGirl != null)
                {
                    // Remove old connections
                    var oldConnections =
                        await cargoContext.ShipGirlWhereToGetShipGirl
                            .Where(e => e.FK_ShipGirl.ShipID == shipGirl.ShipID).ToListAsync();

                    cargoContext.ShipGirlWhereToGetShipGirl.RemoveRange(oldConnections);
                    await cargoContext.SaveChangesAsync();

                    // get all drops locations
                    Dictionary<string, string> dictionary = wrappedDrop.WtgShipGirlJson.GetDrops();

                    if (dictionary.Count > 0)
                    {
                        foreach (string key in dictionary.Keys)
                        {
                            // get location
                            WhereToGetShipGirl whereToGetShipGirl =
                                await cargoContext.WhereToGetShipGirls.FindAsync(key);

                            // check if it exists, if not create
                            if (whereToGetShipGirl == null)
                            {
                                whereToGetShipGirl = cargoContext.WhereToGetShipGirls.Add(new WhereToGetShipGirl { Name = key });
                                cargoContext.SaveChanges();
                            }

                            //  create connection
                            await CreateRelationshipGirlDrop(whereToGetShipGirl, shipGirl, dictionary[key], cargoContext);
                        }
                    }
                }
            }

            Status = Statuses.DownloadComplete;
        }

        /// <summary>
        /// Get path to image folder.
        /// </summary>
        /// <param name="imageName">Name of image for saving</param>
        /// <returns>Path</returns>
        public override string GetImageFolder(string imageName) => ImagesFolderPath;

        public async Task CreateRelationshipGirlDrop(WhereToGetShipGirl wtg, ShipGirl shipGirl, string note, CargoContext cargoContext)
        {
            ShipGirlWhereToGetShipGirl mtm = new ShipGirlWhereToGetShipGirl
            {
                FK_ShipGirl = shipGirl,
                FK_WhereToGetShipGirl = wtg,
                Note = note
            };

            if (cargoContext.ShipGirlWhereToGetShipGirl.Count(e => e.FK_ShipGirl.ShipID == mtm.FK_ShipGirl.ShipID &&
                                                      e.FK_WhereToGetShipGirl.Name == mtm.FK_WhereToGetShipGirl.Name) == 0)
            {
                cargoContext.ShipGirlWhereToGetShipGirl.Add(mtm);
                await cargoContext.SaveChangesAsync();
            }
        }
    }
}
