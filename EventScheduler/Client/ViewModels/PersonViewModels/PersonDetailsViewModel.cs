using Client.Commands;
using Client.Proxies;
using Client.ViewModels.EventViewModels;
using Common;
using Common.Helpers;
using Common.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Client.ViewModels.PersonViewModels
{
    class PersonDetailsViewModel
    {
        private static readonly ILog logger = Log4netHelper.GetLogger();

        public ICommand ClosePersonDetailsCommand { get; set; }
        public ICommand EventDetailsCommand { get; set; }
        public Person SelectedPerson { get; set; }
        public Event SelectedEvent { get; set; }

        public PersonDetailsViewModel(Person selectedPerson)
        {
            SelectedPerson = selectedPerson;

            ClosePersonDetailsCommand = new RelayCommand(ClosePersonDetailsCommandExecute, ClosePersonDetailsCommandCanExecute);
            EventDetailsCommand = new RelayCommand(EventDetailsExecute, EventDetailsCanExecute);

            logger.Debug("PersonDetailsViewModel constructor success.");
            LoggerHelper.Instance.LogMessage($"PersonDetailsViewModel constructor success.", EEventPriority.DEBUG, EStringBuilder.CLIENT);
        }

        private void ClosePersonDetailsCommandExecute(object obj)
        {
            object[] parameters = obj as object[];
            Window currentWindow = Window.GetWindow((UserControl)parameters[0]);
            currentWindow.Close();
        }

        private bool ClosePersonDetailsCommandCanExecute(object arg)
        {
            if (arg == null || !(arg is Object[] parameters) || parameters.Length != 1 || !(parameters[0] is UserControl))
            {
                return false;
            }

            return true;
        }

        private void EventDetailsExecute(object obj)
        {
            Window window = new Window()
            {
                Width = 550,
                Height = 700,
                Content = new EventDetailsViewModel(SelectedEvent),
            };

            window.ShowDialog();
        }

        private bool EventDetailsCanExecute(object arg)
        {
            return SelectedEvent != null;
        }
    }
}
