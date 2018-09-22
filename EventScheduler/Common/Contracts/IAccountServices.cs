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
        PersonWithAccount Login(string username, string password);

        [OperationContract]
        bool CreateNewAccount(PersonWithAccount person);

        [OperationContract]
        bool CreateAccountWithExistingPerson(PersonWithAccount person);

        [OperationContract]
        bool ModifyAccount(PersonWithAccount person);

        [OperationContract]
        bool DeleteAccount(PersonWithAccount person);

        [OperationContract]
        bool DeleteAccountWithPerson(PersonWithAccount person);

        [OperationContract]
        PersonWithAccount GetSingleAccount(String username);

        [OperationContract]
        List<PersonWithAccount> GetAllAccounts();
    }
}
