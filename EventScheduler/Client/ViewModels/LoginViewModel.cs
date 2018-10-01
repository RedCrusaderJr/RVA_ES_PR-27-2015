using Client.Commands;
using Client.Proxies;
using Common.Models;
using log4net;
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
        private static readonly ILog logger = Log4netHelper.GetLogger();

        public ICommand LoginCommand { get; set; }
        public UserControl CurrentUserControl { get; set; }
        public Account LoggedInPerson { get; set; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Execute, CanExecute);
        }

        private void Execute(object parameter)
        {
            Object[] parameters = parameter as Object[];

            String username = parameters[0] as String;
            String password = parameters[1] as String;
            CurrentUserControl = parameters[2] as UserControl;

            try
            {
                Account account = LoginProxy.ConnectToLoginService().Login(username, password);
                if(account == null)
                { 
                    MessageBox.Show("Username or password is incorrect.");
                }
                else
                {
                    MessageBox.Show("Successfull login!");

                    CurrentUserControl.Content = new HomeViewModel(account);
                    CurrentUserControl.VerticalContentAlignment = VerticalAlignment.Top;
                    CurrentUserControl.HorizontalContentAlignment = HorizontalAlignment.Left;
                    CurrentUserControl.Width = 1500;
                    CurrentUserControl.Height = 1000;
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
