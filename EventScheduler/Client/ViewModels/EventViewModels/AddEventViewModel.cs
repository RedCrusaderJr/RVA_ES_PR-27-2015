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

            //Participants = new ObservableCollection<Person>(); //TESTIRANJE
            if (EventProxy.Instance.EventServices.ScheduleEvent(EventToAdd, Participants.ToList()))
            {
                EventList.Add(EventToAdd);

                MessageBox.Show("Event successfully added.");
                UserControl uc = parameters[0] as UserControl;
                CurrentWindow = Window.GetWindow(uc);
                CurrentWindow.Close();
            }
            else
            {
                MessageBox.Show("Error on server.");
                UserControl uc = parameters[0] as UserControl;
                CurrentWindow = Window.GetWindow(uc);
                CurrentWindow.Close();

            }
        }
        private bool AddEventCanExecute(object parameter)
        {
            if (parameter == null || !(parameter is Object[] parameters) || EventToAdd.ScheduledDateTimeBeging == null
                                                                         || EventToAdd.ScheduledDateTimeEnd == null
                                                                         || !(parameters[0] is UserControl)
                                                                         || Participants.Count == 0)
            {
                return false;
            }

            return true;
        }
  
        private void AddPraticipantExecute(object parameter)
        {
            Participants.Add(PersonToParticipate);

            Person foundPerson = AvailablePeople.FirstOrDefault(p => p.JMBG.Equals(PersonToParticipate.JMBG));
            AvailablePeople.Remove(foundPerson);
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
                if (p.IsAvailableForEvent(EventToAdd.ScheduledDateTimeBeging, EventToAdd.ScheduledDateTimeEnd) && !AvailablePeople.Contains(p, new PersonComparer())
                                                                                                               && !Participants.Contains(p, new PersonComparer()))
                {
                    AvailablePeople.Add(p);
                }
            }
            ((StatusBar)arguments[0]).Visibility = Visibility.Visible;

            if(((ListView)arguments[1]).SelectedItem == null)
            {
                return false;
            }

            ListView AvailablePeopleList = (ListView)arguments[1];
            PersonToParticipate = AvailablePeopleList.SelectedItem as Person;

            if (PersonToParticipate.JMBG == null)
            {
                return false;
            }

            return true;
        }

        private void RemovePraticipantExecute(object parameter)
        {
            Person foundPerson = Participants.FirstOrDefault(p => p.JMBG.Equals(PersonToRevokeParticipation.JMBG));
            Participants.Remove(foundPerson);
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


            if(((ListView)parameters[1]).SelectedItem == null)
            {
                return false;
            }

            ListView PatricipantsList = (ListView)arguments[1];
            PersonToRevokeParticipation = PatricipantsList.SelectedItem as Person;

            if (PersonToRevokeParticipation.JMBG == null)
            {
                return false;
            }

            return true;
        }
    }
}
