using Common.Contracts;
using Server.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class PersonServices_Server
    {
        private ServiceHost _serviceHost;

        public PersonServices_Server()
        {
            NetTcpBinding binding = new NetTcpBinding()
            {
                CloseTimeout = new TimeSpan(0, 10, 0),
                OpenTimeout = new TimeSpan(0, 10, 0),
                ReceiveTimeout = new TimeSpan(0, 10, 0),
                SendTimeout = new TimeSpan(0, 10, 0),
            };
            _serviceHost = new ServiceHost(typeof(PersonServices_Provider));
            _serviceHost.AddServiceEndpoint(typeof(IPersonServices), binding, "net.tcp://localhost:6000/IPersonServices");
        }

        public void Open()
        {
            try
            {
                _serviceHost.Open();
                Console.WriteLine($"PersonServices host successfully started at {DateTime.Now}.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failure on opening PersonServices host. Message: {e.Message}");
            }
        }

        public void Close()
        {
            try
            {
                _serviceHost.Close();
                Console.WriteLine($"PersonServices host successfully closed at {DateTime.Now}.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failure on closing PersonServices host. Message: {e.Message}");
            }
        }
    }
}
