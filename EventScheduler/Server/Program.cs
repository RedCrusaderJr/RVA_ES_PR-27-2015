using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Directory.GetCurrentDirectory();
            path = path.Substring(0, path.LastIndexOf("bin")) + "DB";
            AppDomain.CurrentDomain.SetData("DataDirectory", path);

            AccountServices_Server accountServer = new AccountServices_Server();
            PersonServices_Server personServer = new PersonServices_Server();
            EventServices_Server eventServer = new EventServices_Server();
            accountServer.Open();
            personServer.Open();
            eventServer.Open();

            Console.ReadLine();
            accountServer.Close();
            personServer.Close();
            eventServer.Close();
        }
    }
}
