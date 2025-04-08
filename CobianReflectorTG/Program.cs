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

        public static async Task Main()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Log support only two languages ENG and RU");
            Console.ResetColor();

            Task[] tasks = new Task[1];
            tasks[0] = Task.Run(() =>
            {
                BotSettings();
            });
            Task.WaitAll(tasks);

            Console.WriteLine("Pprogram will end in 5 seconds...");
            await Task.Delay(5000);
            //Console.ReadKey();
        }
      
        public static void BotSettings()
        {
            string DirExe = System.Reflection.Assembly.GetExecutingAssembly().Location;

            SettingsBot settingsBot = new SettingsBot();

            if (settingsBot.RegistryValuesExist())
            {
                ErrCheck();
            }
            else
            {
                // Add settings and encrypt to registry
                Console.WriteLine("Enter SERVER NAME for telegram bot");
                string StringName = Console.ReadLine();
                Console.WriteLine("Enter API key for telegram bot:");
                string StringApi = Console.ReadLine();
                Console.WriteLine("Enter CHAT ID for telegram bot");
                string StringId = Console.ReadLine();
                

                settingsBot.CryptBot(StringApi, StringId, StringName);

                CheckBot checkBot = new CheckBot();
                Task.Run(async () => await checkBot.CheckBotMethod()).Wait();

                ErrCheck();
            }
        }

        public static void ErrCheck()
        {
            //Console.OutputEncoding = Encoding.UTF8;

            //String path
            string LogFile = "C:\\Program Files\\Cobian Reflector\\Logs\\" + "Cobian Reflector " + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            string LogFileTemp = "C:\\Program Files\\Cobian Reflector\\Logs\\temp\\" + "Cobian Reflector " + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            string PathLogTemp = "C:\\Program Files\\Cobian Reflector\\Logs\\temp\\";

            //Create temp directory if not exist
            try
            {
                Directory.CreateDirectory(PathLogTemp);
            }
            catch
            {
                Console.WriteLine("Folder access denied: C:\\Program Files\\Cobian Reflector\\Logs");
            }

            //If log file exists, copy file and find errors
            if (File.Exists(LogFile))
            {
                try
                {
                    //Copy opened file to temp directory
                    File.Copy(LogFile, LogFileTemp, true);
                }
                catch
                {
                    Console.WriteLine("Folder access denied: C:\\Program Files\\Cobian Reflector\\Logs");
                }

                //Read file and find errors
                string fileContent = File.ReadAllText(LogFileTemp, Encoding.Unicode);

                Console.WriteLine(fileContent);

                //Find "ERR" lines with date
                string datePattern = DateTime.Now.ToString("yyyy-MM-dd");
                string regexPattern = "ERR " + Regex.Escape(datePattern) + ".*";
                string[] errorLines = Regex.Matches(fileContent, regexPattern, RegexOptions.Multiline).Cast<Match>().Select(match => match.Value).ToArray();
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
                if (errorLines.Length > 0)
                {
                    var task = TelegramBot(errorLines.Length.ToString(), errorDetails);
                    task.Wait();
                }

                //Delete temp file and exit
                Directory.GetFiles(PathLogTemp).ToList().ForEach(File.Delete);
            }

        }

        /*
        public static void ErrCheck()
        {
            //String path
            string LogFile = "C:\\Program Files\\Cobian Reflector\\Logs\\" + "Cobian Reflector " + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            string LogFileTemp = "C:\\Program Files\\Cobian Reflector\\Logs\\temp\\" + "Cobian Reflector " + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            string PathLogTemp = "C:\\Program Files\\Cobian Reflector\\Logs\\temp\\";

            //Create temp directory if not exist
            try
            {
                Directory.CreateDirectory(PathLogTemp);
            }
            catch
            {
                Console.WriteLine("Folder access denied: C:\\Program Files\\Cobian Reflector\\Logs");
                
            }

            //If log file exists, copy file and find errors
            if (File.Exists(LogFile))
            {
                try
                {
                    //Copy opened file to temp directory
                    File.Copy(LogFile, LogFileTemp, true);
                }
                catch
                {
                    Console.WriteLine("Folder access denied: C:\\Program Files\\Cobian Reflector\\Logs");
                }

                //Read file and find errors
                string fileContent = File.ReadAllText(LogFileTemp, Encoding.Unicode);

                Console.WriteLine(fileContent);

                //string fileContent = File.ReadAllText(LogFileTemp);
                string errorLine = Regex.Match(fileContent, @"\*\* (Ошибок:|Number of errors:) (\d+)\.").Value;

                //RU
                //string errorLine = Regex.Match(fileContent, @"\*\* Ошибок: (\d+)\.").Value;

                //ENG
                //string errorLine = Regex.Match(fileContent, @"\*\* Number of errors: (\d+)\.").Value;

                Console.WriteLine($"Extracted error line: {errorLine}");

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
        */

        static async Task TelegramBot(string ErrorCount, string ErrorDetails)
        {
            SettingsBot decryptBot = new SettingsBot();
            decryptBot.Decrypt();
            string string1 = decryptBot.DecryptedString1;
            string string2 = decryptBot.DecryptedString2;
            string string3 = decryptBot.DecryptedString3;

            if (string.IsNullOrEmpty(string1) || string.IsNullOrEmpty(string2) || string.IsNullOrEmpty(string3))
            {
                Console.WriteLine("Enter api key for telegram bot:");
                string StringApi = Console.ReadLine();
                Console.WriteLine("Enter chat id for telegram bot");
                string StringId = Console.ReadLine();
                Console.WriteLine("Enter server name for telegram bot");
                string StringName = Console.ReadLine();

                SettingsBot Write = new SettingsBot();
                Write.CryptBot(StringApi, StringId, StringName);
            }

            try
            {
                string externalIp = GetExternalIp();

                // limit length str ErrorDetails
                string truncatedErrorDetails = ErrorDetails.Length > 1000 ? ErrorDetails.Substring(0, 1000) + "..." : ErrorDetails;

                // Ready VarData
                string VarData = "Cobian Reflector ERR ⚠️"  + "\n" +
                                 "Server Name: " + string3 + "\n" +
                                 "User: " + Environment.UserName + "\n" +
                                 "Computer: " + Environment.MachineName + "\n" +
                                 "Time: " + DateTime.Now + "\n" +
                                 "Ip-Adress: " + externalIp + "\n" +
                                 hdd() + "\n" +
                                 "Error found: " + ErrorCount + "\n" +
                                 "`" + truncatedErrorDetails + "`";

                // Check limit length str VarData (Telegram API limit for text messages is 4096 characters)
                if (VarData.Length > 4096)
                {
                    VarData = VarData.Substring(0, 4093) + "...";
                }

                var client = new System.Net.Http.HttpClient();
                var url = $"https://api.telegram.org/bot{string1}/sendMessage?chat_id={string2}&parse_mode=Markdown&text={VarData}";
                var response = await client.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Bot send successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bot Error: " + ex);
            }
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
