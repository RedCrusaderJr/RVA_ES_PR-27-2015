using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contracts
{
    [ServiceContract]
    public interface IEventServices
    {
        [OperationContract]
        Event ScheduleEvent(Event eventToSchedule, List<Person> participants);

        [OperationContract]
        Event CancleEvent(Event eventToCancle);

        [OperationContract]
        Event EditEvent(Event eventToEdit);

        [OperationContract]
        Event RescheduleEvent(Int32 eventId, DateTime newBegining, DateTime newEnd);

        [OperationContract]
        Event AddParticipants(Int32 eventId, List<Person> participants);

        [OperationContract]
        Event RemoveParticipants(Int32 eventId, List<Person> participants);

        [OperationContract]
        Event EditEventDescription(Int32 eventId, String newDescription);

        [OperationContract]
        Event GetSingleEvent(Int32 eventId);

        [OperationContract]
        List<Event> GetAllEvents();
    }
}
