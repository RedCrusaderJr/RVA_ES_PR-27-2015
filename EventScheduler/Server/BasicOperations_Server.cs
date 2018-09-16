using Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class BasicOperations_Server
    {
        private ServiceHost _serviceHost;

        public BasicOperations_Server()
        {
            NetTcpBinding binding = new NetTcpBinding()
            {
                CloseTimeout = new TimeSpan(0, 10, 0),
                OpenTimeout = new TimeSpan(0, 10, 0),
                ReceiveTimeout = new TimeSpan(0, 10, 0),
                SendTimeout = new TimeSpan(0, 10, 0),
            };
            _serviceHost = new ServiceHost(typeof(BasicOperations_Provider));
            _serviceHost.AddServiceEndpoint(typeof(IBasicOperations), binding, "net.tcp://localhost:6000/IBasicOperations");
        }

        public void Open()
        {
            try
            {
                _serviceHost.Open();
                Console.WriteLine($"Service host successfully started at {DateTime.Now}.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failure on opening service host. Message: {e.Message}");
            }
        }

        public void Close()
        {
            try
            {
                _serviceHost.Close();
                Console.WriteLine($"Service host successfully closed at {DateTime.Now}.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failure on closing service host. Message: {e.Message}");
            }
        }
    }
}
