using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace azurlane_wiki_app
{
    enum ImageType
    {
        Image,
        ImageShipyardIcon,
        ImageChibi,
        ImageIcon,
        ImageBanner,
        // Kai = Retrofit
        ImageKai,
        ImageShipyardIconKai,
        ImageChibiKai,
        ImageIconKai,
        ImageBannerKai
    }

    class DataDownloader
    {
        private string WikiAPIEndpoint = "https://azurlane.koumakan.jp/w/api.php";

        private string ImageInfoAPIEndpoint =
            "https://azurlane.koumakan.jp/w/api.php?action=query&format=json&list=allimages&ailimit=1&aifrom=";

        private string ImagesFolderPath = "./Images";
        private string KaiImagesFolderPath = "./Images/KaiImages";
        private string NonKaiImagesFolderPath = "./Images/Images";

        public DataDownloader()
        {
            if (!Directory.Exists(ImagesFolderPath))
            {
                Directory.CreateDirectory(ImagesFolderPath);
            }
            if (!Directory.Exists(KaiImagesFolderPath))
            {
                Directory.CreateDirectory(KaiImagesFolderPath);
            }
            if (!Directory.Exists(NonKaiImagesFolderPath))
            {
                Directory.CreateDirectory(NonKaiImagesFolderPath);
            }
        }

        public async void DownloadShips()
        {
            string shipFields = "Image,ImageShipyardIcon,ImageChibi,ImageIcon,ImageBanner,ImageKai,ImageShipyardIconKai,ImageChibiKai,ImageIconKai,ImageBannerKai";
            string requestURL = WikiAPIEndpoint + "?action=cargoquery&tables=ships&format=json&fields=" + shipFields + "&limit=1"; //TODO: Remove limit
            string responseJSON = "";
            
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestURL);
            HttpWebResponse response = (HttpWebResponse) await request.GetResponseAsync();

            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    responseJSON = reader.ReadToEnd();
                }
            }

            // TODO: Custom Converter
            responseJSON = responseJSON.Remove(0, 24);
            responseJSON = responseJSON.Remove(responseJSON.Length - 3);

            Dictionary<string, string> imagesDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseJSON);

            foreach (var image in imagesDictionary)
            {
                GetImageInfo(image.Value);
            }
        }

        private string GetImageFolder(string imageName)
        {
            string folderName = "";

            if (imageName.Contains("Kai"))
            {
                // Retrofit folders
                folderName = KaiImagesFolderPath;

                if (imageName.Contains("ShipyardIcon"))
                {
                    folderName += "/KaiShipyardIcons";
                } 
                else if (imageName.Contains("Chibi"))
                {
                    folderName += "/KaiChibis";
                } 
                else if (imageName.Contains("Banner"))
                {
                    folderName += "/KaiBanners";
                } 
                else if (imageName.Contains("Icon"))
                {
                    folderName += "/KaiIcons";
                }
                else
                {
                    folderName += "/KaiImages";
                }
            } 
            else
            {
                folderName = NonKaiImagesFolderPath;

                if (imageName.Contains("ShipyardIcon"))
                {
                    folderName += "/ShipyardIcons";
                } 
                else if (imageName.Contains("Chibi"))
                {
                    folderName += "/Chibis";
                } 
                else if (imageName.Contains("Banner"))
                {
                    folderName += "/Banners";
                } 
                else if (imageName.Contains("Icon"))
                {
                    folderName += "/Icons";
                }
                else
                {
                    folderName += "/Images";
                }
            }

            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            return folderName;
        }

        private async void GetImageInfo(string imageName)
        {
            if (imageName != "")
            {
                string requestURL = ImageInfoAPIEndpoint + imageName;
                string responseJSON = "";

                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestURL);
                HttpWebResponse response = (HttpWebResponse) await request.GetResponseAsync();

                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        responseJSON = reader.ReadToEnd();
                    }
                }

                response.Close();

                Regex name = new Regex("\"url\":\".*?\"", RegexOptions.Compiled);
                MatchCollection matches = name.Matches(responseJSON);

                if (matches.Count > 0)
                {
                    requestURL = matches[0].Value.Remove(0, 7); // removing "url":" 
                    requestURL = requestURL.Remove(requestURL.Length - 1); // removing last "

                    DownloadImage(requestURL, imageName);
                }
            }
        }

        private async void DownloadImage(string imageURL, string imageName)
        {
            if (imageName != "" && imageURL != "")
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(imageURL);

                using (HttpWebResponse response = (HttpWebResponse) await request.GetResponseAsync())
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
            }
        }
    }
}
