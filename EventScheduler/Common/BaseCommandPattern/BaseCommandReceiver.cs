using Common.Contracts;
using Common.IModels;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.PersonCommands
{
    public abstract class BaseCommandReceiver
    {
        public abstract Person Addition(Person personToAdd, IPersonServices proxy);
        public abstract Person Removal(Person personToRemove, IPersonServices proxy);
        public abstract Person Modification(Person personToModify, IPersonServices proxy);
    }
}
