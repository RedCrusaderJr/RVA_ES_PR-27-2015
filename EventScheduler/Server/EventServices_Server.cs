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
    class EventServices_Server
    {
        private ServiceHost _serviceHost;

        public EventServices_Server()
        {
            NetTcpBinding binding = new NetTcpBinding()
            {
                CloseTimeout = new TimeSpan(0, 10, 0),
                OpenTimeout = new TimeSpan(0, 10, 0),
                ReceiveTimeout = new TimeSpan(0, 10, 0),
                SendTimeout = new TimeSpan(0, 10, 0),
            };
            _serviceHost = new ServiceHost(typeof(EventServices_Provider));
            _serviceHost.AddServiceEndpoint(typeof(IEventServices), binding, "net.tcp://localhost:6002/IEventServices");
        }

        public void Open()
        {
            try
            {
                _serviceHost.Open();
                Console.WriteLine($"EventServices host successfully started at {DateTime.Now}.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failure on opening EventServices host. Message: {e.Message}");
            }
        }

        public void Close()
        {
            try
            {
                _serviceHost.Close();
                Console.WriteLine($"EventServices host successfully closed at {DateTime.Now}.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failure on closing EventServices host. Message: {e.Message}");
            }
        }
    }
}
