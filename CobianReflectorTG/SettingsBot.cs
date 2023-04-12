using System;
using System.IO;
using System.Text;

namespace CobianReflectorTG
{
    class SettingsBot
    {
        public string DecryptedString1 { get; private set; }
        public string DecryptedString2 { get; private set; }

        public void CryptBot(string StringApi, string StringId)
        {
            string encryptedString1 = EncryptString(Environment.MachineName.ToString(), StringApi);
            string encryptedString2 = EncryptString(Environment.MachineName.ToString(), StringId);
            using (StreamWriter writer = new StreamWriter("BotSettings.txt"))
            {
                writer.WriteLine(encryptedString1);
                writer.WriteLine(encryptedString2);
            }
            Console.WriteLine("Data saved successfully");
        }

        static string EncryptString(string key, string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] encryptedBytes = new byte[plainTextBytes.Length];
            for (int i = 0; i < plainTextBytes.Length; i++)
            {
                encryptedBytes[i] = (byte)(plainTextBytes[i] ^ keyBytes[i % keyBytes.Length]);
            }
            return Convert.ToBase64String(encryptedBytes);
        }

        public void Decrypt()
        {
            string[] encryptedStrings = File.ReadAllLines("BotSettings.txt");
            DecryptedString1 = DecryptString(Environment.MachineName.ToString(), encryptedStrings[0]);
            DecryptedString2 = DecryptString(Environment.MachineName.ToString(), encryptedStrings[1]);
        }

        private string DecryptString(string key, string encryptedText)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] decryptedBytes = new byte[encryptedBytes.Length];
            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                decryptedBytes[i] = (byte)(encryptedBytes[i] ^ keyBytes[i % keyBytes.Length]);
            }
            return Encoding.UTF8.GetString(decryptedBytes);
        }

    }
}


