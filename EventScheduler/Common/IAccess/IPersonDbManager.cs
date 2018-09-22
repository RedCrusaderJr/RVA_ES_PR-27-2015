using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.IAccess
{
    public interface IPersonDbManager
    {
        Boolean AddPerson(Person personToAdd);
        Boolean ModifyPerson(Person personToModify);
        Boolean DeletePerson(Person personToDelete);
        Person GetSinglePerson(Int32 personId);
        Dictionary<Int32, Person> GetAllPeople();
    }
}
