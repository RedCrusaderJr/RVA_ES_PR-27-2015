using Common.BaseCommandPattern;
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
    public abstract class BasePersonCommand : BaseCommand
    {
        protected BaseCommandReceiver PersonCommandReceiver;
        protected Person ReceiverParameter;
        protected IPersonServices PersonProxy;

        public BasePersonCommand(BaseCommandReceiver personCommandReceiver, Person receiverParameter, IPersonServices proxy)
        {
            PersonCommandReceiver = personCommandReceiver;
            ReceiverParameter = receiverParameter;
            PersonProxy = proxy;
        }
    }
}
