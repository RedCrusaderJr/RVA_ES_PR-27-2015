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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        ~HomeViewModel()
        {
            _personProxy.Unsubscribe();
            _accountProxy.Unsubscribe();
            _eventProxy.Unsubscribe();
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
            List<Person> UpdatedListOfPeople = _personProxy.GetAllPeople();

            foreach(Person p in UpdatedListOfPeople)
            {
                if(!PeopleList.Contains(p, new PersonComparer()))
                {
                    PeopleList.Add(p);
                }
                else
                {
                    Person foundPerson = PeopleList.FirstOrDefault(pe => pe.JMBG.Equals(p.JMBG));
                    if(foundPerson.LastEditTimeStamp != p.LastEditTimeStamp)
                    {
                        PeopleList.Remove(foundPerson);
                        PeopleList.Add(p);
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
            Person duplicatedPerson = _personProxy.AddPerson((Person)SelectedPerson.Duplicate());
            if (duplicatedPerson != null)
            {
                MessageBox.Show("Successful duplication.");
                PeopleList.Add(duplicatedPerson);
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
                    if(foundEvent.LastEditTimeStamp != e.LastEditTimeStamp)
                    {
                        EventsList.Remove(foundEvent);
                        EventsList.Add(e);
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

        //CALLBACKS
        #region IPersonServicesCallback
        public void NotifyPersonAddition(Person addedPerson)
        {
            if(!PeopleList.Contains(addedPerson, new PersonComparer()))
            {
                PeopleList.Add(addedPerson);
            }
        }

        public void NotifyPersonRemoval(Person removedPerson)
        {
            if(PeopleList.Contains(removedPerson, new PersonComparer()))
            {
                Person foundPerson = PeopleList.FirstOrDefault(p => p.JMBG.Equals(removedPerson.JMBG));
                PeopleList.Remove(foundPerson);
            }
        }

        public void NotifyPersonModification(Person modifiedPerson)
        {
            if (PeopleList.Contains(modifiedPerson, new PersonComparer()))
            {
                Person foundPerson = PeopleList.FirstOrDefault(p => p.JMBG.Equals(modifiedPerson.JMBG));
                PeopleList.Remove(foundPerson);
                PeopleList.Add(modifiedPerson);
            }
        }
        #endregion

        #region IEventServicesCallback
        public void NotifyEventAddition(Event addedEvent)
        {
            if (!EventsList.Contains(addedEvent, new EventComparer()))
            {
                EventsList.Add(addedEvent);
            }
        }

        public void NotifyEventRemoval(Event removedEvent)
        {
            if (EventsList.Contains(removedEvent, new EventComparer()))
            {
                Event foundEvent = EventsList.FirstOrDefault(e => e.EventId.Equals(removedEvent.EventId));
                EventsList.Remove(foundEvent);
            }
        }

        public void NotifyEventModification(Event modifiedEvent)
        {
            if (EventsList.Contains(modifiedEvent, new EventComparer()))
            {
                Event foundEvent = EventsList.FirstOrDefault(e => e.EventId.Equals(modifiedEvent.EventId));
                EventsList.Remove(foundEvent);
                EventsList.Add(modifiedEvent);
            }
        }
        #endregion

        #region IAccountServicesCallback
        public void NotifyAccountAddition(Account addedAccount)
        {
            if (!AccountsList.Contains(addedAccount, new AccountComparer()))
            {
                AccountsList.Add(addedAccount);
            }
        }

        public void NotifyAccountRemoval(Account removedAccount)
        {
            if (AccountsList.Contains(removedAccount, new AccountComparer()))
            {
                Account foundAccount = AccountsList.FirstOrDefault(a => a.Username.Equals(removedAccount.Username));
                AccountsList.Remove(foundAccount);
            }
        }

        public void NotifyAccountModification(Account modifiedAccount)
        {
            if (AccountsList.Contains(modifiedAccount, new AccountComparer()))
            {
                Account foundAccount = AccountsList.FirstOrDefault(a => a.Username.Equals(modifiedAccount.Username));
                AccountsList.Remove(foundAccount);
                AccountsList.Add(modifiedAccount);
            }
        }
        #endregion
    }
}
