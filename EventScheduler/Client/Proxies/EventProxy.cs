using Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client.Proxies
{
    public class EventProxy
    {
        public static IEventServices ConnectToAccountService(InstanceContext instanceContext)
        {
            NetTcpBinding binding = new NetTcpBinding()
            {
                CloseTimeout = new TimeSpan(0, 10, 0),
                OpenTimeout = new TimeSpan(0, 10, 0),
                ReceiveTimeout = new TimeSpan(0, 10, 0),
                SendTimeout = new TimeSpan(0, 10, 0),
            };
            DuplexChannelFactory<IEventServices> eventChannelFactory = new DuplexChannelFactory<IEventServices>(instanceContext, binding, new EndpointAddress("net.tcp://localhost:6003/IEventServices"));
            IEventServices eventProxy = eventChannelFactory.CreateChannel();
            return eventProxy;
        }
    }
}
