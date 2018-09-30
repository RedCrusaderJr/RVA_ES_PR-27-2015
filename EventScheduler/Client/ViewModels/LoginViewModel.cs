using Client.Commands;
using Client.Proxies;
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
        public Account LoggedInPerson { get; set; }
        public UserControl MessagesUserControl { get; set; }
        public TextBlock InfoBlock { get; set; }

        public LoginViewModel(UserControl messagesUserControl)
        {
            LoginCommand = new RelayCommand(Execute, CanExecute);
            MessagesUserControl = messagesUserControl;
            //TAKO BLIZU A TAKO DALEKO
            InfoBlock = MessagesUserControl.FindName("InfoBlock") as TextBlock;
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
                    //InfoBlock.Text += "Username or password is incorrect.\n";
                    MessageBox.Show("Username or password is incorrect.");
                }
                else
                {
                    //InfoBlock.Text += "Successfull login!\n";
                    MessageBox.Show("Successfull login!");

                    CurrentUserControl.Content = new HomeViewModel(account, MessagesUserControl);
                    CurrentUserControl.VerticalContentAlignment = VerticalAlignment.Top;
                    CurrentUserControl.HorizontalContentAlignment = HorizontalAlignment.Left;
                    CurrentUserControl.Width = 1500;
                    CurrentUserControl.Height = 1000;
                }
            }
            catch(Exception e)
            {
                //InfoBlock.Text += $"Problem with connection. Message: {e.Message}";
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
