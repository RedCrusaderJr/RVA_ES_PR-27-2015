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
    class AccountServices_Server : BaseServer
    {
        public AccountServices_Server() : base("AccountServices")
        {
            NetTcpBinding binding = new NetTcpBinding()
            {
                CloseTimeout = new TimeSpan(0, 10, 0),
                OpenTimeout = new TimeSpan(0, 10, 0),
                ReceiveTimeout = new TimeSpan(0, 10, 0),
                SendTimeout = new TimeSpan(0, 10, 0),
            };
            _serviceHost = new ServiceHost(typeof(AccountServices_Provider));
            _serviceHost.AddServiceEndpoint(typeof(IAccountServices), binding, "net.tcp://localhost:6001/IAccountServices");
        }
    }
}
