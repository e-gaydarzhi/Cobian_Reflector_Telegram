using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CobianReflectorTG
{
    class Program
    {
        public static void Main()
        {
            //Check Settings
            BotSettings();
        }

        public static void BotSettings()
        {
            if (File.Exists("BotSettings.txt"))
            {
                ErrCheck();
            }
            else
            {
                //Add settings and crypt to file
                Console.WriteLine("Enter api key for telegram bot:");
                String StringApi = Console.ReadLine();
                Console.WriteLine("Enter chat id for telegram bot");
                String StringId = Console.ReadLine();

                SettingsBot Write = new SettingsBot();
                Write.CryptBot(StringApi, StringId);
            }
        }

        public static void ErrCheck()
        {
            //String path
            string LogFile = "C:\\Program Files\\Cobian Reflector\\Logs\\" + "Cobian Reflector " + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            string LogFileTemp = "C:\\Program Files\\Cobian Reflector\\Logs\\temp\\" + "Cobian Reflector " + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            string PathLogTemp = "C:\\Program Files\\Cobian Reflector\\Logs\\temp\\";

            //Create temp directory if not exist
            Directory.CreateDirectory(PathLogTemp);

            //If log file exists, copy file and find errors
            if (File.Exists(LogFile))
            {
                //Copy opened file to temp directory
                File.Copy(LogFile, LogFileTemp, true);

                //Read file and find errors
                string fileContent = File.ReadAllText(LogFileTemp);

                string errorLine = Regex.Match(fileContent, @"\*\* Number of errors: (\d+)\.").Value;
                int errorCount;
                if (int.TryParse(Regex.Match(errorLine, @"\d+").Value, out errorCount))
                {
                    if (errorCount > 0)
                    {
                        Console.WriteLine($"Error found: {errorCount}");

                        //Find "ERR" lines
                        string[] errorLines = Regex.Matches(fileContent, @"ERR.*", RegexOptions.Multiline).Cast<Match>().Select(match => match.Value).ToArray();
                        Console.WriteLine($"Error details: {errorLines.Length}");

                        //Add to builder
                        StringBuilder sb = new StringBuilder();
                        foreach (string line in errorLines)
                        {
                            sb.AppendLine(line);
                        }
                        string errorDetails = sb.ToString();
                        Console.WriteLine(errorDetails);

                        //Call Telegram
                        var task = TelegramBot(errorCount.ToString(), errorDetails.ToString());
                        task.Wait();
                    }
                    else
                    {
                        Console.WriteLine($"No Error found: {errorCount}");
                    }
                }
                else

                {
                    Console.WriteLine("Number of errors not found");
                }
                //Delete temp file and exit
                Directory.GetFiles(PathLogTemp).ToList().ForEach(File.Delete);
            }
        }

        static async Task TelegramBot(string ErrorCount, string ErrorDetails)
        {
            SettingsBot decryptBot = new SettingsBot();
            decryptBot.Decrypt();
            string string1 = decryptBot.DecryptedString1;
            string string2 = decryptBot.DecryptedString2;

            string externalIp = GetExternalIp();
            string VarData = "Cobian Reflector Error⚠️" + "\n" + "User: " + Environment.UserName + "\n" + "Computer: " + Environment.MachineName + "\n" + "Time: " + DateTime.Now + "\n" + "Ip-Adress: " + externalIp + "\n" + hdd() + "\n" + "Error found: " + ErrorCount + "\n" + "`" + ErrorDetails + "`";
            var client = new System.Net.Http.HttpClient();
            var url = $"https://api.telegram.org/bot{string1}/sendMessage?chat_id={string2}&parse_mode=Markdown&text={VarData}";
            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Bot Ok");
        }

        public static string GetExternalIp()
        {
            using (var client = new WebClient())
            {
                string response = client.DownloadString("https://api.ipify.org");
                return response;
            }
        }

        static string hdd()
        {
            try
            {
                DriveInfo driveInfo = new DriveInfo("C:\\");
                long freeSpaceInBytes = driveInfo.TotalFreeSpace;
                double freeSpaceInGB = (double)freeSpaceInBytes / (1024 * 1024 * 1024);
                return $"Free space on C:\\ drive: {freeSpaceInGB:F2} GB";
            }
            catch
            {
                return "Error";
            }
        }
    }
}
