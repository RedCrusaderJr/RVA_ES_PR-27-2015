using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModels.EventViewModels
{
    class EventDetailsViewModel
    {
        public Event SelectedEvent { get; set; }

        public EventDetailsViewModel(Event selecteEvent)
        {
            SelectedEvent = selecteEvent;
        }
    }
}
