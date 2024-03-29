﻿using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CobianReflectorTG
{
    class Program
    {
        public static void Main()
        {
            Task[] tasks = new Task[1];
            tasks[0] = Task.Run(() =>
            {
                BotSettings();
            });
            Task.WaitAll(tasks);
        }

        public static void BotSettings()
        {
            string DirExe = System.Reflection.Assembly.GetExecutingAssembly().Location;

            if (File.Exists(Path.GetDirectoryName(DirExe) + "\\BotSettings.txt"))
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

                CheckBot Check = new CheckBot();
                Task.Run(async () => await Check.CheckBotMethod()).Wait();

                ErrCheck();
            }
        }

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
                Console.ReadKey();
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
                    Console.ReadKey();
                }

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

            if (string1 == "")
            {
                //Add settings and crypt to file
                Console.WriteLine("Enter api key for telegram bot:");
                String StringApi = Console.ReadLine();
                Console.WriteLine("Enter chat id for telegram bot");
                String StringId = Console.ReadLine();

                SettingsBot Write = new SettingsBot();
                Write.CryptBot(StringApi, StringId);
            }

            if (string2 == "")
            {
                //Add settings and crypt to file
                Console.WriteLine("Enter api key for telegram bot:");
                String StringApi = Console.ReadLine();
                Console.WriteLine("Enter chat id for telegram bot");
                String StringId = Console.ReadLine();

                SettingsBot Write = new SettingsBot();
                Write.CryptBot(StringApi, StringId);
            }

            try
            {
                string externalIp = GetExternalIp();
                string VarData = "Cobian Reflector Error⚠️" + "\n" + "User: " + Environment.UserName + "\n" + "Computer: " + Environment.MachineName + "\n" + "Time: " + DateTime.Now + "\n" + "Ip-Adress: " + externalIp + "\n" + hdd() + "\n" + "Error found: " + ErrorCount + "\n" + "`" + ErrorDetails + "`";
                var client = new System.Net.Http.HttpClient();
                var url = $"https://api.telegram.org/bot{string1}/sendMessage?chat_id={string2}&parse_mode=Markdown&text={VarData}";
                var response = await client.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Bot send successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bot Error" + ex);
                Console.ReadKey();
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
