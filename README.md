# CobianBackupTelegram

This is a console program that is used to send notifications to telegrams about unsuccessful backups.
The code is written entirely in c# .Net Framework 4.6
To use, open the project in Visual Studio and change the "BotID" and "BotChatID" lines

The program does not affect the Cobian Backup service, does not change the files of the current (active) log.

Example:
![image](https://user-images.githubusercontent.com/107859162/190390771-3025e227-589b-4b5d-853a-3af6be55adce.png)


Find these lines
```
string BotID = "6497324784:A47WeRqkc457frtIT5Qut4Qz6CfetFe489c";
string BotChatID = "-71554284";
```
You need to change the data inside the quotes

Example:
![image](https://user-images.githubusercontent.com/107859162/190393047-fafa81bc-a747-40e3-b080-7841a4b3f7cf.png)


After you have managed to compile the console program, you must install it in cobian backup so that it will be executed when the task is completed.
![image](https://user-images.githubusercontent.com/107859162/190393595-72d7baab-d8b2-4c62-98dd-44d180870008.png)
