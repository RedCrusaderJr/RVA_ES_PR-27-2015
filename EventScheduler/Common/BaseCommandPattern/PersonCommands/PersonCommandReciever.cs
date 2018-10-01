using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Contracts;
using Common.IModels;
using Common.Models;
using Common.PersonCommands;

namespace Common.BaseCommandPattern.PersonCommands
{
    public class PersonCommandReciever : BaseCommandReceiver
    {
        public override Person Addition(Person personToAdd, IPersonServices proxy)
        {
            return proxy.AddPerson(personToAdd);
        }

        public override Person Modification(Person personToModify, IPersonServices proxy)
        {
            return proxy.ModifyPerson(personToModify);
        }

        public override Person Removal(Person personToRemove, IPersonServices proxy)
        {
            return proxy.DeletePerson(personToRemove);
        }
    }
}
