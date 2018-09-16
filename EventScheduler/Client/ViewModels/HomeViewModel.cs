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

namespace Client.ViewModels
{
    public class HomeViewModel
    {
        public ICommand AddPersonCommand { get; set; }
        public ICommand ModifyPersonCommand { get; set; }
        public ICommand LogOutCommand { get; set; }
        public Person LoggedInPerson { get; set; }


        public HomeViewModel(Person person)
        {
            AddPersonCommand = new RelayCommand(AddPersonExecute, AddPersonCanExecute);
            ModifyPersonCommand = new RelayCommand(ModifyPersonExecute, ModifyPersonCanExecute);
            LogOutCommand = new RelayCommand(LogOutExecute, LogOutCanExecute);
            LoggedInPerson = person;
        }

        private void AddPersonExecute(object parameter)
        {
            Window window = new Window()
            {
                Width = 600,
                Height = 600,
                Content = new AddPersonViewModel(),
            };

            window.Show();
        }

        private bool AddPersonCanExecute(object paramter)
        {
            return LoggedInPerson.Role == ERole.ADMIN;
        }

        private void ModifyPersonExecute(object parameter)
        {
            Window window = new Window()
            {
                Width = 500,
                Height = 600,
                Content = new ModifyPersonViewModel(LoggedInPerson),
            };

            window.ShowDialog();

            LoggedInPerson = Proxy.Instance.BasicOperations.GetSinglePeson(LoggedInPerson.Username);
            
            Object[] parameters = parameter as Object[];
            TextBlock tb = parameters[0] as TextBlock;
            tb.Text = $"Username: {LoggedInPerson.Username}     First name: {LoggedInPerson.FirstName}     Last name: {LoggedInPerson.LastName}";
        }

        private bool ModifyPersonCanExecute(object parameter)
        {
            if (parameter == null || !(parameter is Object[] parameters) || parameters.Length != 1)
            {
                return false;
            }

            return true;
        }

        private void LogOutExecute(object parameter)
        {
            Object[] parameters = parameter as Object[];

            UserControl uc = parameters[0] as UserControl;
            Window currentWindow = Window.GetWindow(uc);

            Window newWindow = new Window()
            {
                Width = 400,
                Height = 400,
                Content = new LoginViewModel(),
            };
            
            newWindow.Show();
            currentWindow.Close();
        }

        private bool LogOutCanExecute(object parameter)
        {
            if (parameter == null || !(parameter is Object[] parameters) || parameters.Length != 1)
            {
                return false;
            }

            return true;
        }
    }
}
