# Cobian Reflector Telegram

This is a console program for Windows, that is used to send notifications to telegrams about unsuccessful backups. The code is written entirely in c# .Net Framework 4.6 The program does not affect the Cobian Reflector service, does not change the files of the current (active) log.

[Release For Windows](https://github.com/e-gaydarzhi-2077/Cobian_Reflector_Telegram/releases)

Example:
![image](https://user-images.githubusercontent.com/107859162/191241057-3c8876c5-0f69-46f4-9ca4-7f549bf2250c.png)

After you have downloaded and extracted the files, you need to modify the ```CobianReflectorTG.cfg``` file
Line 0 is the bot's api token
Line 1 is the group id
(Note the group id is written with a - sign)
(Id of personal correspondence is written without this sig)

Example:
![image](https://user-images.githubusercontent.com/107859162/191242438-9e199706-2d0d-414f-a027-6194cc7894e1.png)

Then add a finishing action (To the last task)
Run ```CobianReflectorTG.exe```

Example:
![image](https://user-images.githubusercontent.com/107859162/191242909-678be467-401b-496d-b97f-4ecd154c53d7.png)

