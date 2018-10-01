using Common.Contracts;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.PersonCommands
{
    public class AddPersonCommand : BasePersonCommand
    {

        public AddPersonCommand(BaseCommandReceiver commandReceiver, Person person, IPersonServices personProxy) : base(commandReceiver, person, personProxy) { }

        public override void Execute()
        {
            PersonCommandReceiver.Addition(ReceiverParameter, PersonProxy);
        }

        public override void Unexecute()
        {
            PersonCommandReceiver.Removal(ReceiverParameter, PersonProxy);
        }
    }
}
