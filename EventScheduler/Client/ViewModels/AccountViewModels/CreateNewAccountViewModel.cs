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

namespace Client.ViewModels.AccountViewModels
{
    class CreateNewAccountViewModel
    {
        public ICommand CreateNewAccountCommand { get; set; }

        public Window CurrentWindow { get; set; }
        public Account AccountToAdd { get; set; }
        public ERole SelectedOption { get; set; }


        public CreateNewAccountViewModel()
        {
            CreateNewAccountCommand = new RelayCommand(CreateNewAccountExecute, CreateNewAccountCanExecute);
        }

        private void CreateNewAccountExecute(object parameter)
        {
            Object[] parameters = parameter as Object[];
            String username = parameters[0] as String;
            String password = parameters[1] as String;
            String firstName = parameters[2] as String;
            String lastName = parameters[3] as String;
            String jmbg = parameters[4] as String;
            object roleRegular = parameters[5];
            object roleAdmin = parameters[6];
            UserControl CreateNewAccountUserControl = parameters[7] as UserControl;

            AccountToAdd = new Account(username)
            {
                FirstName = firstName,
                LastName = lastName,
                Password = password,
                Role = (bool)roleRegular ? ERole.REGULAR : ERole.ADMIN,
            };
   
            if (AccountProxy.Instance.AccountServices.CreateNewAccount(AccountToAdd))
            {
                CurrentWindow = Window.GetWindow(CreateNewAccountUserControl);
                CurrentWindow.Close();
            }
            else
            {
                MessageBox.Show("Error on server or username already exists.");
                CurrentWindow = Window.GetWindow(CreateNewAccountUserControl);
                CurrentWindow.Close();
            }
            
        }

        private bool CreateNewAccountCanExecute(object parameter)
        {
            if (parameter == null || !(parameter is Object[] parameters) || (String)parameters[0] == String.Empty
                                                                        || (String)parameters[1] == String.Empty
                                                                        || (String)parameters[2] == String.Empty
                                                                        || (String)parameters[3] == String.Empty
                                                                        || (String)parameters[4] == String.Empty
                                                                        || (bool)parameters[5] == (bool)parameters[6])
            {
                return false;
            }

            return true;
        }
    }
}
