using Common.Contracts;
using Common.Models;
using Server.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class BasicOperations_Provider : IBasicOperations
    {
        //RETURNING NULL
        public Person Login(string username, string password)
        {
            Person person = DBManager.Instance.GetSinglePeson(username);

            if(person != null)
            {
                if(person.Password.Equals(password))
                {
                    return person;
                }
            }

            return null;
        }

        public bool AddPerson(Person person)
        {
            if (DBManager.Instance.AddPerson(person))
            {
                return true;
            }

            return false;
        }

        public bool ModifyPerson(Person person)
        {
            if(DBManager.Instance.ModifyPerson(person))
            {
                return true;
            }

            return false;
        }
    }
}
