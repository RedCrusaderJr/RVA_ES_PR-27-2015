using Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class AccountProxy
    {
        #region Instance
        private static AccountProxy _instance;
        private static readonly object syncLock = new object();
        private AccountProxy()
        {
            NetTcpBinding binding = new NetTcpBinding()
            {
                CloseTimeout = new TimeSpan(0, 10, 0),
                OpenTimeout = new TimeSpan(0, 10, 0),
                ReceiveTimeout = new TimeSpan(0, 10, 0),
                SendTimeout = new TimeSpan(0, 10, 0),
            };
            AccountServices = new ChannelFactory<IAccountServices>(binding, new EndpointAddress("net.tcp://localhost:6001/IAccountServices")).CreateChannel();
        }
        public static AccountProxy Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new AccountProxy();
                        }
                    }
                }

                return _instance;
            }
        }
        #endregion

        public IAccountServices AccountServices { get; }
    }
}
