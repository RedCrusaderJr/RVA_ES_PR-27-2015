using Client.Commands;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Client.ViewModels.EventViewModels
{
    class EventDetailsViewModel
    {
        public ICommand CloseEventDetailsCommand { get; set; }
        public Event SelectedEvent { get; set; }

        public EventDetailsViewModel(Event selecteEvent)
        {
            SelectedEvent = selecteEvent;

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
            if (arg == null || !(arg is Object[] parameters) || parameters.Length != 1 || !(parameters[0] is UserControl))
            {
                return false;
            }

            return true;
        }
    }
}
