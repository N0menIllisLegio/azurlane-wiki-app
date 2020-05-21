using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace azurlane_wiki_app.Data.Downloaders
{
    public enum Statuses
    {
        Pending,
        DownloadComplete,
        ErrorInDeserialization,
        InProgress,
        DownloadError,
        EmptyResponse
    }

    public abstract class DataDownloader : INotifyPropertyChanged
    {
        public static readonly string ImagesFolderPath = "./Images";

        protected const string WikiApiEndpoint = "https://azurlane.koumakan.jp/w/api.php";
        protected ActionBlock<string> downloadBlock;

        protected object locker = new object();
        private const string ImageInfoApiEndpoint =
            "https://azurlane.koumakan.jp/w/api.php?action=query&format=json&list=allimages&ailimit=1&aifrom=";

        #region Notify

        private int _currentImageCount;
        private int _totalImageCount;
        private Statuses _statuses;
        private string _statusImageMessage;
        private string _statusDataMessage;
        private int _currentDataCount;
        private int _totalDataCount;

        public int TotalImageCount
        {
            get => _totalImageCount;
            set
            {
                _totalImageCount = value;
                OnPropertyChanged(nameof(TotalImageCount));
            }
        }

        public int CurrentImageCount
        {
            get => _currentImageCount;
            set
            {
                _currentImageCount = value;
                OnPropertyChanged(nameof(CurrentImageCount));
            }
        }


        public int TotalDataCount
        {
            get => _totalDataCount;
            set
            {
                _totalDataCount = value;
                OnPropertyChanged(nameof(TotalDataCount));
            }
        }

        public int CurrentDataCount
        {
            get => _currentDataCount;
            set
            {
                _currentDataCount = value;
                OnPropertyChanged(nameof(CurrentDataCount));
            }
        }

        public Statuses Status
        {
            get => _statuses;
            set
            {
                _statuses = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public string StatusImageMessage
        {
            get => _statusImageMessage;
            set
            {
                _statusImageMessage = value;
                OnPropertyChanged(nameof(StatusImageMessage));
            }
        }

        public string StatusDataMessage
        {
            get => _statusDataMessage;
            set
            {
                _statusDataMessage = value;
                OnPropertyChanged(nameof(StatusDataMessage));
            }
        }

        public string DownloadTitle { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        /// <summary>
        /// Initializer.
        /// </summary>
        /// <param name="ThreadsCount"> Count of threads for download. If ThreadsCount &lt;= 0 then Max possible amount.</param>
        protected DataDownloader(int ThreadsCount)
        {
            Status = Statuses.Pending;

            if (!Directory.Exists(ImagesFolderPath))
            {
                Directory.CreateDirectory(ImagesFolderPath);
            }

            downloadBlock = ThreadsCount <= 0 
                ? new ActionBlock<string>(
                    image => DownloadImage(image).Wait())
                : new ActionBlock<string>(
                    image => DownloadImage(image).Wait(),
                    new ExecutionDataflowBlockOptions
                    {
                        MaxDegreeOfParallelism = ThreadsCount
                    });

            downloadBlock.Completion.ContinueWith(dB => StatusImageMessage = "Complete.");
        }

        /// <summary>
        /// Downloads all items and save them
        /// </summary>
        public abstract Task Download();

        /// <summary>
        /// Downloads only specified item
        /// </summary>
        /// <param name="id">Item identifier (name, id etc.)</param>
        public abstract Task Download(string id);

        /// <summary>
        /// Gets path to folder for image. Depends on image name. (Image containing ShipyardIcon in name gets path to ShipyardIcons folder).
        /// </summary>
        /// <param name="imageName">Image name (example: 22Chibi.png)</param>
        /// <returns></returns>
        public abstract string GetImageFolder(string imageName);

        /// <summary>
        /// Gets all data from specified table.
        /// </summary>
        /// <param name="table">Table to get data from</param>
        /// <param name="fields">Fields from table</param>
        /// <returns>Json array with data</returns>
        protected async Task<string> GetData(string table, string fields, string where)
        {
            int offset = 0;
            int limit = 500;

            string result = "[";
            string responseJson;

            do
            {
                string requestUrl = WikiApiEndpoint + "?action=cargoquery&format=json&tables=" + table + "&fields=" + fields +
                                    "&where=" + where + "&limit=" + limit + "&offset=" + offset;
                responseJson = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();

                using (Stream stream = response?.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            responseJson = await reader.ReadToEndAsync();
                        }
                    }
                }

                responseJson = responseJson.Remove(0, 15);           // remove {"cargoquery":[
                responseJson = responseJson.Remove(responseJson.Length - 2);        // remove ]}

                result += responseJson + ",";
                offset += 500;
            } while (responseJson.Length != 0);

            return result.Remove(result.Length - 2) + ']'; // remove last , and add ]
        }

        /// <summary>
        /// Gets all data from joined tables.
        /// </summary>
        /// <param name="tables">Tables to get data from (shipDrops,ship_skills,equipment,ships)</param>
        /// <param name="fields">Fields from tables (shipDrops.Name, ships.ShipID etc)</param>
        /// <param name="joinOn">Fields to join on (ships._pageName=ship_skills._pageName)</param>
        /// <returns>Json array with data</returns>
        protected async Task<string> GetData(string tables, string fields, string joinOn, string where)
        {
            int offset = 0;
            int limit = 500;

            string result = "[";
            string responseJson;

            do
            {
                string requestUrl = WikiApiEndpoint + "?action=cargoquery&format=json&tables=" + tables + "&where=" + where
                                    + "&fields=" + fields + "&limit=" + limit + "&offset=" + offset + "&join_on=" + joinOn;
                responseJson = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();

                using (Stream stream = response?.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            responseJson = await reader.ReadToEndAsync();
                        }
                    }
                }

                responseJson = responseJson.Remove(0, 15);           // remove {"cargoquery":[
                responseJson = responseJson.Remove(responseJson.Length - 2);        // remove ]}

                result += responseJson + ",";
                offset += 500;
            } while (responseJson.Length != 0);

            return result.Remove(result.Length - 2) + ']'; // remove last , and add ]
        }

        /// <summary>
        /// Download image by its name.
        /// </summary>
        /// <param name="imageName">Image name (example: 22Chibi.png)</param>
        /// <returns>Relative image path</returns>
        protected async Task<string> DownloadImage(string imageName)
        {
            string imageUrl;
            string imagePath = "";

            if (!string.IsNullOrEmpty(imageName))
            {
                try
                {
                    imageUrl = await GetImageInfo(imageName);
                }
                catch
                {
                    imageUrl = "";
                }
            }
            else
            {
                imageUrl = "";
            }

            await DownloadImageByUrl(imageUrl, imageName);

            lock (locker)
            {
                CurrentImageCount++;
            }

            return imagePath;
        }

        /// <summary>
        /// Download image by url
        /// </summary>
        /// <param name="imageUrl">Image URL</param>
        /// <param name="imageName">Image file name with extension (image.png).</param>
        /// <returns>Relative path to image</returns>
        protected async Task<string> DownloadImageByUrl(string imageUrl, string imageName)
        {
            string imagePath = "";

            if (!string.IsNullOrEmpty(imageUrl) && !string.IsNullOrEmpty(imageName))
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                                       SecurityProtocolType.Tls11 |
                                                       SecurityProtocolType.Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(imageUrl);
                request.Timeout = 14400000;
                request.KeepAlive = true;

                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                    {
                        try
                        {
                            Stream stream = response.GetResponseStream();

                            if (stream != null)
                            {
                                using (BinaryReader binaryReader = new BinaryReader(stream))
                                {
                                    using (MemoryStream memoryStream = new MemoryStream())
                                    {
                                        byte[] buffer = binaryReader.ReadBytes(1024);

                                        while (buffer.Length > 0)
                                        {
                                            memoryStream.Write(buffer, 0, buffer.Length);
                                            buffer = binaryReader.ReadBytes(1024);
                                        }

                                        byte[] fileBuffer = new byte[(int)memoryStream.Length];
                                        memoryStream.Position = 0;
                                        await memoryStream.ReadAsync(fileBuffer, 0, fileBuffer.Length);

                                        string folderName = GetImageFolder(imageName);
                                        imagePath = folderName + "/" + imageName;

                                        using (FileStream fileStream =
                                            new FileStream(imagePath, FileMode.Create))
                                        {
                                            await fileStream.WriteAsync(fileBuffer, 0, fileBuffer.Length);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            //TODO: Add error display
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message); // 404
                }
            }

            return imagePath;
        }

        /// <summary>
        /// Gets image URL from image name.
        /// </summary>
        /// <param name="imageName">Image name (example: 22Chibi.png)</param>
        /// <returns></returns>
        private async Task<string> GetImageInfo(string imageName)
        {
            string result = "";

            string dirName = GetImageFolder(imageName);
            
            if (imageName != "" && !File.Exists(dirName + "/" + imageName))
            {
                string requestUrl = ImageInfoApiEndpoint + imageName;
                string responseJson = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);

                using (HttpWebResponse response = (HttpWebResponse) await request.GetResponseAsync())
                {
                    using (Stream stream = response?.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                responseJson = await reader.ReadToEndAsync();
                            }
                        }
                    }
                }

                int i = responseJson.IndexOf("\"url\":\"", StringComparison.Ordinal) + 7;
                
                while (responseJson[i] != '"')
                {
                    result += responseJson[i++];
                }
            }

            return result;
        }
    }
}
