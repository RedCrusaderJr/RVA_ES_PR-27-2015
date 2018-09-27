using Client.Commands;
using Client.Proxies;
using Common.Helpers;
using Common.Models;
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
    class AddEventViewModel
    {
        public ICommand AddEventCommand { get; set; }
        public ICommand AddPraticipantCommand { get; set; }
        public ICommand RemovePraticipantCommand { get; set; }
        public Window CurrentWindow { get; set; }
        public Event EventToAdd { get; set; }
        public ObservableCollection<Event> EventList { get; set; }
        public Person PersonToParticipate { get; set; }
        public Person PersonToRevokeParticipation { get; set; }
        public ObservableCollection<Person> AvailablePeople { get; set; }
        public ObservableCollection<Person> Participants { get; set; }

        public AddEventViewModel(ObservableCollection<Event> eventList)
        {
            AddEventCommand = new RelayCommand(AddEventExecute, AddEventCanExecute);
            AddPraticipantCommand = new RelayCommand(AddPraticipantExecute, AddPraticipantCanExecute);
            RemovePraticipantCommand = new RelayCommand(RemovePraticipantExecute, RemovePraticipantCanExecute);
            EventList = eventList;

            EventToAdd = new Event();
            PersonToParticipate = new Person();
            PersonToRevokeParticipation = new Person();
            AvailablePeople = new ObservableCollection<Person>();
            Participants = new ObservableCollection<Person>();
        }

        private void AddEventExecute(object parameter)
        {
            Object[] parameters = parameter as Object[];
            EventToAdd = new Event()
            {
                EventTitle = parameters[0] as String,
                Description = parameters[1] as String,
            };

            if (EventProxy.Instance.EventServices.ScheduleEvent(EventToAdd, Participants.ToList()))
            {
                EventList.Add(EventToAdd);

                MessageBox.Show("Person successfully added.");
                UserControl uc = parameters[2] as UserControl;
                CurrentWindow = Window.GetWindow(uc);
                CurrentWindow.Close();
            }
            else
            {
                MessageBox.Show("Error on server or username already exists.");
                UserControl uc = parameters[2] as UserControl;
                CurrentWindow = Window.GetWindow(uc);
                CurrentWindow.Close();

            }
        }
        private bool AddEventCanExecute(object parameter)
        {
            if (parameter == null || !(parameter is Object[] parameters)|| (String)parameters[0] == String.Empty
                                                                        || (String)parameters[1] == String.Empty
                                                                        || EventToAdd.ScheduledDateTimeBeging == null
                                                                        || EventToAdd.ScheduledDateTimeEnd == null
                                                                        || !(parameters[2] is UserControl))
            {
                return false;
            }

            return true;
        }

        private void AddPraticipantExecute(object parameter)
        {
            Participants.Add(PersonToParticipate);
        }
        private bool AddPraticipantCanExecute(object parameter)
        {
            Object[] arguments = parameter as Object[];

            if (parameter == null || !(parameter is Object[] parameters) || !(parameters[0] is StatusBar))
            {
                return false;
            }
            else if(EventToAdd.ScheduledDateTimeBeging == null || EventToAdd.ScheduledDateTimeEnd == null)
            {
                ((StatusBar)arguments[0]).Visibility = Visibility.Collapsed;
                return false;
            }

            foreach (Person p in PersonProxy.Instance.PersonServices.GetAllPeople())
            {
                if (!AvailablePeople.Contains(p, new PersonComparer()))
                {
                    AvailablePeople.Add(p);
                }
            }
            ((StatusBar)arguments[0]).Visibility = Visibility.Visible;

            ListView AvailablePeopleList = (ListView)arguments[1];

            if (PersonToParticipate.JMBG == null)
            {
                return false;
            }

            return true;
        }

        private void RemovePraticipantExecute(object parameter)
        {
            Participants.Remove(PersonToRevokeParticipation);
        }
        private bool RemovePraticipantCanExecute(object parameter)
        {
            Object[] arguments = parameter as Object[];

            if (parameter == null || !(parameter is Object[] parameters) || !(parameters[0] is StatusBar))
            {
                return false;
            }
            else if(EventToAdd.ScheduledDateTimeBeging == null || EventToAdd.ScheduledDateTimeEnd == null)
            {
                ((StatusBar)arguments[0]).Visibility = Visibility.Collapsed;
                return false;
            }

            ((StatusBar)arguments[0]).Visibility = Visibility.Visible;


            ListView PatricipantsList = (ListView)arguments[1];

            if (PersonToRevokeParticipation.JMBG == null)
            {
                return false;
            }

            return true;
        }
    }
}
