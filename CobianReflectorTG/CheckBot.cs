using System;
using System.Net;
using System.Threading.Tasks;

namespace CobianReflectorTG
{
    class CheckBot
    {
        public async Task CheckBotMethod()
        {
            SettingsBot decryptBot = new SettingsBot();
            decryptBot.Decrypt();
            string string1 = decryptBot.DecryptedString1;
            string string2 = decryptBot.DecryptedString2;
            string string3 = decryptBot.DecryptedString3;

            if (string1 == "")
            {
                //Add settings and crypt to file
                Console.WriteLine("Enter api key for telegram bot:");
                String StringApi = Console.ReadLine();
                Console.WriteLine("Enter chat id for telegram bot");
                String StringId = Console.ReadLine();
                Console.WriteLine("Enter server name for telegram bot");
                string StringName = Console.ReadLine();

                SettingsBot Write = new SettingsBot();
                Write.CryptBot(StringApi, StringId, StringName);
            }

            if (string2 == "")
            {
                //Add settings and crypt to file
                Console.WriteLine("Enter api key for telegram bot:");
                String StringApi = Console.ReadLine();
                Console.WriteLine("Enter chat id for telegram bot");
                String StringId = Console.ReadLine();
                Console.WriteLine("Enter server name for telegram bot");
                string StringName = Console.ReadLine();

                SettingsBot Write = new SettingsBot();
                Write.CryptBot(StringApi, StringId, StringName);
            }

            try
            {
                string externalIp = GetExternalIp();
                string VarData = "Cobian Reflector ✅ " + "\n" + "Srver Name: " + string3 + "\n" + "User: " + Environment.UserName + "\n" + "Computer: " + Environment.MachineName + "\n" + "Time: " + DateTime.Now + "\n" + "Ip-Adress: " + externalIp +  "\n" + "`" + "Bot installation was successful ✅" + "`";
                var client = new System.Net.Http.HttpClient();
                var url = $"https://api.telegram.org/bot{string1}/sendMessage?chat_id={string2}&parse_mode=Markdown&text={VarData}";
                var response = await client.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Check Bot Ok");
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
    }
}
