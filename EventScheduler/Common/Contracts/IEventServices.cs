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
        Boolean ScheduleEvent(Event eventToSchedule, List<Person> participants);

        [OperationContract]
        Boolean CancleEvent(Event eventToCancle);

        [OperationContract]
        Boolean EditEvent(Event eventToEdit);

        [OperationContract]
        Boolean RescheduleEvent(Int32 eventId, DateTime newBegining, DateTime newEnd);

        [OperationContract]
        Boolean AddParticipants(Int32 eventId, List<Person> participants);

        [OperationContract]
        Boolean RemoveParticipants(Int32 eventId, List<Person> participants);

        [OperationContract]
        Boolean EditEventDescription(Int32 eventId, String newDescription);

        [OperationContract]
        Event GetSingleEvent(Int32 eventId);

        [OperationContract]
        List<Event> GetAllEvents();
    }
}
