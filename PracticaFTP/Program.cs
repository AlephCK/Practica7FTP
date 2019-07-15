﻿using System;
using System.IO;
using System.Net;
using System.Text;


namespace PracticaFTP
{
    class Program
    {
        static void Main(string[] args)
        {


            Chilkat.Global glob = new Chilkat.Global();
            bool successUnlock = glob.UnlockBundle("Anything for 30-day trial");
            if (successUnlock != true)
            {
                Console.WriteLine(glob.LastErrorText);
                return;
            }

            int status = glob.UnlockStatus;
            if (status == 2)
            {
                Console.WriteLine("Unlocked using purchased unlock code.");
            }
            else
            {
                Chilkat.Ftp2 ftp = new Chilkat.Ftp2();

                ftp.Hostname = "localhost";
                ftp.Username = "unapec";
                ftp.Password = "admin";

                // Connect and login to the FTP server.
                bool success = ftp.Connect();
                if (success != true)
                {
                    Console.WriteLine(ftp.LastErrorText);
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                    return;
                }

                // Change to the remote directory where the file will be uploaded.
                success = ftp.ChangeRemoteDir("doc");
                if (success != true)
                {
                    Console.WriteLine(ftp.LastErrorText);
                    return;
                }

                // Upload a file.
                string localPath = "c:/Dell Downloads/prueba.txt";
                string remoteFilename = "prueba.txt";

                success = ftp.PutFile(localPath, remoteFilename);
                if (success != true)
                {
                    Console.WriteLine(ftp.LastErrorText);
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                    return;
                }

                success = ftp.Disconnect();

                Console.WriteLine("File Uploaded!");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();

            }


        }
    }
}