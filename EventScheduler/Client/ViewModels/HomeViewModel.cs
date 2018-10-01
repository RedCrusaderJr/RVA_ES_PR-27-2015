using Client.Commands;
using Client.Proxies;
using Client.ViewModels.AccountViewModels;
using Client.ViewModels.EventViewModels;
using Client.ViewModels.PersonViewModels;
using Common.Contracts;
using Common.Helpers;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Client.ViewModels
{
    [CallbackBehavior(UseSynchronizationContext = false)]
    public class HomeViewModel : IPersonServicesCallback, IEventServicesCallback, IAccountServicesCallback 
    {
        #region ICommands
        public ICommand AddPersonCommand { get; set; }
        public ICommand ModifyPersonCommand { get; set; }
        public ICommand DeletePersonCommand { get; set; }
        public ICommand PersonDetailsCommand { get; set; }
        public ICommand DuplicatePersonCommand { get; set; }
        public ICommand UndoPersonCommand { get; set; }
        public ICommand RedoPersonCommand { get; set; }
        public ICommand ShowPeopleCommand { get; set; }

        public ICommand AddEventCommand { get; set; }
        public ICommand ModifyEventCommand { get; set; }
        public ICommand DeleteEventCommand { get; set; }
        public ICommand EventDetailsCommand { get; set; }
        public ICommand ShowEventsCommand { get; set; }

        public ICommand CreateAccountCommand { get; set; }
        public ICommand ModifyAccountCommand { get; set; }
        public ICommand ModifyPersonalAccountCommand { get; set; }
        public ICommand DeleteAccountCommand { get; set; }
        public ICommand ShowAccountsCommand { get; set; }

        public ICommand LogOutCommand { get; set; }
        #endregion

        public Account LoggedInAccount { get; set; }
        public Person SelectedPerson { get; set; }
        public Event SelectedEvent { get; set; }
        public Account SelectedAccount { get; set; }
        public ObservableCollection<Person> PeopleList { get; set; }
        public ObservableCollection<Event> EventsList { get; set; }
        public ObservableCollection<Account> AccountsList { get; set; }

        //public static object SyncLockPerson { get; set; } = new object();
        //public static object SyncLockAccount { get; set; } = new object();
        //public static object SyncLockEvent { get; set; } = new object();

        IPersonServices _personProxy;
        IAccountServices _accountProxy;
        IEventServices _eventProxy;

        public UserControl MessageUserControl { get; set; }
        public TextBlock InfoBlock { get; set; }

        public HomeViewModel(Account person, UserControl messageUserControl)
        {
            MessageUserControl = messageUserControl;
            InfoBlock = MessageUserControl.FindName("InfoBlock") as TextBlock;

            AddPersonCommand = new RelayCommand(AddPersonExecute, AddPersonCanExecute);
            ModifyPersonCommand = new RelayCommand(ModifyPersonExecute, ModifyPersonCanExecute);
            DeletePersonCommand = new RelayCommand(DeletePersonExecute, DeletePersonCanExecute);
            PersonDetailsCommand = new RelayCommand(PersonDetailsExecute, PersonDetailsCanExecute);
            DuplicatePersonCommand = new RelayCommand(DuplicatePersonExecute, DuplicatePersonCanExecute);
            UndoPersonCommand = new RelayCommand(UndoPersonExecute, UndoPersonCanExecute);
            RedoPersonCommand = new RelayCommand(RedoPersonExecute, RedoPersonCanExecute);
            ShowPeopleCommand = new RelayCommand(ShowPeopleExecute, ShowPeopleCanExecute);

            AddEventCommand = new RelayCommand(AddEventExecute, AddEventCanExecute);
            ModifyEventCommand = new RelayCommand(ModifyEventExecute, ModifyEventCanExecute);
            DeleteEventCommand = new RelayCommand(DeleteEventExecute, DeleteEventCanExecute);
            EventDetailsCommand = new RelayCommand(EventDetailsExecute, EventDetailsCanExecute);
            ShowEventsCommand = new RelayCommand(ShowEventsExecute, ShowEventsCanExecute);

            CreateAccountCommand = new RelayCommand(CreateAccountExecute, CreateAccountCanExecute);
            ModifyAccountCommand = new RelayCommand(ModifyAccountExecute, ModifyAccountCanExecute);
            ModifyPersonalAccountCommand = new RelayCommand(ModifyPersonalAccountExecute, ModifyPersonalAccountCanExecute);
            DeleteAccountCommand = new RelayCommand(DeleteAccountExecute, DeleteAccountCanExecute);
            ShowAccountsCommand = new RelayCommand(ShowAccountsExecute, ShowAccountsCanExecute);

            LogOutCommand = new RelayCommand(LogOutExecute, LogOutCanExecute);

            InstanceContext instanceContext = new InstanceContext(this);
            _personProxy = PersonProxy.ConnectToPersonService(instanceContext);
            _accountProxy = AccountProxy.ConnectToAccountService(instanceContext);
            _eventProxy = EventProxy.ConnectToAccountService(instanceContext);

            _personProxy.Subscribe();
            _accountProxy.Subscribe();
            _eventProxy.Subscribe();

            LoggedInAccount = person;

            PeopleList = new ObservableCollection<Person>(_personProxy.GetAllPeople());
            AccountsList = new ObservableCollection<Account>(_accountProxy.GetAllAccounts());
            EventsList = new ObservableCollection<Event>(_eventProxy.GetAllEvents());
        }

        #region PersonCommandExecutions
        private void AddPersonExecute(object parameter)
        {
            Window window = new Window()
            {
                Width = 600,
                Height = 600,
                Content = new AddPersonViewModel(PeopleList, _personProxy),
            };

            window.ShowDialog();
        }
        private bool AddPersonCanExecute(object paramter)
        {
            //UpdatePeopleList();
            PeopleUpdateTask();
            return true;
        }

        private void ModifyPersonExecute(object parameter)
        {
            Window window = new Window()
            {
                Width = 500,
                Height = 600,
                Content = new ModifyPersonViewModel(SelectedPerson, PeopleList, _personProxy),
            };

            window.ShowDialog();
        }
        private bool ModifyPersonCanExecute(object parameter)
        {
            if (SelectedPerson == null)
            {
                return false;
            }

            return true;
        }

        private void DeletePersonExecute(object parameter)
        {
            Window window = new Window()
            {
                Width = 500,
                Height = 600,
                Content = new DeletePersonConfirmationViewModel(SelectedPerson, PeopleList, _personProxy),
            };

            window.ShowDialog();
        }
        private bool DeletePersonCanExecute(object parameter)
        {
            return SelectedPerson != null;
        }

        private void PersonDetailsExecute(object parameter)
        {
            Window window = new Window()
            {
                Width = 800,
                Height = 600,
                Content = new PersonDetailsViewModel(SelectedPerson),
            };

            window.ShowDialog();
        }
        private bool PersonDetailsCanExecute(object parameter)
        {
            return SelectedPerson != null;
        }

        private void DuplicatePersonExecute(object parameter)
        {
            Person duplicatedPerson = _personProxy.DuplicatePerson(SelectedPerson);
            if (duplicatedPerson != null)
            {
                MessageBox.Show("Successful duplication.");
            }
            else
            {
                MessageBox.Show("Unsuccessful duplication.");
            }
        }
        private bool DuplicatePersonCanExecute(object parameter)
        {
            return SelectedPerson != null;
        }

        private void UndoPersonExecute(object parameter)
        {
            throw new NotImplementedException();
        }
        private bool UndoPersonCanExecute(object parameter)
        {
            return false;
        }

        private void RedoPersonExecute(object parameter)
        {
            throw new NotImplementedException();
        }
        private bool RedoPersonCanExecute(object parameter)
        {
            return false;
        }

        private void ShowPeopleExecute(object parameter)
        {
            Object[] parameters = parameter as Object[];
            DataGrid peopleTable = parameters[0] as DataGrid;
            DataGrid eventTable = parameters[1] as DataGrid;
            DataGrid accountTable = parameters[2] as DataGrid;

            peopleTable.Visibility = Visibility.Visible;
            eventTable.Visibility = Visibility.Collapsed;
            accountTable.Visibility = Visibility.Collapsed;
        }
        private bool ShowPeopleCanExecute(object parameter)
        {
            if (parameter == null || !(parameter is Object[] parameters) || parameters.Length != 3
                                                                         || !(parameters[0] is DataGrid peopleTable)
                                                                         || !(parameters[1] is DataGrid eventsTable)
                                                                         || !(parameters[2] is DataGrid accountsTable)
                                                                         || peopleTable.Visibility == Visibility.Visible)
            {
                return false;
            }

            return true;
        }
        #endregion

        #region EventCommandExecutions
        private void AddEventExecute(object parameter)
        {
            Window window = new Window()
            {
                Width = 550,
                Height = 800,
                Content = new AddEventViewModel(EventsList, PeopleList, _eventProxy, _personProxy),
            };

            window.ShowDialog();
        }
        private bool AddEventCanExecute(object parameter)
        {
            //UpdateEventsList();
            EventsUpdateTask();
            return true;
        }

        private void ModifyEventExecute(object parameter)
        {
            Window window = new Window()
            {
                Width = 550,
                Height = 800,
                Content = new ModifyEventViewModel(SelectedEvent, EventsList, PeopleList, _eventProxy, _personProxy),
            };

            window.ShowDialog();
        }
        private bool ModifyEventCanExecute(object parameter)
        {
            if (SelectedEvent == null)
            {
                return false;
            }

            return true;
        }

        private void DeleteEventExecute(object parameter)
        {
            Window window = new Window()
            {
                Width = 550,
                Height = 700,
                Content = new DeleteEventConfirmationViewModel(SelectedEvent, EventsList, PeopleList, _eventProxy),
            };

            window.ShowDialog();
        }
        private bool DeleteEventCanExecute(object parameter)
        {
            if (SelectedEvent == null)
            {
                return false;
            }

            return true;
        }

        private void EventDetailsExecute(object parameter)
        {
            Window window = new Window()
            {
                Width = 550,
                Height = 700,
                Content = new EventDetailsViewModel(SelectedEvent),
            };

            window.ShowDialog();
        }
        private bool EventDetailsCanExecute(object parameter)
        {
            return SelectedEvent != null;
        }

        private void ShowEventsExecute(object parameter)
        {
            Object[] parameters = parameter as Object[];
            DataGrid peopleTable = parameters[0] as DataGrid;
            DataGrid eventTable = parameters[1] as DataGrid;
            DataGrid accountTable = parameters[2] as DataGrid;

            peopleTable.Visibility = Visibility.Collapsed;
            eventTable.Visibility = Visibility.Visible;
            accountTable.Visibility = Visibility.Collapsed;
        }
        private bool ShowEventsCanExecute(object parameter)
        {
            if (parameter == null || !(parameter is Object[] parameters) || parameters.Length != 3
                                                                         || !(parameters[0] is DataGrid peopleTable)
                                                                         || !(parameters[1] is DataGrid eventsTable)
                                                                         || !(parameters[2] is DataGrid accountsTable)
                                                                         || eventsTable.Visibility == Visibility.Visible)
            {
                return false;
            }

            return true;
        }
        #endregion

        #region AccountCommandExecutions
        private void CreateAccountExecute(object parameter)
        {
            Window window = new Window()
            {
                Width = 600,
                Height = 600,
                Content = new CreateNewAccountViewModel(AccountsList, _accountProxy),
            };

            window.ShowDialog();
            
        }
        private bool CreateAccountCanExecute(object parameter)
        {
            AccountsUpdateTask();
            return LoggedInAccount.Role == ERole.ADMIN;
        }

        private void ModifyAccountExecute(object parameter)
        {
            string modifiedAccountUsername = SelectedAccount.Username;

            Window window = new Window()
            {
                Width = 500,
                Height = 600,
                Content = new ModifyAccountViewModel(SelectedAccount, AccountsList, _accountProxy),
            };

            window.ShowDialog();

            if (LoggedInAccount.Username == modifiedAccountUsername)
            {
                LoggedInAccount = AccountsList.First(a => a.Username.Equals(LoggedInAccount.Username));

                Object[] parameters = parameter as Object[];
                TextBlock tb = parameters[0] as TextBlock;
                tb.Text = $"Username: {LoggedInAccount.Username}     First name: {LoggedInAccount.FirstName}     Last name: {LoggedInAccount.LastName}";
            }
        }
        private bool ModifyAccountCanExecute(object parameter)
        {
            if (parameter == null || !(parameter is Object[] parameters) || parameters.Length != 1 || SelectedAccount == null)
            {
                return false;
            }

            return true;
        }

        private void ModifyPersonalAccountExecute(object parameter)
        {
            Window window = new Window()
            {
                Width = 500,
                Height = 600,
                Content = new ModifyAccountViewModel(LoggedInAccount, AccountsList, _accountProxy),
            };

            window.ShowDialog();

            LoggedInAccount = AccountsList.FirstOrDefault(a => a.Username.Equals(LoggedInAccount.Username));
            Object[] parameters = parameter as Object[];
            TextBlock tb = parameters[0] as TextBlock;
            tb.Text = $"Username: {LoggedInAccount.Username}     First name: {LoggedInAccount.FirstName}     Last name: {LoggedInAccount.LastName}";         
        }
        private bool ModifyPersonalAccountCanExecute(object parameter)
        {
            return true;
        }

        private void DeleteAccountExecute(object parameter)
        {
            Window window = new Window()
            {
                Width = 500,
                Height = 600,
                Content = new DeleteAccountConfirmationViewModel(SelectedAccount, AccountsList, _accountProxy),
            };

            window.ShowDialog();
        }
        private bool DeleteAccountCanExecute(object parameter)
        {
            return SelectedAccount != null;
        }

        private void ShowAccountsExecute(object parameter)
        {
            Object[] parameters = parameter as Object[];
            DataGrid peopleTable = parameters[0] as DataGrid;
            DataGrid eventTable = parameters[1] as DataGrid;
            DataGrid accountTable = parameters[2] as DataGrid;

            peopleTable.Visibility = Visibility.Collapsed;
            eventTable.Visibility = Visibility.Collapsed;
            accountTable.Visibility = Visibility.Visible;
        }
        private bool ShowAccountsCanExecute(object parameter)
        {
            if (parameter == null || !(parameter is Object[] parameters) || parameters.Length != 3
                                                                         || !(parameters[0] is DataGrid peopleTable)
                                                                         || !(parameters[1] is DataGrid eventsTable)
                                                                         || !(parameters[2] is DataGrid accountsTable)
                                                                         || accountsTable.Visibility == Visibility.Visible)
            {
                return false;
            }

            return true;
        }

        private void LogOutExecute(object parameter)
        {
            Object[] parameters = parameter as Object[];

            _personProxy.Unsubscribe();
            _accountProxy.Unsubscribe();
            _eventProxy.Unsubscribe();

            UserControl CurrentUserControl = parameters[0] as UserControl;
            CurrentUserControl.Content = new LoginViewModel(MessageUserControl);
            CurrentUserControl.VerticalContentAlignment = VerticalAlignment.Top;
            CurrentUserControl.HorizontalContentAlignment = HorizontalAlignment.Left;
            CurrentUserControl.Width = 1500;
            CurrentUserControl.Height = 1000;
        }
        private bool LogOutCanExecute(object parameter)
        {
            if (parameter == null || !(parameter is Object[] parameters) || parameters.Length != 1)
            {
                return false;
            }

            return true;
        }
        #endregion

        private void UpdatePeopleList()
        {
            List<Person> UpdatedListOfPeople = _personProxy.GetAllPeople();

            foreach (Person p in UpdatedListOfPeople)
            {
                if (!PeopleList.Contains(p, new PersonComparer()))
                {
                    PeopleList.Add(p);
                }
                else
                {
                    Person foundPerson = PeopleList.FirstOrDefault(pe => pe.JMBG.Equals(p.JMBG));
                    if (DateTime.Compare(foundPerson.LastEditTimeStamp, p.LastEditTimeStamp) != 0 || foundPerson.ScheduledEvents.Count != p.ScheduledEvents.Count)
                    {
                        PeopleList.Remove(foundPerson);
                        PeopleList.Add(p);
                    }
                    else 
                    {
                        foreach(Event e in foundPerson.ScheduledEvents)
                        {
                            if(!p.ScheduledEvents.TrueForAll(ev => ev.EventId.Equals(e.EventId) 
                                                               && ev.Description.Equals(e.Description)
                                                               && ev.EventTitle.Equals(e.EventTitle)
                                                               && ev.ScheduledDateTimeBeging.Equals(e.ScheduledDateTimeBeging)
                                                               && ev.ScheduledDateTimeEnd.Equals(e.ScheduledDateTimeEnd)
                                                               && ev.LastEditTimeStamp.Equals(e.LastEditTimeStamp)))
                            {
                                PeopleList.Remove(foundPerson);
                                PeopleList.Add(p);
                            }
                        }
                    }
                }
            }

            List<Person> peopleToBeRemoved = new List<Person>();
            foreach (Person p in PeopleList)
            {
                if (!UpdatedListOfPeople.Contains(p, new PersonComparer()))
                {
                    peopleToBeRemoved.Add(p);
                }
            }
            peopleToBeRemoved.ForEach(p => PeopleList.Remove(p));
        }
        private void UpdateEventsList()
        {
            List<Event> UpdatedListOfEvents = _eventProxy.GetAllEvents();

            foreach (Event e in UpdatedListOfEvents)
            {
                if (!EventsList.Contains(e, new EventComparer()))
                {
                    EventsList.Add(e);
                }
                else
                {
                    Event foundEvent = EventsList.FirstOrDefault(ev => ev.EventId.Equals(e.EventId));
                    if (DateTime.Compare(foundEvent.LastEditTimeStamp, e.LastEditTimeStamp) != 0 || foundEvent.Participants.Count != e.Participants.Count)
                    {
                        EventsList.Remove(foundEvent);
                        EventsList.Add(e);
                    }
                    else
                    {
                        foreach (Person p in foundEvent.Participants)
                        {
                            if (!e.Participants.TrueForAll(pe => pe.JMBG.Equals(p.JMBG)
                                                                && pe.FirstName.Equals(p.FirstName)
                                                                && pe.LastName.Equals(p.LastName)
                                                                && pe.LastEditTimeStamp.Equals(p.LastEditTimeStamp)))

                            {
                                EventsList.Remove(foundEvent);
                                EventsList.Add(e);
                            }
                        }
                    }
                }
            }

            List<Event> eventsToBeRemoved = new List<Event>();
            foreach (Event e in EventsList)
            {
                if (!UpdatedListOfEvents.Contains(e, new EventComparer()))
                {
                    eventsToBeRemoved.Add(e);
                }
            }
            eventsToBeRemoved.ForEach(e => EventsList.Remove(e));
        }

        //CALLBACKS
        #region IPersonServicesCallback
        public void NotifyPersonAddition(Person addedPerson)
        {
            if(!PeopleList.Contains(addedPerson, new PersonComparer()))
            {
                UpdateHelper.Instance.PeopleListAdditionHelper.Add(addedPerson);
            }
        }

        public void NotifyPersonDuplicate(Person addedPerson)
        {
            if (!PeopleList.Contains(addedPerson, new PersonComparer()))
            {
                UpdateHelper.Instance.PeopleListAdditionHelper.Add(addedPerson);
            }
        }

        public void NotifyPersonRemoval(Person removedPerson)
        {
            if(PeopleList.Contains(removedPerson, new PersonComparer()))
            {
                UpdateHelper.Instance.PeopleListRemovalHelper.Add(removedPerson);
            }
        }

        public void NotifyPersonModification(Person modifiedPerson)
        {
            if (PeopleList.Contains(modifiedPerson, new PersonComparer()))
            {
                UpdateHelper.Instance.PeopleListModificationHelper.Add(modifiedPerson);
            }
        }
        #endregion

        #region IEventServicesCallback
        public void NotifyEventAddition(Event addedEvent)
        {
            if (!EventsList.Contains(addedEvent, new EventComparer()))
            {
                UpdateHelper.Instance.EventsListAdditionHelper.Add(addedEvent);
            }
        }

        public void NotifyEventRemoval(Event removedEvent)
        {
            if (EventsList.Contains(removedEvent, new EventComparer()))
            {
                UpdateHelper.Instance.EventsListRemovalHelper.Add(removedEvent);
            }
        }

        public void NotifyEventModification(Event modifiedEvent)
        {
            if (EventsList.Contains(modifiedEvent, new EventComparer()))
            {
                UpdateHelper.Instance.EventsListModificationHelper.Add(modifiedEvent);
            }
        }
        #endregion

        #region IAccountServicesCallback
        public void NotifyAccountAddition(Account addedAccount)
        {
            if (!AccountsList.Contains(addedAccount, new AccountComparer()))
            {
                UpdateHelper.Instance.AccountsListAdditionHelper.Add(addedAccount);
            }
        }

        public void NotifyAccountRemoval(Account removedAccount)
        {
            if (AccountsList.Contains(removedAccount, new AccountComparer()))
            {
                UpdateHelper.Instance.AccountsListRemovalHelper.Add(removedAccount);
            }
        }

        public void NotifyAccountModification(Account modifiedAccount)
        {
            if (AccountsList.Contains(modifiedAccount, new AccountComparer()))
            {
                UpdateHelper.Instance.AccountsListModificationHelper.Add(modifiedAccount);
            }
        }
        #endregion

        private void PeopleUpdateTask()
        {
            if (UpdateHelper.Instance.PeopleListAdditionHelper.Count != 0)
            {
                foreach (Person p in UpdateHelper.Instance.PeopleListAdditionHelper)
                {
                    if (!PeopleList.Contains(p, new PersonComparer()))
                    {
                        PeopleList.Add(p);

                        foreach (Event e in p.ScheduledEvents)
                        {
                            Event foundEvent = EventsList.First(ev => ev.EventId.Equals(e.EventId));
                            EventsList.Remove(foundEvent);
                            foundEvent.Participants.Add(p);
                            EventsList.Add(foundEvent); //OPREZ

                        }
                    }
                }
            }

            if (UpdateHelper.Instance.PeopleListRemovalHelper.Count != 0)
            {
                foreach (Person p in UpdateHelper.Instance.PeopleListRemovalHelper)
                {
                    if (PeopleList.Contains(p, new PersonComparer()))
                    {
                        Person foundPerson = PeopleList.FirstOrDefault(pe => pe.JMBG.Equals(p.JMBG));
                        PeopleList.Remove(foundPerson);

                        foreach(Event e in p.ScheduledEvents)
                        {
                            Event foundEvent = EventsList.First(ev => ev.EventId.Equals(e.EventId));
                            EventsList.Remove(foundEvent);
                            foundEvent.Participants = new List<Person>(foundEvent.Participants.Where(per => per.JMBG != p.JMBG));
                            EventsList.Add(foundEvent);
                            
                        }
                    }
                }
            }

            if (UpdateHelper.Instance.PeopleListModificationHelper.Count != 0)
            {
                foreach (Person p in UpdateHelper.Instance.PeopleListModificationHelper)
                {
                    if (PeopleList.Contains(p, new PersonComparer()))
                    {
                        Person foundPerson = PeopleList.FirstOrDefault(pe => pe.JMBG.Equals(p.JMBG));
                        PeopleList.Remove(foundPerson);
                        PeopleList.Add(p);

                        foreach (Event e in p.ScheduledEvents)
                        {
                            Event foundEvent = EventsList.First(ev => ev.EventId.Equals(e.EventId));
                            EventsList.Remove(foundEvent);
                            foundEvent.Participants = new List<Person>(foundEvent.Participants.Where(per => per.JMBG != p.JMBG));
                            foundEvent.Participants.Add(p);
                            EventsList.Add(foundEvent);

                        }
                    }
                }
            }

            if (++UpdateHelper.Instance.PersonCounter >= UpdateHelper.Instance.Limit - 1)
            {
                UpdateHelper.Instance.PeopleListAdditionHelper = new List<Person>();
                UpdateHelper.Instance.PeopleListRemovalHelper = new List<Person>();
                UpdateHelper.Instance.PeopleListModificationHelper = new List<Person>();

                UpdateHelper.Instance.PersonCounter = 0;
            }
        }

        private void EventsUpdateTask()
        {
            if (UpdateHelper.Instance.EventsListAdditionHelper.Count != 0)
            {
                foreach (Event e in UpdateHelper.Instance.EventsListAdditionHelper)
                {
                    if (!EventsList.Contains(e, new EventComparer()))
                    {
                        EventsList.Add(e);

                        foreach (Person p in e.Participants)
                        {
                            Person foundPerson = PeopleList.First(pe => pe.JMBG.Equals(p.JMBG));
                            PeopleList.Remove(foundPerson);
                            foundPerson.ScheduledEvents.Add(e);
                            PeopleList.Add(foundPerson); //OPREZ

                        }
                    }
                }
            }

            if (UpdateHelper.Instance.EventsListRemovalHelper.Count != 0)
            {
                foreach (Event e in UpdateHelper.Instance.EventsListRemovalHelper)
                {
                    if (EventsList.Contains(e, new EventComparer()))
                    {
                        Event foundEvent = EventsList.FirstOrDefault(ev => ev.EventId.Equals(e.EventId));
                        EventsList.Remove(foundEvent);

                        foreach (Person p in e.Participants)
                        {
                            Person foundPerson = PeopleList.First(pe => pe.JMBG.Equals(p.JMBG));
                            PeopleList.Remove(foundPerson);
                            foundPerson.ScheduledEvents = new List<Event>(foundPerson.ScheduledEvents.Where(ev => ev.EventId != e.EventId));
                            PeopleList.Add(foundPerson);
                        }
                    }
                }
            }

            if (UpdateHelper.Instance.EventsListModificationHelper.Count != 0)
            {
                foreach (Event e in UpdateHelper.Instance.EventsListModificationHelper)
                {
                    if (EventsList.Contains(e, new EventComparer()))
                    {
                        Event foundEvent = EventsList.FirstOrDefault(ev => ev.EventId.Equals(e.EventId));
                        EventsList.Remove(foundEvent);
                        EventsList.Add(e);

                        foreach (Person p in e.Participants)
                        {
                            Person foundPerson = PeopleList.First(pe => pe.JMBG.Equals(p.JMBG));
                            PeopleList.Remove(foundPerson);
                            foundPerson.ScheduledEvents = new List<Event>(foundPerson.ScheduledEvents.Where(ev => ev.EventId != e.EventId));
                            foundPerson.ScheduledEvents.Add(e);
                            PeopleList.Add(foundPerson);
                        }
                    }
                }
            }

            if (++UpdateHelper.Instance.EventCounter >= UpdateHelper.Instance.Limit - 1)
            {
                UpdateHelper.Instance.EventsListAdditionHelper = new List<Event>();
                UpdateHelper.Instance.EventsListRemovalHelper = new List<Event>();
                UpdateHelper.Instance.EventsListModificationHelper = new List<Event>();

                UpdateHelper.Instance.EventCounter = 0;
            }
        }

        private void AccountsUpdateTask()
        {
            if (UpdateHelper.Instance.AccountsListAdditionHelper.Count != 0)
            {
                foreach (Account a in UpdateHelper.Instance.AccountsListAdditionHelper)
                {
                    if (!AccountsList.Contains(a, new AccountComparer()))
                    {
                        AccountsList.Add(a);
                    }
                }
            }

            if (UpdateHelper.Instance.AccountsListModificationHelper.Count != 0)
            {
                foreach (Account a in UpdateHelper.Instance.AccountsListModificationHelper)
                {
                    if (AccountsList.Contains(a, new AccountComparer()))
                    {
                        Account foundAccount = AccountsList.FirstOrDefault(ac => ac.Username.Equals(a.Username));
                        AccountsList.Remove(foundAccount);
                    }
                }
            }

            if (UpdateHelper.Instance.AccountsListRemovalHelper.Count != 0)
            {
                foreach (Account a in UpdateHelper.Instance.AccountsListRemovalHelper)
                {
                    if (AccountsList.Contains(a, new AccountComparer()))
                    {
                        Account foundAccount = AccountsList.FirstOrDefault(ac => ac.Username.Equals(a.Username));
                        AccountsList.Remove(foundAccount);
                        AccountsList.Add(a);
                    }
                }
            }

            if (++UpdateHelper.Instance.AccountCounter >= UpdateHelper.Instance.Limit - 1)
            {
                UpdateHelper.Instance.AccountsListAdditionHelper = new List<Account>();
                UpdateHelper.Instance.AccountsListRemovalHelper = new List<Account>();
                UpdateHelper.Instance.AccountsListModificationHelper = new List<Account>();

                UpdateHelper.Instance.AccountCounter = 0;
            }
        }
    }
}
