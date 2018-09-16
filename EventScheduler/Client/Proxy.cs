using Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Proxy
    {
        #region Instance
        private static Proxy _instance;
        private static readonly object syncLock = new object();
        private Proxy()
        {
            NetTcpBinding binding = new NetTcpBinding()
            {
                CloseTimeout = new TimeSpan(0,10,0),
                OpenTimeout = new TimeSpan(0, 10, 0),
                ReceiveTimeout = new TimeSpan(0, 10, 0),
                SendTimeout = new TimeSpan(0, 10, 0),
            };
            BasicOperations = new ChannelFactory<IBasicOperations>(binding, new EndpointAddress("net.tcp://localhost:6000/IBasicOperations")).CreateChannel();
        }
        public static Proxy Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new Proxy();
                        }
                    }
                }

                return _instance;
            }
        }
        #endregion

        public IBasicOperations BasicOperations;
    }
}
