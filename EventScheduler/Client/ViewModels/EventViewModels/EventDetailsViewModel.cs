using Client.Commands;
using Client.Proxies;
using Common.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Client.ViewModels.EventViewModels
{
    class EventDetailsViewModel
    {
        private static readonly ILog logger = Log4netHelper.GetLogger();

        public ICommand CloseEventDetailsCommand { get; set; }
        public Event SelectedEvent { get; set; }
        public ObservableCollection<Person> Participants { get; set; }

        public EventDetailsViewModel(Event selecteEvent)
        {
            SelectedEvent = selecteEvent;
            Participants = new ObservableCollection<Person>(SelectedEvent.Participants);
            CloseEventDetailsCommand = new RelayCommand(CloseEventDetailsCommandExecute, CloseEventDetailsCommandCanExecute);
        }

        private void CloseEventDetailsCommandExecute(object obj)
        {
            object[] parameters = obj as object[];
            Window currentWindow = Window.GetWindow((UserControl)parameters[0]);
            currentWindow.Close();
        }
        private bool CloseEventDetailsCommandCanExecute(object arg)
        {
            if (arg == null || !(arg is Object[] parameters) || parameters.Length != 2 
                                                             || !(parameters[0] is UserControl)
                                                             || !(parameters[1] is StatusBar))
            {
                return false;
            }


            if(Participants.Count==0)
            {
                ((StatusBar)parameters[1]).Visibility = Visibility.Collapsed;
            }

            return true;
        }
    }
}
