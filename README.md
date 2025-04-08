# Cobian Reflector Telegram

This is a console program for Windows, that is used to send notifications to telegrams about unsuccessful backups. The code is written entirely in c# .Net Framework 4.6 The program does not affect the Cobian Reflector service, does not change the files of the current (active) log.

[Release For Windows](https://github.com/e-gaydarzhi-2077/Cobian_Reflector_Telegram/releases)

After update 1.3, the api key and chat id are now encrypted.

Application launch example:
You need to run the program for the first time as an administrator so that it writes all the encrypted keys of your bot to the registry.

Then enter:
1) Your server name (So that you can see which server has a problem)
2) Telegram bot API key
3) Your group ID for notifications (Don't forget that it should look like this -34792156)


![2023-04-12 11_36_09-Temp](https://user-images.githubusercontent.com/107859162/231405590-566a499c-4e98-4ba0-8a63-f54955e35b7f.png)


After the application finishes its work on finding errors,
a notification should come to your telegram chat as shown in the screenshot below:
![2023_04_12_11_45_18_Mirvari_Backup_Eugene_44782_](https://user-images.githubusercontent.com/107859162/231404399-e7cfc2c9-4aac-491d-b0f6-fb8a83a47ca6.png)

You can add a launch in the task scheduler, or add to cobian reflector (Post Backup Events) to make it run at the end of the job.
