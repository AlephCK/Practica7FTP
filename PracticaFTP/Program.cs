using Bogus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;


namespace PracticaFTP
{
    class Program
    {

        static void Main(string[] args)
        {
            

            GenerarArchivo();
            Console.WriteLine("archivo generado con exito");
            Console.ReadKey();

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
                string localPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName) + @"\prueba.txt";
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


       
        static void GenerarArchivo()
        {
            string digitos = "0123456789";
            string digitos2 = "abcdefghijklmnñopqrstuwxyz";
            List<ClienteSeguro> clientes = new List<ClienteSeguro>();

            for (int i = 0; i< 50; i++) {

                var clienteSeguro = new Faker<ClienteSeguro>()
                .RuleFor(e => e.Cedula, f => f.Random.String2(11, digitos))
                .RuleFor(e => e.Nombre, f => f.Name.FirstName())
                .RuleFor(e => e.Apellido, f => f.Name.LastName())
                .RuleFor(e => e.Edad, f => f.Random.Number(18, 50))
                .RuleFor(e => e.Fecha_Ingreso, f => f.Date.Recent(100))
                .RuleFor(e => e.Tipo_Seguro, f => f.Random.String2(5, digitos2))
                .RuleFor(e => e.Estado, f => f.Random.String2(5, digitos2));

                clientes.Add(clienteSeguro.Generate());

            }

            string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName) + @"\prueba.txt";

            using (StreamWriter file = new StreamWriter(path))
            {
                string encabezado = $" Nombre | Apellido | Cédula | Edad | Fecha Ingreso | Tipo Seguro | Estado";
                file.WriteLine(encabezado);

                foreach (var clients in clientes)
                {
                    string client = $"{clients.Nombre}|{clients.Apellido}|{clients.Cedula}| {clients.Edad} | {clients.Fecha_Ingreso} | " +
                        $" {clients.Tipo_Seguro} | {clients.Estado}";

                    file.WriteLine(client);
                   
                }


            }

        }
    }
}

