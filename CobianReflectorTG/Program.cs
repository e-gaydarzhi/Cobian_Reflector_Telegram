using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CobianReflectorTG
{
    class Program
    {
        static void Main(string[] args)
        {

            //Vars
            string DateNow = DateTime.Now.ToString("yyyy-MM-dd");
            string PathLog = "C:\\Program Files\\Cobian Reflector\\Logs\\";
            string PathLogTemp = "C:\\Program Files\\Cobian Reflector\\Logs\\temp\\";
            string BotID = File.ReadLines(System.Environment.CurrentDirectory + "\\CobianReflectorTG.cfg").ElementAtOrDefault(0);
            string BotChatID = File.ReadLines(System.Environment.CurrentDirectory + "\\CobianReflectorTG.cfg").ElementAtOrDefault(1);

            //Create Temp if not exist
            System.IO.Directory.CreateDirectory(PathLogTemp);

            if (File.Exists(PathLog + "Cobian Reflector " + DateNow + ".txt"))
            {
                Console.WriteLine("File exists");

                //Copy Opened File
                System.IO.File.Copy(PathLog + "Cobian Reflector " + DateNow + ".txt", PathLogTemp + "Cobian Reflector " + DateNow + ".txt", true);

                //Get Public IP
                string externalIpString = new WebClient().DownloadString("http://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "").Trim();
                var externalIp = IPAddress.Parse(externalIpString);

                //Read File
                var lines = File.ReadLines(PathLogTemp + "Cobian Reflector " + DateNow + ".txt");
                string result = string.Join("\n", lines.Where(s => s.IndexOf("ERR", StringComparison.InvariantCultureIgnoreCase) >= 0));
                Console.WriteLine(result);

                try
                {
                    //Check results if empty
                    if (result == "")
                    {
                        Console.WriteLine("Log file not have errors");
                    }
                    else
                    {
                        //Send result in Telegram
                        string GetBot = "Ошибка Cobian Reflector ⚠️ " + "\n" + "Пользователь: " + Environment.UserName + "\n" + "Компьютер: " + Environment.MachineName + "\n" + "IP-Адресс: " + externalIp.ToString() + "\n" + "`" + result + "`";
                        System.Net.WebRequest reqGET = System.Net.WebRequest.Create(@"https://api.telegram.org/bot" + BotID + "/sendMessage?chat_id=" + BotChatID + "&parse_mode=Markdown&text=" + GetBot);
                        System.Net.WebResponse resp = reqGET.GetResponse();
                        System.IO.Stream stream = resp.GetResponseStream();
                        System.IO.StreamReader sr = new System.IO.StreamReader(stream);
                        Console.WriteLine("Errors not finded");
                    }
                }
                catch
                {
                    Console.WriteLine("File cannot be opened");
                }
                finally
                {
                    //Delete temp file and exit
                    System.IO.File.Delete(PathLogTemp + "Cobian Reflector " + DateNow + ".txt");
                    Environment.Exit(0);
                }
            }
            else
            {
                Console.WriteLine("File not exists");
                Environment.Exit(0);
            }
        }
    }
}
