using System;
using System.ServiceModel;

namespace Server.Servers
{
    internal abstract class BaseServer : IDisposable
    {
        protected ServiceHost _serviceHost;
        private String _serviceName;

        public BaseServer(String serviceName)
        {
            _serviceName = serviceName;
        }

        public void Open()
        {
            try
            {
                _serviceHost.Open();
                Console.WriteLine($"{_serviceName} host successfully started at {DateTime.Now}.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failure on opening {_serviceName} host. Message: {e.Message}");
            }
        }

        public void Close()
        {
            try
            {
                _serviceHost.Close();
                Console.WriteLine($"{_serviceName} host successfully started at {DateTime.Now}.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failure on opening {_serviceName} host. Message: {e.Message}");
            }
        }

        public void Dispose()
        {
            this.Close();
        }
    }
}