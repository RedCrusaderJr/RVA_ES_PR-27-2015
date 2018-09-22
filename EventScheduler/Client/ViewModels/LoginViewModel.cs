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
using System.Windows.Media;

namespace Client.ViewModels
{
    public class LoginViewModel
    {
        public ICommand LoginCommand { get; set; }
        public UserControl CurrentUserControl { get; set; }
        public PersonWithAccount LoggedInPerson { get; set; }
        
        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Execute, CanExecute);
            LoggedInPerson = new PersonWithAccount();
        }

        private void Execute(object parameter)
        {
            Object[] parameters = parameter as Object[];

            String username = parameters[0] as String;
            String password = parameters[1] as String;
            CurrentUserControl = parameters[2] as UserControl;

            try
            {
                PersonWithAccount personWithAccount = AccountProxy.Instance.AccountServices.Login(username, password);
                if(personWithAccount == null)
                {
                    MessageBox.Show("Username or password is incorrect.");
                }
                else
                {
                    MessageBox.Show("Successfull login!");

                    //PROBAJ BILO STA DRUGO nekako preko resource-a
                    /*
                    var parent = VisualTreeHelper.GetParent(CurrentUserControl);
                    var grid = VisualTreeHelper.GetChild(parent, 1);
                    UserControl messageView = VisualTreeHelper.GetChild(grid, 4) as UserControl;
                    TextBlock tb = VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(messageView, 1), 3) as TextBlock;

                    tb.Text += "Successfull login!";

                    /*
                    CurrentUserControl.Content = new HomeViewModel(person);
                    CurrentUserControl.Width = 1200;
                    CurrentUserControl.Height = 600;
                    */

                    /*
                    UserControl HomeViewUserControl = new UserControl()
                    {
                        Width = 1200,
                        Height = 600,
                        Content = new HomeViewModel(person),
                    }; 
                    */

                    //CurrentUserControl.Content = new HomeViewModel(personWithAccount);
                    //CurrentUserControl.Width = 1200;
                    //CurrentUserControl.Height = 800;
                    
                    
                    Window newWindow = new Window()
                    {
                        Width = 1200,
                        Height = 600, 
                        Content = new HomeViewModel(personWithAccount),
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
