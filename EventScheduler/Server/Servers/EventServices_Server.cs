using Common.Contracts;
using Server.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server.Servers
{
    class EventServices_Server : BaseServer
    {
        public EventServices_Server() : base("EventServices")
        {
            NetTcpBinding binding = new NetTcpBinding()
            {
                CloseTimeout = new TimeSpan(0, 10, 0),
                OpenTimeout = new TimeSpan(0, 10, 0),
                ReceiveTimeout = new TimeSpan(0, 10, 0),
                SendTimeout = new TimeSpan(0, 10, 0),
            };
            _serviceHost = new ServiceHost(typeof(EventServices_Provider));
            _serviceHost.AddServiceEndpoint(typeof(IEventServices), binding, "net.tcp://localhost:6003/IEventServices");
        }
    }
}
