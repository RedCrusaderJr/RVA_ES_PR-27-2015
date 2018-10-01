using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.BaseCommandPattern
{
    public abstract class BaseCommand
    {
        public abstract void Execute();
        public abstract void Unexecute();
    }
}
