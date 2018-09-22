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

namespace Client.ViewModels.AccountViewModels
{
    class CreateNewAccountViewModel
    {
        public ICommand CreateNewAccountCommand { get; set; }

        public Window CurrentWindow { get; set; }
        public PersonWithAccount AccountToAdd { get; set; }
        public ERole SelectedOption { get; set; }


        public CreateNewAccountViewModel()
        {
            CreateNewAccountCommand = new RelayCommand(CreateNewAccountExecute, CreateNewAccountCanExecute);
            AccountToAdd = new PersonWithAccount();
        }

        private void CreateNewAccountExecute(object parameter)
        {
            Object[] parameters = parameter as Object[];
            AccountToAdd = new PersonWithAccount()
            {
                Username = parameters[0] as String,
                Password = parameters[1] as String,
                FirstName = parameters[2] as String,
                LastName = parameters[3] as String,
                Role = (bool)parameters[4] ? ERole.REGULAR : ERole.ADMIN,
            };
   
            if (AccountProxy.Instance.AccountServices.CreateNewAccount(AccountToAdd))
            {
                UserControl uc = parameters[6] as UserControl;
                CurrentWindow = Window.GetWindow(uc);
                CurrentWindow.Close();
            }
            else
            {
                MessageBox.Show("Error on server or username already exists.");
                UserControl uc = parameters[6] as UserControl;
                CurrentWindow = Window.GetWindow(uc);
                CurrentWindow.Close();
            }
            
        }

        private bool CreateNewAccountCanExecute(object parameter)
        {
            if (parameter == null || !(parameter is Object[] parameters) || (String)parameters[0] == String.Empty
                                                                        || (String)parameters[1] == String.Empty
                                                                        || (String)parameters[2] == String.Empty
                                                                        || (String)parameters[3] == String.Empty
                                                                        || (bool)parameters[4] == (bool)parameters[5])
            {
                return false;
            }

            return true;
        }
    }
}
