using Common.Contracts;
using Common.Models;
using Common.PersonCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.BaseCommandPattern.PersonCommands
{
    public class ModifyPersonCommand : BasePersonCommand
    {
        public ModifyPersonCommand(BaseCommandReceiver commandReceiver, Person person, IPersonServices personProxy) : base(commandReceiver, person, personProxy) { }

        public override void Execute()
        {
            ReceiverParameter = PersonCommandReceiver.Modification(ReturnedState, PersonProxy);
        }

        public override void Unexecute()
        {
            ReturnedState = PersonCommandReceiver.Modification(ReceiverParameter, PersonProxy);
        }
    }
}
