using Common.Contracts;
using Common.Helpers;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server.Providers
{
    public class PersonServiceCallback
    {
        #region Instance
        private static PersonServiceCallback _instance;
        private static readonly object syncLock = new object();
        private PersonServiceCallback()
        {
            _subscribers = new List<IPersonServicesCallback>();
        }
        public static PersonServiceCallback Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new PersonServiceCallback();
                        }
                    }
                }

                return _instance;
            }
        }
        #endregion

        private List<IPersonServicesCallback> _subscribers;

        public void Subscribe(IPersonServicesCallback currentCallbackProxy)
        {
            if (!_subscribers.Contains(currentCallbackProxy))
            {
                _subscribers.Add(currentCallbackProxy);
            }

            UpdateHelper.Instance.Limit = _subscribers.Count;
        }

        public void Unsubscribe(IPersonServicesCallback currentCallbackProxy)
        {
            if (_subscribers.Contains(currentCallbackProxy))
            {
                _subscribers.Remove(currentCallbackProxy);
            }

            UpdateHelper.Instance.Limit = _subscribers.Count;
        }

        public void NotifyAllPersonAddition(Person addedPerson, IPersonServicesCallback currentCallbackProxy)
        {
            addedPerson.ScheduledEvents.ForEach(e => e.Participants.ForEach(p => p.ScheduledEvents.ForEach(ev => ev.Participants = new List<Person>())));

            if (_subscribers.Count > 0)
            {
                foreach(IPersonServicesCallback sub in _subscribers)
                {
                    sub.NotifyPersonAddition(addedPerson);
                    /*
                    if (sub != currentCallbackProxy)
                    {
                        sub.NotifyPersonAddition(addedPerson);
                    }
                    */
                }
            }
        }

        public void NotifyAllPersonDuplication(Person addedPerson, IPersonServicesCallback currentCallbackProxy)
        {
            addedPerson.ScheduledEvents.ForEach(e => e.Participants.ForEach(p => p.ScheduledEvents.ForEach(ev => ev.Participants = new List<Person>())));

            if (_subscribers.Count > 0)
            {
                foreach (IPersonServicesCallback sub in _subscribers)
                {
                    sub.NotifyPersonDuplicate(addedPerson);
                    //if (sub != currentCallbackProxy)
                    //{
                    //    sub.NotifyPersonDuplicate(addedPerson);
                    //}
                }
            }
        }

        public void NotifyAllPersonRemoval(Person removedPerson, IPersonServicesCallback currentCallbackProxy)
        {
            removedPerson.ScheduledEvents.ForEach(e => e.Participants.ForEach(p => p.ScheduledEvents.ForEach(ev => ev.Participants = new List<Person>())));

            if (_subscribers.Count > 0)
            {
                foreach (IPersonServicesCallback sub in _subscribers)
                {
                    sub.NotifyPersonRemoval(removedPerson);
                    //if (sub != currentCallbackProxy)
                    //{
                    //    sub.NotifyPersonRemoval(removedPerson);
                    //}
                }
            }
        }

        public void NotifyAllPersonModification(Person modifiedPerson, IPersonServicesCallback currentCallbackProxy)
        {
            modifiedPerson.ScheduledEvents.ForEach(e => e.Participants.ForEach(p => p.ScheduledEvents.ForEach(ev => ev.Participants = new List<Person>())));

            if (_subscribers.Count > 0)
            {
                foreach (IPersonServicesCallback sub in _subscribers)
                {
                    sub.NotifyPersonModification(modifiedPerson);
                    //if (sub != currentCallbackProxy)
                    //{
                    //    sub.NotifyPersonModification(modifiedPerson);
                    //}
                }
            }
        }
    }
}
