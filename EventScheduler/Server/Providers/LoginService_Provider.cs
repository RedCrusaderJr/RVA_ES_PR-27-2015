using Common.Contracts;
using Common.Models;
using log4net;
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
        private static readonly ILog logger = Log4netHelper.GetLogger();

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
