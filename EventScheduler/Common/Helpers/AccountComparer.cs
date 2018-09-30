using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public class AccountComparer : IEqualityComparer<Account>
    {
        public bool Equals(Account x, Account y)
        {
            return x.Username == y.Username;
        }

        public int GetHashCode(Account obj)
        {
            return obj.GetHashCode();
        }
    }
}
