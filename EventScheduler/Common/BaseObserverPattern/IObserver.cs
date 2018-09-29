using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.BaseObserverPattern
{
    public interface IObserverPattern
    {
        void NotifyAdditon(BaseNotifier notifier);
        void NotifyChange(BaseNotifier notifier);
        void NotifyRemoval(BaseNotifier notifier);
    }
}
