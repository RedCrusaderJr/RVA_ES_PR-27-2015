using Common.BaseObserverPattern;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Providers.ObserverPattern
{
    public class EventNotifier : BaseNotifier
    {
        public Event EvetToBeNotifiedAbout { get; set; } 

        public EventNotifier() : base() { }

        public override void NotifyAllAdditon()
        {
            for(int i=0; i < Observers.Count; i ++)
            {
                PersonObserver p = Observers[i] as PersonObserver;
                p.NotifyAdditon(this);
            }
        }

        public override void NotifyAllChange()
        {
            for (int i = 0; i < Observers.Count; i++)
            {
                PersonObserver p = Observers[i] as PersonObserver;
                p.NotifyChange(this);
            }
        }

        public override void NotifyAllRemoval()
        {
            for (int i = 0; i < Observers.Count; i++)
            {
                PersonObserver p = Observers[i] as PersonObserver;
                p.NotifyRemoval(this);
            }
        }
    }
}
