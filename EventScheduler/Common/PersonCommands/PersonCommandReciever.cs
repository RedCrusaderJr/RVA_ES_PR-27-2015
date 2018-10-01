using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Contracts;
using Common.IModels;
using Common.Models;

namespace Common.PersonCommands
{
    public class PersonCommandReciever : BaseCommandReceiver
    {
        public override void Addition(Person personToAdd, IPersonServices proxy)
        {
            proxy.AddPerson(personToAdd);
        }

        public override void Modification(Person personToModify, IPersonServices proxy)
        {
            proxy.ModifyPerson(personToModify);
        }

        public override void Removal(Person personToRemove, IPersonServices proxy)
        {
            proxy.DeletePerson(personToRemove);
        }
    }
}
