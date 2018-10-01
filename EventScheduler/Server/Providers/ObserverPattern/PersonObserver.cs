using Common.BaseObserverPattern;
using Common.Helpers;
using Common.Models;
using Server.Access;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Providers.ObserverPattern
{
    public class PersonObserver : Person, IObserverPattern
    {
        public PersonObserver() : base() { }
        public PersonObserver(Person person) : base(person) { }

        public void NotifyAdditon(BaseNotifier notifier)
        {
            Event eventToBeNotifiedAbout = ((EventNotifier)notifier).EvetToBeNotifiedAbout;

            if (!eventToBeNotifiedAbout.Participants.Contains(this, new PersonComparer()))
            {
                return;
            }

            if (IsAvailableForEvent(eventToBeNotifiedAbout.ScheduledDateTimeBeging, eventToBeNotifiedAbout.ScheduledDateTimeEnd))
            {
                if(ScheduleParticipationInEvent(eventToBeNotifiedAbout))
                {
                    DbManager.Instance.ModifyPerson(this);
                }
            }
        }

        public void NotifyChange(BaseNotifier notifier)
        {
            Event eventToBeNotifiedAbout = ((EventNotifier)notifier).EvetToBeNotifiedAbout;

            if (eventToBeNotifiedAbout.Participants.Contains(this, new PersonComparer()))
            {
                if(ScheduledEvents.Contains(eventToBeNotifiedAbout, new EventComparer()))
                {
                    if(IsAvailableForEventExcludingOneSpecificEvent(eventToBeNotifiedAbout.ScheduledDateTimeBeging, eventToBeNotifiedAbout.ScheduledDateTimeEnd, eventToBeNotifiedAbout))
                    {
                        if(CancleParticipationInEvent(eventToBeNotifiedAbout) && ScheduleParticipationInEvent(eventToBeNotifiedAbout))
                        {
                            DbManager.Instance.ModifyPerson(this);
                        }
                    }
                    else
                    {
                        if(CancleParticipationInEvent(eventToBeNotifiedAbout))
                        {
                            DbManager.Instance.ModifyPerson(this);
                        }
                    }
                }
                else
                {
                    if(IsAvailableForEvent(eventToBeNotifiedAbout.ScheduledDateTimeBeging, eventToBeNotifiedAbout.ScheduledDateTimeEnd))
                    {
                        if(ScheduleParticipationInEvent(eventToBeNotifiedAbout))
                        {
                            DbManager.Instance.ModifyPerson(this);
                        }
                    }
                }
            }
            else
            {
                if(ScheduledEvents.Contains(eventToBeNotifiedAbout, new EventComparer()))
                {
                    if(CancleParticipationInEvent(eventToBeNotifiedAbout))
                    {
                        DbManager.Instance.ModifyPerson(this);
                    }
                }
            }

        }

        public void NotifyRemoval(BaseNotifier notifier)
        {
            Event eventToBeNotifiedAbout = ((EventNotifier)notifier).EvetToBeNotifiedAbout;

            if (!eventToBeNotifiedAbout.Participants.Contains(this, new PersonComparer()))
            {
                return;
            }

            if(!ScheduledEvents.Contains(eventToBeNotifiedAbout, new EventComparer()))
            {
                return;
            }

            CancleParticipationInEvent(eventToBeNotifiedAbout);
            DbManager.Instance.ModifyPerson(this);
        }
    }
}
