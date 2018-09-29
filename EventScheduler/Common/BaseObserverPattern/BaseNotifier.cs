using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.BaseObserverPattern
{
    public abstract class BaseNotifier
    {
        public List<IObserverPattern> Observers { get; protected set; }

        public BaseNotifier()
        {
            Observers = new List<IObserverPattern>();
        }

        public abstract void NotifyAllAdditon();
        public abstract void NotifyAllChange();
        public abstract void NotifyAllRemoval();
        public virtual void RegisterObserver(IObserverPattern observer)
        {
            Observers.Add(observer);
        }

        public virtual void UnregisterObserver(IObserverPattern observer)
        {
            Observers.Remove(observer);
        }
    }
}
