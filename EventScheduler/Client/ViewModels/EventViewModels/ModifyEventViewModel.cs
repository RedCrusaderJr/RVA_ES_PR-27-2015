using Client.Commands;
using Client.Proxies;
using Common;
using Common.Contracts;
using Common.Helpers;
using Common.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
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
        private static readonly ILog logger = Log4netHelper.GetLogger();

        public ICommand ModifyEventCommand { get; set; }
        public ICommand AddPraticipantCommand { get; set; }
        public ICommand RemovePraticipantCommand { get; set; }
        public Window CurrentWindow { get; set; }
        public Event EventToModify { get; set; }
        public ObservableCollection<Event> EventList { get; set; }
        public ObservableCollection<Person> PeopleList { get; set; }
        public Person PersonToParticipate { get; set; }
        public Person PersonToRevokeParticipation { get; set; }
        public ObservableCollection<Person> AvailablePeople { get; set; }
        public ObservableCollection<Person> Participants { get; set; }

        public IEventServices EventProxy { get; set; }
        public IPersonServices PersonProxy { get; set; }

        public ModifyEventViewModel(Event eventToModify, ObservableCollection<Event> eventList, ObservableCollection<Person> peopleList, IEventServices eventProxy, IPersonServices personProxy)
        {
            ModifyEventCommand = new RelayCommand(ModifyEventExecute, ModifyEventCanExecute);
            AddPraticipantCommand = new RelayCommand(AddPraticipantExecute, AddPraticipantCanExecute);
            RemovePraticipantCommand = new RelayCommand(RemovePraticipantExecute, RemovePraticipantCanExecute);
            EventList = eventList;
            PeopleList = peopleList;

            EventProxy = eventProxy;
            PersonProxy = personProxy;

            EventToModify = eventToModify;
            PersonToParticipate = new Person();
            PersonToRevokeParticipation = new Person();
            AvailablePeople = new ObservableCollection<Person>(PersonProxy.GetAllPeople().Where(p => p.IsAvailableForEvent(EventToModify.ScheduledDateTimeBeging, EventToModify.ScheduledDateTimeEnd)));
            Participants = new ObservableCollection<Person>(EventToModify.Participants);
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

            List<Person> peopleToBeRemoved = new List<Person>();
            foreach (Person p in EventToModify.Participants)
            {
                if (!Participants.Contains(p, new PersonComparer()))
                {
                    peopleToBeRemoved.Add(p);
                }
            }
            peopleToBeRemoved.ForEach(p => EventToModify.Participants.Remove(p));

            Event editedEvent = EventProxy.EditEvent(EventToModify);
            if (editedEvent != null)
            {
                logger.Info($"Event successfully modified.");
                LoggerHelper.Instance.LogMessage($"Event successfully modified.", EEventPriority.INFO, EStringBuilder.CLIENT);
                UserControl uc = parameters[0] as UserControl;
                CurrentWindow = Window.GetWindow(uc);
                CurrentWindow.Close();
            }
            else
            {
                logger.Error($"Error on server.");
                LoggerHelper.Instance.LogMessage($"Error on server.", EEventPriority.ERROR, EStringBuilder.CLIENT);

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
                                                                         || Participants.Count == 0
                                                                         || (DateTime.Compare((DateTime)EventToModify.ScheduledDateTimeBeging, (DateTime)EventToModify.ScheduledDateTimeEnd) > 0))
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
            else if (EventToModify.ScheduledDateTimeBeging == null || EventToModify.ScheduledDateTimeEnd == null)
            {
                ((StatusBar)arguments[0]).Visibility = Visibility.Collapsed;
                return false;
            }

            foreach (Person p in PersonProxy.GetAllPeople().Where(per => per.IsAvailableForEvent(EventToModify.ScheduledDateTimeBeging, EventToModify.ScheduledDateTimeEnd)))
            {
                if (!AvailablePeople.Contains(p, new PersonComparer()) && !Participants.Contains(p, new PersonComparer()))
                {
                    AvailablePeople.Add(p);
                }
            }
            foreach(Person p in EventToModify.Participants)
            {
                if(p.IsAvailableForEventExcludingOneSpecificEvent(EventToModify.ScheduledDateTimeBeging, EventToModify.ScheduledDateTimeEnd, EventToModify)
                                                                                                      && !AvailablePeople.Contains(p, new PersonComparer())
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
            else if (EventToModify.ScheduledDateTimeBeging == null || EventToModify.ScheduledDateTimeEnd == null)
            {
                ((StatusBar)arguments[0]).Visibility = Visibility.Collapsed;
                return false;
            }

            List<Person> peopleToRemove = new List<Person>();
            foreach (Person p in Participants)
            {
                if (!p.IsAvailableForEvent(EventToModify.ScheduledDateTimeBeging, EventToModify.ScheduledDateTimeEnd) && !EventToModify.Participants.Contains(p, new PersonComparer()))
                {
                    peopleToRemove.Add(p);
                }
            }
            peopleToRemove.ForEach(p => Participants.Remove(p));

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
