using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.IAccess
{
    public interface IPersonWithAccountDbManager
    {
        Boolean AddPersonWithAccount(PersonWithAccount personWithAccountToAdd);
        Boolean ModifyPersonWithAccount(PersonWithAccount personWithAccountToModify);
        Boolean DeletePersonWithAccount(PersonWithAccount personWithAccountToDelete);
        PersonWithAccount GetSinglePersonWithAccount(Int32 personWithAccountId);
        PersonWithAccount GetSinglePersonWithAccountByUsername(String personWithAccountUsername);
        Dictionary<Int32, PersonWithAccount> GetAllPeopleWithAccounts();
    }
}
