using Common.BaseClasses;
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
            foreach (PersonObserver p in Observers)
            {
                p.NotifyAdditon(this);
            }
        }

        public override void NotifyAllChange()
        {
            foreach (PersonObserver p in Observers)
            {
                p.NotifyChange(this);
            }
        }

        public override void NotifyAllRemoval()
        {
            foreach (PersonObserver p in Observers)
            {
                p.NotifyRemoval(this);
            }
        }
    }
}
