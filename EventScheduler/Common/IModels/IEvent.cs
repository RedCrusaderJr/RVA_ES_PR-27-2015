using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.IModels
{
    public interface IEvent
    {
        Int32 EventId { get; set; }
        DateTime CreatedTimeStamp { get; }
        DateTime LastEditTimeStamp { get; set; }
        DateTime? ScheduledDateTimeBeging { get; set; }
        DateTime? ScheduledDateTimeEnd { get; set; }
        String EventTitle { get; set; }
        String Description { get; set; }

        List<Person> Participants { get; }

        Boolean AddParticipant(Person participant);
        Boolean RemoveParticipant(Person participant);
    }
}
