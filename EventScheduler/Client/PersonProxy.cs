using Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class PersonProxy
    {
        #region Instance
        private static PersonProxy _instance;
        private static readonly object syncLock = new object();
        private PersonProxy()
        {
            NetTcpBinding binding = new NetTcpBinding()
            {
                CloseTimeout = new TimeSpan(0,10,0),
                OpenTimeout = new TimeSpan(0, 10, 0),
                ReceiveTimeout = new TimeSpan(0, 10, 0),
                SendTimeout = new TimeSpan(0, 10, 0),
            };
            PersonServices = new ChannelFactory<IPersonServices>(binding, new EndpointAddress("net.tcp://localhost:6000/IPersonServices")).CreateChannel();
        }
        public static PersonProxy Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new PersonProxy();
                        }
                    }
                }

                return _instance;
            }
        }
        #endregion

        public IPersonServices PersonServices { get; }
    }
}
