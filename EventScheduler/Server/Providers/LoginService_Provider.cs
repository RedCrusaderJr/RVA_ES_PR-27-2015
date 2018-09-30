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
    class LoginService_Provider : ILoginService
    {
        public Account Login(string username, string password)
        {
            Account account = DbManager.Instance.GetSingleAccountByUsername(username);

            if (account != null)
            {
                if (account.Password.Equals(password))
                {
                    return account;
                }
            }
            return null;
        }
    }
}
