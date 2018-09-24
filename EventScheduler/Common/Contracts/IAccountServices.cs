using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contracts
{
    [ServiceContract]
    public interface IAccountServices
    {
        [OperationContract]
        Account Login(string username, string password);

        [OperationContract]
        bool CreateNewAccount(Account account);

        //[OperationContract]
        //bool CreateAccountWithExistingPerson(Account account);
        
        [OperationContract]
        bool ModifyAccount(Account account);

        [OperationContract]
        bool DeleteAccount(Account account);

        //[OperationContract]
        //bool DeleteAccountWithPerson(Account account);

        [OperationContract]
        Account GetSingleAccount(String username);

        [OperationContract]
        List<Account> GetAllAccounts();
    }
}
