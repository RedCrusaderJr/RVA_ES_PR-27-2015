using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public enum ERole
    {
        REGULAR = 0,
        ADMIN
    }

    public enum EEventPriority
    {
        DEBUG,
        INFO,
        WARN,
        ERROR,
        FATAL
    }

    public enum EStringBuilder
    {
        CLIENT,
        SERVER
    }
}
