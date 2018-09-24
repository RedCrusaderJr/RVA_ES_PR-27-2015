using Common.Contracts;
using Common.Helpers;
using Common.Models;
using Server.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Providers
{
    class EventServices_Provider : IEventServices
    {
        public bool ScheduleEvent(Event eventToSchedule, List<Person> participants)
        {
            List<Person> participantsToAdd = participants.Where(p => !eventToSchedule.Participants.Contains(p, new PersonComparer())).ToList();
            participantsToAdd.ForEach(p => eventToSchedule.AddParticipant(p));

            if(DbManager.Instance.AddEvent(eventToSchedule))
            {
                //TODO: NOTIFY ALL PERSONS THAT NEED TO BE NOTIFIED....
                return true;
            }

            return false;
        }

        public bool CancleEvent(Event eventToCancle)
        {
            if(DbManager.Instance.DeleteEvent(eventToCancle))
            {
                //TODO: NOTIFY ALL PERSONS THAT NEED TO BE NOTIFIED....
                return true;
            }

            return false;
        }

        public bool EditEvent(Event eventToEdit)
        {
            if(DbManager.Instance.ModifyEvent(eventToEdit))
            {
                //TODO: NOTIFY...
                return true;
            }

            return false;
        }

        public bool RescheduleEvent(Int32 eventId, DateTime newBegining, DateTime newEnd)
        {
            Event foundEvent = DbManager.Instance.GetSingleEvent(eventId);
            if(foundEvent == null)
            {
                return false;
            }

            foundEvent.ScheduledDateTimeBeging = newBegining;
            foundEvent.ScheduledDateTimeEnd = newEnd;
            foundEvent.LastEditTimeStamp = DateTime.Now;

            if(DbManager.Instance.ModifyEvent(foundEvent))
            {
                //TODO: NOTIFY
                return true;
            }

            return false;
        }

        public bool AddParticipants(Int32 eventId, List<Person> participants)
        {
            Event foundEvent = DbManager.Instance.GetSingleEvent(eventId);
            if (foundEvent == null)
            {
                return false;
            }

            List<Person> participantsToAdd = participants.Where(p => !foundEvent.Participants.Contains(p, new PersonComparer())).ToList();
            participantsToAdd.ForEach(p => foundEvent.AddParticipant(p));
            
            if (DbManager.Instance.ModifyEvent(foundEvent))
            {
                //TODO: NOTIFY
                return true;
            }

            return false;
        }

        public bool RemoveParticipants(Int32 eventId, List<Person> participants)
        {
            Event foundEvent = DbManager.Instance.GetSingleEvent(eventId);
            if (foundEvent == null)
            {
                return false;
            }

            List<Person> participantsToRemove = participants.Where(p => foundEvent.Participants.Contains(p, new PersonComparer())).ToList();
            participantsToRemove.ForEach(p => foundEvent.RemoveParticipant(p));

            if (DbManager.Instance.ModifyEvent(foundEvent))
            {
                //TODO: NOTIFY
                return true;
            }

            return false;
        }

        public bool EditEventDescription(Int32 eventId, string newDescription)
        {
            Event foundEvent = DbManager.Instance.GetSingleEvent(eventId);
            if (foundEvent == null)
            {
                return false;
            }

            foundEvent.Description = newDescription;

            if (DbManager.Instance.ModifyEvent(foundEvent))
            {
                return true;
            }

            return false;
        }

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
    }
}
