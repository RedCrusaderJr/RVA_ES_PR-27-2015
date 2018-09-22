using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.IModels

{
    public interface IPerson
    {
        String FirstName { get; set; }
        String LastName { get; set; }
        List<Event> ScheduledEvents {get; }

        Boolean ScheduleParticipationInEvent(Event e);
        Boolean CancleParticipationInEvent(Event e);

        Boolean IsAvailableForEvent(DateTime begining, DateTime end);
    }
}
