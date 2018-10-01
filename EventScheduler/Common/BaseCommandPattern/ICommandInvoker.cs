using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.BaseCommandPattern
{
    public interface ICommandInvoker
    {
        Int32 CurrentCommand { get; }
        List<BaseCommand> ListOfCommands { get; }

        Boolean Undo();
        Boolean Redo();
        void RegisterCommand(BaseCommand command);
    }
}
