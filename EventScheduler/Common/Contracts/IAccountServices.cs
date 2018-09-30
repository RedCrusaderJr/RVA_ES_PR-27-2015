using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contracts
{
    [ServiceContract(CallbackContract = typeof(IAccountServicesCallback))]
    public interface IAccountServices
    {
        [OperationContract]
        Account CreateNewAccount(Account account);

        [OperationContract]
        Account ModifyAccount(Account account);

        [OperationContract]
        Account DeleteAccount(Account account);

        [OperationContract]
        Account GetSingleAccount(String username);

        [OperationContract]
        List<Account> GetAllAccounts();

        [OperationContract]
        void Subscribe();

        [OperationContract]
        void Unsubscribe();
    }

    public interface IAccountServicesCallback
    {
        [OperationContract]
        void NotifyAccountAddition(Account addedAccount);

        [OperationContract]
        void NotifyAccountRemoval(Account removedAccount);

        [OperationContract]
        void NotifyAccountModification(Account modifiedAccount);
    }
}
