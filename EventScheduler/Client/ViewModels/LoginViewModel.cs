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
    public class LoginViewModel
    {
        public ICommand LoginCommand { get; set; }
        public UserControl CurrentUserControl { get; set; }
        public Person LoggedInPerson { get; set; }
        
        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Execute, CanExecute);
            LoggedInPerson = new Person();
        }

        private void Execute(object parameter)
        {
            Object[] parameters = parameter as Object[];

            String username = parameters[0] as String;
            String password = parameters[1] as String;
            CurrentUserControl = parameters[2] as UserControl;

            try
            {
                Person person = Proxy.Instance.BasicOperations.Login(username, password);
                if(person == null)
                {
                    MessageBox.Show("Username or password is incorrect.");
                }
                else
                {
                    MessageBox.Show("Successfull login!");

                    Window newWindow = new Window()
                    {
                        Width = 1200,
                        Height = 600, 
                        Content = new HomeViewModel(person),
                    };
                    newWindow.Show();

                    Window currentWindow = Window.GetWindow(CurrentUserControl);
                    currentWindow.Close();
                }
            }
            catch(Exception e)
            {
                MessageBox.Show($"Problem with connection. Message: {e.Message}");
            }
        }

        private bool CanExecute(object parameter)
        {
            if (parameter == null || !(parameter is Object[] parameters) || (String)parameters[0] == "" || (String)parameters[1] == "")
            {
                return false;
            }

            return true;
        }
    }
}
