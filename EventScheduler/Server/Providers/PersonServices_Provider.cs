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
    class PersonServices_Provider : IPersonServices
    {
        //ADAPTIRA METODE BAZE...
        public bool AddPerson(Person person)
        {
            if (DbManager.Instance.AddPerson(person))
            {
                return true;
            }

            return false;
        }

        //ADAPTIRA METODE BAZE...
        public bool ModifyPerson(Person person)
        {
            if(DbManager.Instance.ModifyPerson(person))
            {
                return true;
            }

            return false;
        }

        public bool DeletePerson(Person person)
        {
            if (DbManager.Instance.DeletePerson(person))
            {
                return true;
            }

            return false;
        }

        //ADAPTIRA METODE BAZE...
        public Person GetSinglePerson(int id)
        {
            Person person = DbManager.Instance.GetSinglePerson(id);

            if(person == null)
            {
                throw new NullReferenceException();
            }

            return person;
        }

        public List<Person> GetAllPeople()
        {
            return DbManager.Instance.GetAllPeople().Values.ToList();
        }
    }
}
