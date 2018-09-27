using Client.Commands;
using Client.Proxies;
using Client.ViewModels.AccountViewModels;
using Client.ViewModels.EventViewModels;
using Client.ViewModels.PersonViewModels;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Client.ViewModels
{
    public class HomeViewModel
    {
        #region ICommands
        public ICommand AddPersonCommand { get; set; }
        public ICommand ModifyPersonCommand { get; set; }
        public ICommand DeletePersonCommand { get; set; }
        public ICommand PersonDetailsCommand { get; set; }
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

        public HomeViewModel(Account person)
        {
            AddPersonCommand = new RelayCommand(AddPersonExecute, AddPersonCanExecute);
            ModifyPersonCommand = new RelayCommand(ModifyPersonExecute, ModifyPersonCanExecute);
            DeletePersonCommand = new RelayCommand(DeletePersonExecute, DeletePersonCanExecute);
            PersonDetailsCommand = new RelayCommand(PersonDetailsExecute, PersonDetailsCanExecute);
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

            LoggedInAccount = person;

            PeopleList = new ObservableCollection<Person>(PersonProxy.Instance.PersonServices.GetAllPeople());
            EventsList = new ObservableCollection<Event>(EventProxy.Instance.EventServices.GetAllEvents());
            AccountsList = new ObservableCollection<Account>(AccountProxy.Instance.AccountServices.GetAllAccounts());
        }

        #region PersonCommandExecutions
        private void AddPersonExecute(object parameter)
        {
            Window window = new Window()
            {
                Width = 600,
                Height = 600,
                Content = new AddPersonViewModel(PeopleList),
            };

            window.ShowDialog();
        }
        private bool AddPersonCanExecute(object paramter)
        {
            return true;
        }

        private void ModifyPersonExecute(object parameter)
        {
            Window window = new Window()
            {
                Width = 500,
                Height = 600,
                Content = new ModifyPersonViewModel(SelectedPerson, PeopleList),
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
                Content = new DeletePersonConfirmationViewModel(SelectedPerson, PeopleList),
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
                Width = 500,
                Height = 600,
                Content = new PersonDetailsViewModel(SelectedPerson),
            };

            window.ShowDialog();
        }
        private bool PersonDetailsCanExecute(object parameter)
        {
            return SelectedPerson != null;
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
                Content = new AddEventViewModel(EventsList),
            };

            window.ShowDialog();
        }
        private bool AddEventCanExecute(object parameter)
        {
            return true;
        }

        private void ModifyEventExecute(object parameter)
        {
            Window window = new Window()
            {
                Width = 500,
                Height = 600,
                Content = new ModifyEventViewModel(SelectedEvent),
            };

            window.ShowDialog();

            EventsList = new ObservableCollection<Event>(EventProxy.Instance.EventServices.GetAllEvents());
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
                Width = 500,
                Height = 600,
                Content = new DeleteEventConfirmationViewModel(SelectedEvent),
            };

            window.ShowDialog();

            EventsList = new ObservableCollection<Event>(EventProxy.Instance.EventServices.GetAllEvents());
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
                Width = 500,
                Height = 600,
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
                Content = new CreateNewAccountViewModel(AccountsList),
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
                Content = new ModifyAccountViewModel(SelectedAccount, AccountsList),
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
                Content = new ModifyAccountViewModel(LoggedInAccount, AccountsList),
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
                Content = new DeleteAccountConfirmationViewModel(SelectedAccount, AccountsList),
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
            CurrentUserControl.Content = new LoginViewModel();
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
    }
}
