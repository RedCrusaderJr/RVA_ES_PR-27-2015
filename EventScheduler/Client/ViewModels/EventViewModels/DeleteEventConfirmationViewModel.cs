using Client.Commands;
using Client.Proxies;
using Common.Contracts;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Client.ViewModels.EventViewModels
{
    class DeleteEventConfirmationViewModel
    {
        public ICommand DeleteEventCommand { get; set; }
        public Event EventToBeDeleted { get; set; }
        public ObservableCollection<Event> EventList { get; set; }
        public ObservableCollection<Person> PeopleList { get; set; }
        public ObservableCollection<Person> Participants { get; set; }

        public IEventServices EventProxy { get; set; }

        public DeleteEventConfirmationViewModel(Event eventToBeDeleted, ObservableCollection<Event> eventList, ObservableCollection<Person> peopleList, IEventServices eventProxy)
        {
            EventToBeDeleted = eventToBeDeleted;
            EventList = eventList;
            PeopleList = peopleList;
            Participants = new ObservableCollection<Person>(EventToBeDeleted.Participants);
            DeleteEventCommand = new RelayCommand(DeleteEventExecute, DeleteEventCanExecute);

            EventProxy = eventProxy;
        }

        private void DeleteEventExecute(object obj)
        {
            Event deletedEvent = EventProxy.CancleEvent(EventToBeDeleted);
            if (deletedEvent != null)
            {
                object[] parameters = obj as object[];
                Window currentWindow = Window.GetWindow((UserControl)parameters[0]);
                currentWindow.Close();
            }
        }
        private bool DeleteEventCanExecute(object arg)
        {
            if (arg == null || !(arg is Object[] parameters) || parameters.Length != 1
                                                             || !(parameters[0] is UserControl))
            {
                return false;
            }

            return true;
        }
    }
}
