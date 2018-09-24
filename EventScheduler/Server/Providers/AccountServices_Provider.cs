using Common.Contracts;
using Common.Models;
using Server.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Providers
{
    class AccountServices_Provider : IAccountServices
    {
        //RETURNING NULL
        public Account Login(string username, string password)
        {
            Account account = DbManager.Instance.GetSingleAccountByUsername(username);

            if(account != null)
            {
                if(account.Password.Equals(password))
                {
                    return account;
                }
            }

            //throw new NullReferenceException();
            return null;
        }

        public bool CreateNewAccount(Account accountToCreate)
        {
            if(DbManager.Instance.AddAccount(accountToCreate))
            {
                return true;
            }

            return false;
        }
        /*
        public bool CreateAccountWithExistingPerson(Account accountToCreate)
        {
            if (DbManager.Instance.AddAccount(accountToCreate))
            {
                return true;
            }

            return false;
        }
        */
        public bool ModifyAccount(Account accountToModify)
        {
            if(DbManager.Instance.ModifyAccount(accountToModify))
            {
                return true;
            }

            return false;
        }

        public bool DeleteAccount(Account accountToBeDeleted)
        {
            if(DbManager.Instance.DeleteAccount(accountToBeDeleted))
            {
                if(DbManager.Instance.DeleteAccount(accountToBeDeleted))
                {
                    return true;
                }
            }

            return false;
        }

        /*
        public bool DeleteAccountWithPerson(Account accountToDelete)
        {
            if(!DbManager.Instance.DeleteAccount(accountToDelete))
            {
                return false;
            }

            if (!DbManager.Instance.DeletePerson(accountToDelete.PersonWithAccount))
            {
                return false;
            }

            return true;
        }
        */

        public Account GetSingleAccount(string username)
        {
            Account account = DbManager.Instance.GetSingleAccountByUsername(username);

            if (account == null)
            {
                throw new NullReferenceException();
            }

            return account;
        }

        public List<Account> GetAllAccounts()
        {
            return DbManager.Instance.GetAllAccounts();
        }
    }
}
