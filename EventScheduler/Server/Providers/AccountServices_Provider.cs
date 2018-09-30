using Common.Contracts;
using Common.Models;
using Server.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server.Providers
{
    [ServiceBehavior(ConcurrencyMode=ConcurrencyMode.Reentrant)]
    class AccountServices_Provider : IAccountServices
    {
        public Account CreateNewAccount(Account accountToCreate)
        {
            Account createdAccount = DbManager.Instance.AddAccount(accountToCreate);
            if (createdAccount != null)
            {
                IAccountServicesCallback currentCallbackProxy = OperationContext.Current.GetCallbackChannel<IAccountServicesCallback>();
                AccountServiceCallback.Instance.NotifyAllAccountAddition(createdAccount, currentCallbackProxy);

                return createdAccount;
            }

            return null;
        }

        public Account ModifyAccount(Account accountToModify)
        {
            Account modifiedAccount = DbManager.Instance.ModifyAccount(accountToModify);
            if (modifiedAccount != null)
            {
                IAccountServicesCallback currentCallbackProxy = OperationContext.Current.GetCallbackChannel<IAccountServicesCallback>();
                AccountServiceCallback.Instance.NotifyAllAccountModification(modifiedAccount, currentCallbackProxy);

                return modifiedAccount;
            }

            return null;
        }

        public Account DeleteAccount(Account accountToBeDeleted)
        {
            Account deletedAccount = DbManager.Instance.DeleteAccount(accountToBeDeleted);
            if (deletedAccount != null)
            {
                IAccountServicesCallback currentCallbackProxy = OperationContext.Current.GetCallbackChannel<IAccountServicesCallback>();
                AccountServiceCallback.Instance.NotifyAllAccountRemoval(deletedAccount, currentCallbackProxy);

                return deletedAccount;   
            }

            return null;
        }

        public Account GetSingleAccount(string username)
        {
            Account account = DbManager.Instance.GetSingleAccountByUsername(username);

            if (account == null)
            {
                return null;
            }

            return account;
        }

        public List<Account> GetAllAccounts()
        {
            return DbManager.Instance.GetAllAccounts();
        }

        public void Subscribe()
        {
            IAccountServicesCallback currentCallbackProxy = OperationContext.Current.GetCallbackChannel<IAccountServicesCallback>();
            AccountServiceCallback.Instance.Subscribe(currentCallbackProxy);
        }

        public void Unsubscribe()
        {
            IAccountServicesCallback currentCallbackProxy = OperationContext.Current.GetCallbackChannel<IAccountServicesCallback>();
            AccountServiceCallback.Instance.Unsubscribe(currentCallbackProxy);
        }
    }
}
