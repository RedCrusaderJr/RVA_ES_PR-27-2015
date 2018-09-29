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
        Event AddEvent(Event eventToAdd);
        Event ModifyEvent(Event eventToModify);
        Event DeleteEvent(Event eventToDelete);
        Event GetSingleEvent(Int32 eventId);
        List<Event> GetAllEvents();
    }
}
