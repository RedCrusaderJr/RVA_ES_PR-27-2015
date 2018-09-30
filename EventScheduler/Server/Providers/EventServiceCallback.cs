using Common.Contracts;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Providers
{
    public class EventServiceCallback
    {
        #region Instance
        private static EventServiceCallback _instance;
        private static readonly object syncLock = new object();
        private EventServiceCallback()
        {
            _subscribers = new List<IEventServicesCallback>();
        }
        public static EventServiceCallback Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new EventServiceCallback();
                        }
                    }
                }

                return _instance;
            }
        }
        #endregion

        private List<IEventServicesCallback> _subscribers;

        public void Subscribe(IEventServicesCallback currentCallbackProxy)
        {
            if (!_subscribers.Contains(currentCallbackProxy))
            {
                _subscribers.Add(currentCallbackProxy);
            }
        }

        public void Unsubscribe(IEventServicesCallback currentCallbackProxy)
        {
            if (_subscribers.Contains(currentCallbackProxy))
            {
                _subscribers.Remove(currentCallbackProxy);
            }
        }

        public void NotifyAllEventAddition(Event addedEvent, IEventServicesCallback currentCallbackProxy)
        {
            if (_subscribers.Count > 0)
            {
                foreach (IEventServicesCallback sub in _subscribers)
                {
                    if (sub != currentCallbackProxy)
                    {
                        sub.NotifyEventAddition(addedEvent);
                    }
                }
            }
        }

        public void NotifyAllEventRemoval(Event removedEvent, IEventServicesCallback currentCallbackProxy)
        {
            if (_subscribers.Count > 0)
            {
                foreach (IEventServicesCallback sub in _subscribers)
                {
                    if (sub != currentCallbackProxy)
                    {
                        sub.NotifyEventRemoval(removedEvent);
                    }
                }
            }
        }

        public void NotifyAllEventModification(Event modifiedEvent, IEventServicesCallback currentCallbackProxy)
        {
            if (_subscribers.Count > 0)
            {
                foreach (IEventServicesCallback sub in _subscribers)
                {
                    if (sub != currentCallbackProxy)
                    {
                        sub.NotifyEventModification(modifiedEvent);
                    }
                }
            }
        }

    }
}
