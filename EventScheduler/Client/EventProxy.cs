﻿using Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class EventProxy
    {
        #region Instance
        private static EventProxy _instance;
        private static readonly object syncLock = new object();
        private EventProxy()
        {
            NetTcpBinding binding = new NetTcpBinding()
            {
                CloseTimeout = new TimeSpan(0, 10, 0),
                OpenTimeout = new TimeSpan(0, 10, 0),
                ReceiveTimeout = new TimeSpan(0, 10, 0),
                SendTimeout = new TimeSpan(0, 10, 0),
            };
            EventServices = new ChannelFactory<IEventServices>(binding, new EndpointAddress("net.tcp://localhost:6002/IEventServices")).CreateChannel();
        }
        public static EventProxy Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new EventProxy();
                        }
                    }
                }

                return _instance;
            }
        }
        #endregion

        public IEventServices EventServices { get; }
    }
}
