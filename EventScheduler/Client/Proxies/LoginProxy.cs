using Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client.Proxies
{
    public class LoginProxy
    {
        public static ILoginService ConnectToLoginService()
        {
            NetTcpBinding binding = new NetTcpBinding()
            {
                CloseTimeout = new TimeSpan(0, 10, 0),
                OpenTimeout = new TimeSpan(0, 10, 0),
                ReceiveTimeout = new TimeSpan(0, 10, 0),
                SendTimeout = new TimeSpan(0, 10, 0),
            };
            ChannelFactory<ILoginService> loginChannelFactory = new ChannelFactory<ILoginService>(binding, new EndpointAddress("net.tcp://localhost:6000/ILoginService"));
            ILoginService loginProxy = loginChannelFactory.CreateChannel();
            return loginProxy;
        }
    }
}
