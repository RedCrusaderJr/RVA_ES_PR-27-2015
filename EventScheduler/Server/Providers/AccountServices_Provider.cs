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
        public PersonWithAccount Login(string username, string password)
        {
            PersonWithAccount personWithAccount = DbManager.Instance.GetSinglePersonWithAccountByUsername(username);

            if(personWithAccount != null)
            {
                if(personWithAccount.Password.Equals(password))
                {
                    return personWithAccount;
                }
            }

            //throw new NullReferenceException();
            return null;
        }

        public bool CreateNewAccount(PersonWithAccount personWithAccountToCreate)
        {
            if(DbManager.Instance.AddPersonWithAccount(personWithAccountToCreate))
            {
                return true;
            }

            return false;
        }

        public bool CreateAccountWithExistingPerson(PersonWithAccount personWithAccountToCreate)
        {
            if(DbManager.Instance.DeletePerson(personWithAccountToCreate as Person))
            {
                if(DbManager.Instance.AddPersonWithAccount(personWithAccountToCreate))
                {
                    return true;
                }
            }

            return false;
        }

        public bool ModifyAccount(PersonWithAccount personWithAccountToModify)
        {
            if(DbManager.Instance.ModifyPersonWithAccount(personWithAccountToModify))
            {
                return true;
            }

            return false;
        }

        public bool DeleteAccount(PersonWithAccount accountToBeDeleted)
        {
            if(DbManager.Instance.DeletePersonWithAccount(accountToBeDeleted))
            {
                if(DbManager.Instance.AddPerson(accountToBeDeleted as Person))
                {
                    return true;
                }
            }

            return false;
        }

        public bool DeleteAccountWithPerson(PersonWithAccount personWithAccountToDelete)
        {
            if(DbManager.Instance.DeletePersonWithAccount(personWithAccountToDelete))
            {
                return true;
            }

            return false;
        }

        public PersonWithAccount GetSingleAccount(string username)
        {
            PersonWithAccount personWithAccount = DbManager.Instance.GetSinglePersonWithAccountByUsername(username);

            if (personWithAccount == null)
            {
                throw new NullReferenceException();
            }

            return personWithAccount;
        }

        public List<PersonWithAccount> GetAllAccounts()
        {
            return DbManager.Instance.GetAllPeopleWithAccounts().Values.ToList();
        }
    }
}
