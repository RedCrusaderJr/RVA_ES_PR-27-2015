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
        Person AddPerson(Person personToAdd);
        Person ModifyPerson(Person personToModify);
        Person DeletePerson(Person personToDelete);
        Person GetSinglePerson(String jmbg);
        List<Person> GetAllPeople();
    }
}
