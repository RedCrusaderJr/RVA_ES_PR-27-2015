using Common.IModels;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.IAccess
{
    public interface IEventDbManager
    {
        Boolean AddEvent(Event eventToAdd);
        Boolean ModifyEvent(Event eventToModify);
        Boolean DeleteEvent(Event eventToDelete);
        Event GetSingleEvent(Int32 eventId);
        List<Event> GetAllEvents();
    }
}
