using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string username = ConfigurationManager.AppSettings["FtpUsername"];
            string pass = ConfigurationManager.AppSettings["FtpPassword"];
            string path = ConfigurationManager.AppSettings["FtpPath"];
            int port = 43827;

            using (SftpClient client = new SftpClient(path, port, username, pass))
            {
                client.Connect();
                client.ChangeDirectory("/exports");
                var files = client.ListDirectory(client.WorkingDirectory);
                foreach (var file in files)
                {
                    if (file.FullName.Contains(".csv"))
                    {
                        using (FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory +@"\"+ file.Name, FileMode.OpenOrCreate))
                        {
                            client.BufferSize = 4 * 1024;
                            client.DownloadFile(file.FullName, fs);
                            fs.Close();
                        }
                    }
                }
            }


        }
    }
}
