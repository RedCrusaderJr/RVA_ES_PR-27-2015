using Server.Servers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            using (LoginService_Server loginServer = new LoginService_Server())
            using (AccountServices_Server accountServer = new AccountServices_Server())
            using (PersonServices_Server personServer = new PersonServices_Server())
            using (EventServices_Server eventServer = new EventServices_Server())
            {
                loginServer.Open();
                accountServer.Open();
                personServer.Open();
                eventServer.Open();

                //Process.Start(@"C:\Users\dimitrijemitic1996\source\repos\RVA\RVA_ES_PR-27-2015\EventScheduler\Client\bin\Debug\Client");
                Console.ReadLine();
            }
        }
    }
}
