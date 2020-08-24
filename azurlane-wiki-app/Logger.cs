using System;
using System.IO;

namespace azurlane_wiki_app
{
	class Logger
    {
		static readonly string PathToLogFile = "./log.txt";
		static object locker = new object();

		public static void Write(string message)
		{
			WriteToFile(message);
		}

		public static void Write(string message, string place)
        {
			WriteToFile($"{place}: {message}");
        }

        public static void Write(string message, string place, Exception exception)
        {
            WriteToFile($"{place}: {message}\n\t\t{exception}");
        }

		private static void WriteToFile(string str)
        {
			lock (locker)
			{
				using (StreamWriter file = new StreamWriter(PathToLogFile, true))
				{
					file.WriteLine($"{DateTime.Now}	  {str}");
				}
			}
		}
	}
}