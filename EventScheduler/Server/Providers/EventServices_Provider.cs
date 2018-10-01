using Common.Contracts;
using Common.Helpers;
using Common.Models;
using log4net;
using Server.Access;
using Server.Providers.ObserverPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server.Providers
{
    //TODO: FASADA
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class EventServices_Provider : IEventServices 
    {
        private static readonly ILog logger = Log4netHelper.GetLogger();

        public Event ScheduleEvent(Event eventToSchedule, List<Person> participants)
        {
            List<Person> participantsToAdd = participants.Where(p => !eventToSchedule.Participants.Contains(p, new PersonComparer())).ToList();
            participantsToAdd.ForEach(p => eventToSchedule.AddParticipant(p));

            Event scheduledEvent = DbManager.Instance.AddEvent(eventToSchedule);
            if (scheduledEvent != null)
            {
                IEventServicesCallback currentCallbackProxy = OperationContext.Current.GetCallbackChannel<IEventServicesCallback>();
                EventServiceCallback.Instance.NotifyAllEventAddition(scheduledEvent, currentCallbackProxy);

                scheduledEvent.Participants = new List<Person>();
                return scheduledEvent;
            }

            return null;
        }

        public Event CancleEvent(Event eventToCancle)
        {
            Event cancledEvent = DbManager.Instance.DeleteEvent(eventToCancle);
            if (cancledEvent != null)
            {
                IEventServicesCallback currentCallbackProxy = OperationContext.Current.GetCallbackChannel<IEventServicesCallback>();
                EventServiceCallback.Instance.NotifyAllEventRemoval(cancledEvent, currentCallbackProxy);

                cancledEvent.Participants = new List<Person>();
                return cancledEvent;
            }

            return null;
        }

        public Event EditEvent(Event eventToEdit)
         {
            Event editedEvent = DbManager.Instance.ModifyEvent(eventToEdit);
            if (editedEvent != null)
            {
                IEventServicesCallback currentCallbackProxy = OperationContext.Current.GetCallbackChannel<IEventServicesCallback>();
                EventServiceCallback.Instance.NotifyAllEventModification(editedEvent, currentCallbackProxy);

                editedEvent.Participants = new List<Person>();
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
                IEventServicesCallback currentCallbackProxy = OperationContext.Current.GetCallbackChannel<IEventServicesCallback>();
                EventServiceCallback.Instance.NotifyAllEventModification(rescheduledEvent, currentCallbackProxy);

                rescheduledEvent.Participants = new List<Person>();
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
                IEventServicesCallback currentCallbackProxy = OperationContext.Current.GetCallbackChannel<IEventServicesCallback>();
                EventServiceCallback.Instance.NotifyAllEventModification(modifiedEvent, currentCallbackProxy);

                modifiedEvent.Participants = new List<Person>();
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
                IEventServicesCallback currentCallbackProxy = OperationContext.Current.GetCallbackChannel<IEventServicesCallback>();
                EventServiceCallback.Instance.NotifyAllEventModification(modifiedEvent, currentCallbackProxy);

                modifiedEvent.Participants = new List<Person>();
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
                IEventServicesCallback currentCallbackProxy = OperationContext.Current.GetCallbackChannel<IEventServicesCallback>();
                EventServiceCallback.Instance.NotifyAllEventModification(modifiedEvent, currentCallbackProxy);

                modifiedEvent.Participants = new List<Person>();
                return modifiedEvent;
            }

            return null;
        }

        public Event GetSingleEvent(int eventId)
        { 
            Event foundEvent = DbManager.Instance.GetSingleEvent(eventId);

            if (foundEvent == null)
            {
                return null;
            }

            foundEvent.Participants.ForEach(p => p.ScheduledEvents.ForEach(e => e.Participants.ForEach(pa => pa.ScheduledEvents = new List<Event>())));
            return foundEvent;
        }

        public List<Event> GetAllEvents()
        {
            List<Event> events = DbManager.Instance.GetAllEvents();
            events.ForEach(e => e.Participants.ForEach(p => p.ScheduledEvents.ForEach(ev => ev.Participants.ForEach(pa => pa.ScheduledEvents = new List<Event>()))));
            return events;
        }

        public void Subscribe()
        {
            IEventServicesCallback currentCallbackProxy = OperationContext.Current.GetCallbackChannel<IEventServicesCallback>();
            EventServiceCallback.Instance.Subscribe(currentCallbackProxy);
        }

        public void Unsubscribe()
        {
            IEventServicesCallback currentCallbackProxy = OperationContext.Current.GetCallbackChannel<IEventServicesCallback>();
            EventServiceCallback.Instance.Unsubscribe(currentCallbackProxy);
        }
    }
}
