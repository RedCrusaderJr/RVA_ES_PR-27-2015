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
        Account AddAccount(Account accountToAdd);
        Account ModifyAccount(Account accountToModify);
        Account DeleteAccount(Account accountToDelete);
        Account GetSingleAccountByUsername(String accountUsername);
        List<Account> GetAllAccounts();
    }
}
