using Common.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public class EventComparer : IEqualityComparer<Event>
    {
        //STA CE BITI SA KOPIJAMA OBJEKATA
        public bool Equals(Event x, Event y)
        {
            return x.EventId == y.EventId;
        }

        public int GetHashCode(Event obj)
        {
            return obj.EventId;
        }
    }
}
