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
    class ModifyEventViewModel
    {
        public ICommand ModifyEventCommand { get; set; }
        public ICommand AddPraticipantCommand { get; set; }
        public ICommand RemovePraticipantCommand { get; set; }
        public Window CurrentWindow { get; set; }
        public Event EventToModify { get; set; }
        public ObservableCollection<Event> EventList { get; set; }
        public Person PersonToParticipate { get; set; }
        public Person PersonToRevokeParticipation { get; set; }
        public ObservableCollection<Person> AvailablePeople { get; set; }
        public ObservableCollection<Person> Participants { get; set; }
        private bool _participantsInitialized = false;

        public ModifyEventViewModel(Event eventToModify, ObservableCollection<Event> eventList)
        {
            ModifyEventCommand = new RelayCommand(ModifyEventExecute, ModifyEventCanExecute);
            AddPraticipantCommand = new RelayCommand(AddPraticipantExecute, AddPraticipantCanExecute);
            RemovePraticipantCommand = new RelayCommand(RemovePraticipantExecute, RemovePraticipantCanExecute);
            EventList = eventList;

            EventToModify = eventToModify;
            PersonToParticipate = new Person();
            PersonToRevokeParticipation = new Person();
            AvailablePeople = new ObservableCollection<Person>();
            Participants = new ObservableCollection<Person>();
        }

        private void ModifyEventExecute(object parameter)
        {
            Object[] parameters = parameter as Object[];

            foreach (Person p in Participants)
            {
                if (!EventToModify.Participants.Contains(p, new PersonComparer()))
                {
                    EventToModify.AddParticipant(p);
                }
            }

            List<Person> toBeRemoved = new List<Person>();
            foreach (Person p in EventToModify.Participants)
            {
                if (!Participants.Contains(p, new PersonComparer()))
                {
                    toBeRemoved.Add(p);
                }
            }

            toBeRemoved.ForEach(p => EventToModify.Participants.Remove(p));

            toBeRemoved = new List<Person>(EventToModify.Participants); //TEST
            toBeRemoved.ForEach(p => EventToModify.Participants.Remove(p));

            if (EventProxy.Instance.EventServices.EditEvent(EventToModify))
            {
                EventToModify = EventProxy.Instance.EventServices.GetSingleEvent(EventToModify.EventId);

                Event eventInList = EventList.First(p => p.EventId.Equals(EventToModify.EventId));
                eventInList.EventTitle = EventToModify.EventTitle;
                eventInList.Description = EventToModify.Description;
                eventInList.LastEditTimeStamp = EventToModify.LastEditTimeStamp;
                eventInList.ScheduledDateTimeBeging = EventToModify.ScheduledDateTimeBeging;
                eventInList.ScheduledDateTimeEnd = EventToModify.ScheduledDateTimeEnd;

                foreach (Person p in EventToModify.Participants)
                {
                    if(!eventInList.Participants.Contains(p, new PersonComparer()))
                    {
                        eventInList.AddParticipant(p);
                    }
                }

                foreach (Person p in eventInList.Participants)
                {
                    if (!EventToModify.Participants.Contains(p, new PersonComparer()))
                    {
                        eventInList.RemoveParticipant(p);
                    }
                }

                MessageBox.Show("Event successfully modified.");
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
        private bool ModifyEventCanExecute(object parameter)
        {
            if (parameter == null || !(parameter is Object[] parameters) || EventToModify.ScheduledDateTimeBeging == null
                                                                         || EventToModify.ScheduledDateTimeEnd == null
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
            AvailablePeople.Remove(PersonToParticipate);
        }
        private bool AddPraticipantCanExecute(object parameter)
        {
            Object[] arguments = parameter as Object[];

            if (parameter == null || !(parameter is Object[] parameters) || !(parameters[0] is StatusBar))
            {
                return false;
            }
            else if (EventToModify.ScheduledDateTimeBeging == null || EventToModify.ScheduledDateTimeEnd == null)
            {
                ((StatusBar)arguments[0]).Visibility = Visibility.Collapsed;
                return false;
            }

            foreach (Person p in PersonProxy.Instance.PersonServices.GetAllPeople())
            {
                if (_participantsInitialized && p.IsAvailableForEvent(EventToModify.ScheduledDateTimeBeging, EventToModify.ScheduledDateTimeEnd) && !AvailablePeople.Contains(p, new PersonComparer())
                                                                                                                     && !Participants.Contains(p, new PersonComparer()))
                {
                    AvailablePeople.Add(p);
                }
            }
            ((StatusBar)arguments[0]).Visibility = Visibility.Visible;

            if (((ListView)arguments[1]).SelectedItem == null)
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
            Participants.Remove(PersonToRevokeParticipation);
        }
        private bool RemovePraticipantCanExecute(object parameter)
        {
            Object[] arguments = parameter as Object[];

            if (parameter == null || !(parameter is Object[] parameters) || !(parameters[0] is StatusBar))
            {
                return false;
            }
            else if (EventToModify.ScheduledDateTimeBeging == null || EventToModify.ScheduledDateTimeEnd == null)
            {
                ((StatusBar)arguments[0]).Visibility = Visibility.Collapsed;
                return false;
            }

            foreach (Person p in EventToModify.Participants)
            {
                if (p.IsAvailableForEvent(EventToModify.ScheduledDateTimeBeging, EventToModify.ScheduledDateTimeEnd) && !AvailablePeople.Contains(p, new PersonComparer())
                                                                                                                     && !Participants.Contains(p, new PersonComparer()))
                {
                    AvailablePeople.Add(p);
                }
            }
            _participantsInitialized = true;
            ((StatusBar)arguments[0]).Visibility = Visibility.Visible;


            if (((ListView)parameters[1]).SelectedItem == null)
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
