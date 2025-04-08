using Microsoft.Win32;
using System;
using System.Text;

namespace CobianReflectorTG
{
    class SettingsBot
    {
        public string DecryptedString1 { get; private set; }
        public string DecryptedString2 { get; private set; }
        public string DecryptedString3 { get; private set; }

        private const string RegistryPath = @"SOFTWARE\CobianSoft";

        public bool RegistryValuesExist()
        {
            try
            {
                using (RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(RegistryPath))
                {
                    if (key != null)
                    {
                        string randomUser = (string)key.GetValue("RandomUser", string.Empty);
                        string randomPass = (string)key.GetValue("RandomPass", string.Empty);
                        string randomName = (string)key.GetValue("RandomName", string.Empty);

                        return !string.IsNullOrEmpty(randomUser) && !string.IsNullOrEmpty(randomPass) && !string.IsNullOrEmpty(randomName);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while accessing the registry: " + ex.Message);
            }
            return false;
        }


        public void CryptBot(string StringApi, string StringId, string StringName)
        {
            try
            {
                string encryptedString1 = EncryptString(Environment.MachineName.ToString(), StringApi);
                string encryptedString2 = EncryptString(Environment.MachineName.ToString(), StringId);
                string encryptedString3 = EncryptString(Environment.MachineName.ToString(), StringName);

                //Create or open key for 64 bit system
                using (RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).CreateSubKey(RegistryPath))
                {
                    if (key != null)
                    {
                        key.SetValue("RandomUser", encryptedString1, RegistryValueKind.String);
                        key.SetValue("RandomPass", encryptedString2, RegistryValueKind.String);
                        key.SetValue("RandomName", encryptedString3, RegistryValueKind.String);
                        Console.WriteLine("Data saved successfully");
                    }
                    else
                    {
                        Console.WriteLine("Failed to open or create the registry key.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Первый запуск от имени Администратора / First launch as Administrator:\n" + ex.Message);
                Console.ResetColor();
            }
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
            try
            {
                //Open key 64 bit system
                using (RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(RegistryPath))
                {
                    if (key != null)
                    {
                        string encryptedString1 = (string)key.GetValue("RandomUser", string.Empty);
                        string encryptedString2 = (string)key.GetValue("RandomPass", string.Empty);
                        string encryptedString3 = (string)key.GetValue("RandomName", string.Empty);

                        if (!string.IsNullOrEmpty(encryptedString1) && !string.IsNullOrEmpty(encryptedString2) && !string.IsNullOrEmpty(encryptedString3))
                        {
                            DecryptedString1 = DecryptString(Environment.MachineName.ToString(), encryptedString1);
                            DecryptedString2 = DecryptString(Environment.MachineName.ToString(), encryptedString2);
                            DecryptedString3 = DecryptString(Environment.MachineName.ToString(), encryptedString3);
                            Console.WriteLine("Data decrypted successfully");
                        }
                        else
                        {
                            Console.WriteLine("No data found in the registry.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Failed to open the registry key.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while reading data from the registry: " + ex.Message);
            }
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
