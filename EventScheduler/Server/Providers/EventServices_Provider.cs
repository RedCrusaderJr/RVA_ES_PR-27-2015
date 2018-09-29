using Common.Contracts;
using Common.Helpers;
using Common.Models;
using Server.Access;
using Server.Providers.ObserverPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Providers
{
    public class EventServices_Provider : IEventServices 
    {
        public Event ScheduleEvent(Event eventToSchedule, List<Person> participants)
        {
            List<Person> participantsToAdd = participants.Where(p => !eventToSchedule.Participants.Contains(p, new PersonComparer())).ToList();
            participantsToAdd.ForEach(p => eventToSchedule.AddParticipant(p));

            Event scheduledEvent = DbManager.Instance.AddEvent(eventToSchedule);
            if (scheduledEvent != null)
            {
                //TODO: NOTIFY ALL PERSONS THAT NEED TO BE NOTIFIED....
                scheduledEvent.Participants.ForEach(p => p.ScheduledEvents = new List<Event>());
                return scheduledEvent;
            }

            return null;
        }

        public Event CancleEvent(Event eventToCancle)
        {
            Event cancledEvent = DbManager.Instance.DeleteEvent(eventToCancle);
            if (cancledEvent != null)
            {
                //TODO: NOTIFY ALL PERSONS THAT NEED TO BE NOTIFIED....
                cancledEvent.Participants.ForEach(p => p.ScheduledEvents = new List<Event>());
                return cancledEvent;
            }

            return null;
        }

        public Event EditEvent(Event eventToEdit)
         {
            Event editedEvent = DbManager.Instance.ModifyEvent(eventToEdit);
            if (editedEvent != null)
            {
                //TODO: NOTIFY...
                editedEvent.Participants.ForEach(p => p.ScheduledEvents = new List<Event>());
                return editedEvent;
            }

            return null;
        }

        public Event RescheduleEvent(Int32 eventId, DateTime newBegining, DateTime newEnd)
        {
            Event foundEvent = DbManager.Instance.GetSingleEvent(eventId);
            if(foundEvent == null)
            {
                return null;
            }

            foundEvent.ScheduledDateTimeBeging = newBegining;
            foundEvent.ScheduledDateTimeEnd = newEnd;
            foundEvent.LastEditTimeStamp = DateTime.Now;

            Event rescheduledEvent = DbManager.Instance.ModifyEvent(foundEvent);
            if(rescheduledEvent != null)
            {
                //TODO: NOTIFY
                rescheduledEvent.Participants.ForEach(p => p.ScheduledEvents = new List<Event>());
                return rescheduledEvent;
            }

            return null;
        }

        public Event AddParticipants(Int32 eventId, List<Person> participants)
        {
            Event foundEvent = DbManager.Instance.GetSingleEvent(eventId);
            if (foundEvent == null)
            {
                return null;
            }

            List<Person> participantsToAdd = participants.Where(p => !foundEvent.Participants.Contains(p, new PersonComparer())).ToList();
            participantsToAdd.ForEach(p => foundEvent.AddParticipant(p));

            Event modifiedEvent = DbManager.Instance.ModifyEvent(foundEvent);
            if(modifiedEvent != null)
            {
                //TODO: NOTIFY
                modifiedEvent.Participants.ForEach(p => p.ScheduledEvents = new List<Event>());
                return modifiedEvent;
            }

            return null;
        }

        public Event RemoveParticipants(Int32 eventId, List<Person> participants)
        {
            Event foundEvent = DbManager.Instance.GetSingleEvent(eventId);
            if (foundEvent == null)
            {
                return null;
            }

            List<Person> participantsToRemove = participants.Where(p => foundEvent.Participants.Contains(p, new PersonComparer())).ToList();
            participantsToRemove.ForEach(p => foundEvent.RemoveParticipant(p));

            Event modifiedEvent = DbManager.Instance.ModifyEvent(foundEvent);
            if (modifiedEvent != null)
            {
                //TODO: NOTIFY
                modifiedEvent.Participants.ForEach(p => p.ScheduledEvents = new List<Event>());
                return modifiedEvent;
            }

            return null;
        }

        public Event EditEventDescription(Int32 eventId, string newDescription)
        {
            Event foundEvent = DbManager.Instance.GetSingleEvent(eventId);
            if (foundEvent == null)
            {
                return null;
            }

            foundEvent.Description = newDescription;

            Event modifiedEvent = DbManager.Instance.ModifyEvent(foundEvent);
            if (modifiedEvent != null)
            {
                modifiedEvent.Participants.ForEach(p => p.ScheduledEvents = new List<Event>());
                return modifiedEvent;
            }

            return null;
        }

        /*
        public Event GetSingleEvent(Int32 eventId)
        {
            Event foundEvent = DbManager.Instance.GetSingleEvent(eventId);

            if (foundEvent == null)
            {
                throw new NullReferenceException();
            }

            return foundEvent;
        }

        
        public List<Event> GetAllEvents()
        {
            return DbManager.Instance.GetAllEvents();
        }
        */
        public Event GetSingleEvent(int eventId)
        { 
            Event foundEvent = DbManager.Instance.GetSingleEvent(eventId);

            if (foundEvent == null)
            {
                throw new NullReferenceException();
            }

            foundEvent.Participants.ForEach(p => p.ScheduledEvents = new List<Event>());
            return foundEvent;
        }

        public List<Event> GetAllEvents()
        {
            List<Event> events = DbManager.Instance.GetAllEvents();
            events.ForEach(e => e.Participants.ForEach(p => p.ScheduledEvents = new List<Event>()));
            return events;
        }
    }
}
