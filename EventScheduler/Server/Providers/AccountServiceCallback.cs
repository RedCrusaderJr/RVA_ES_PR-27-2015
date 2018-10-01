using Common.Contracts;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Providers
{
    public class AccountServiceCallback
    {
        #region Instance
        private static AccountServiceCallback _instance;
        private static readonly object syncLock = new object();
        private AccountServiceCallback()
        {
            _subscribers = new List<IAccountServicesCallback>();
        }
        public static AccountServiceCallback Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new AccountServiceCallback();
                        }
                    }
                }

                return _instance;
            }
        }
        #endregion

        private List<IAccountServicesCallback> _subscribers;

        public void Subscribe(IAccountServicesCallback currentCallbackProxy)
        {
            if (!_subscribers.Contains(currentCallbackProxy))
            {
                _subscribers.Add(currentCallbackProxy);
            }
        }

        public void Unsubscribe(IAccountServicesCallback currentCallbackProxy)
        {
            if (_subscribers.Contains(currentCallbackProxy))
            {
                _subscribers.Remove(currentCallbackProxy);
            }
        }

        public void NotifyAllAccountAddition(Account addedAccount, IAccountServicesCallback currentCallbackProxy)
        {
            if (_subscribers.Count > 0)
            {
                foreach (IAccountServicesCallback sub in _subscribers)
                {
                    sub.NotifyAccountAddition(addedAccount);
                    //if (sub != currentCallbackProxy)
                    //{
                    //    sub.NotifyAccountAddition(addedAccount);
                    //}
                }
            }
        }

        public void NotifyAllAccountRemoval(Account removedAccount, IAccountServicesCallback currentCallbackProxy)
        {
            if (_subscribers.Count > 0)
            {
                foreach (IAccountServicesCallback sub in _subscribers)
                {
                    sub.NotifyAccountRemoval(removedAccount);
                    //if (sub != currentCallbackProxy)
                    //{
                    //    sub.NotifyAccountRemoval(removedAccount);
                    //}
                }
            }
        }

        public void NotifyAllAccountModification(Account modifiedAccount, IAccountServicesCallback currentCallbackProxy)
        {
            if (_subscribers.Count > 0)
            {
                foreach (IAccountServicesCallback sub in _subscribers)
                {
                    sub.NotifyAccountModification(modifiedAccount);
                    //if (sub != currentCallbackProxy)
                    //{
                    //    sub.NotifyAccountModification(modifiedAccount);
                    //}
                }
            }
        }

    }
}
