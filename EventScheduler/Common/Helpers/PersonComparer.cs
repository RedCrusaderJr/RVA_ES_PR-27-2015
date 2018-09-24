using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public class PersonComparer : IEqualityComparer<Person>
    {
        //STA CE BITI SA KOPIJAMA OBJEKATA
        public bool Equals(Person x, Person y)
        {
            return x.JMBG == y.JMBG;
        }

        public int GetHashCode(Person obj)
        {
            return obj.GetHashCode();
        }
    }
}
