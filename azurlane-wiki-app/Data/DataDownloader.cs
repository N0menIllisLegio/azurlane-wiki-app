using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace azurlane_wiki_app
{
    abstract class DataDownloader : INotifyPropertyChanged
    {
        public const string ImagesFolderPath = "./Images";
        public const string WikiAPIEndpoint = "https://azurlane.koumakan.jp/w/api.php";
        public const string ImageInfoAPIEndpoint =
            "https://azurlane.koumakan.jp/w/api.php?action=query&format=json&list=allimages&ailimit=1&aifrom=";

        protected List<string> ReloadImages = new List<string>();

        public abstract string GetImageFolder(string imageName);
        public abstract void Download();

        #region Notify

        private int _currentImageCount = 0;
        public int _totalImageCount = 0;

        public int totalImageCount
        {
            get => _totalImageCount;
            set
            {
                _totalImageCount = value;
                OnPropertyChanged(nameof(totalImageCount));
            }
        }

        public int currentImageCount
        {
            get => _currentImageCount;
            set
            {
                _currentImageCount = value;
                OnPropertyChanged(nameof(currentImageCount));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        protected DataDownloader()
        {
            if (!Directory.Exists(ImagesFolderPath))
            {
                Directory.CreateDirectory(ImagesFolderPath);
            }
        }

        /// <summary>
        /// Download image by its name.
        /// </summary>
        /// <param name="imageName">Image name (example: 22Chibi.png)</param>
        public async Task DownloadImage(string imageName)
        {
            string imageURL = "";

            try
            {
                imageURL = await GetImageInfo(imageName);
            }
            catch
            {
                ReloadImages.Add(imageName);
                imageURL = "";
            }

            if (imageName != "" && imageURL != "")
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                                       SecurityProtocolType.Tls11 |
                                                       SecurityProtocolType.Tls12;

                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(imageURL);
                request.Timeout = 14400000;
                request.KeepAlive = true;

                using (HttpWebResponse response = (HttpWebResponse) await request.GetResponseAsync())
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

                                    byte[] fileBuffer = new byte[(int) memoryStream.Length];
                                    memoryStream.Position = 0;
                                    memoryStream.Read(fileBuffer, 0, fileBuffer.Length);

                                    string folderName = GetImageFolder(imageName);

                                    using (FileStream fileStream =
                                        new FileStream(folderName + "/" + imageName, FileMode.Create))
                                    {
                                        fileStream.Write(fileBuffer, 0, fileBuffer.Length);
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        ReloadImages.Add(imageName);
                    }
                }
            }
            currentImageCount++;
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
                string requestURL = ImageInfoAPIEndpoint + imageName;
                string responseJSON = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestURL);

                using (HttpWebResponse response = (HttpWebResponse) await request.GetResponseAsync())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            responseJSON = reader.ReadToEnd();
                        }
                    }
                }

                Regex name = new Regex("\"url\":\".*?\"", RegexOptions.Compiled);
                MatchCollection matches = name.Matches(responseJSON);

                if (matches.Count > 0)
                {
                    requestURL = matches[0].Value.Remove(0, 7); // removing "url":" 
                    requestURL = requestURL.Remove(requestURL.Length - 1); // removing last "

                    result = requestURL;
                }
            }

            return result;
        }
    }
}
