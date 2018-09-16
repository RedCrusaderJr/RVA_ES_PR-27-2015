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

            window.Show();
        }

        private bool ModifyPersonCanExecute(object parameter)
        {
            return true;
        }

        private void LogOutExecute(object parameter)
        {
            /*
            Object[] parameters = parameter as Object[];

            UserControl uc = parameters[0] as UserControl;
            Window currentWindow = Window.GetWindow(uc);
         
            UserControl newWindow = UserControl.Ge

            currentWindow.Close();
            newWindow.Show();
            */    
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
