using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Log4netHelper
    {
        public static ILog GetLogger([CallerFilePath]string filename = "")
        {
            return LogManager.GetLogger(filename);
        }
    }
}
