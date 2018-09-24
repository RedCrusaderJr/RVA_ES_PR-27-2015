using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.IAccess
{
    public interface IAccountDbManager
    {
        Boolean AddAccount(Account accountToAdd);
        //Boolean AddAccount(Account accountToAdd, Person personToAdd);
        Boolean ModifyAccount(Account accountToModify);
        Boolean DeleteAccount(Account accountToDelete);
        Account GetSingleAccountByUsername(String accountUsername);
        List<Account> GetAllAccounts();
    }
}
