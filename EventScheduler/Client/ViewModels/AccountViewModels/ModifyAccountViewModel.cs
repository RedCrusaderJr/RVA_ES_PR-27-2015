using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Client.Commands;
using Client.Proxies;
using Common.Models;

namespace Client.ViewModels.AccountViewModels
{
    class ModifyAccountViewModel
    {
        public ICommand ModifyAccountCommand { get; set; }
        public Window CurrentWindow { get; set; }
        public Account SelectedAccount { get; set; }
        public Account AccountToModify { get; set; }
        public ObservableCollection<Account> AccountsList { get; set;}

        public ModifyAccountViewModel(Account selectedAccount, ObservableCollection<Account> accountsList)
        {
            ModifyAccountCommand = new RelayCommand(ModifyAccountExecute, ModifyAccountCanExecute);
            SelectedAccount = selectedAccount;
            AccountToModify = new Account(SelectedAccount.Username)
            {
                Password = SelectedAccount.Password,
                FirstName = SelectedAccount.FirstName,
                LastName = SelectedAccount.LastName,
                Role = SelectedAccount.Role,
            };
            AccountsList = accountsList;
        }

        private void ModifyAccountExecute(object parameter)
        {
            Object[] parameters = parameter as Object[];

            if (AccountProxy.Instance.AccountServices.ModifyAccount(AccountToModify))
            {
                Account AccountInTheList = AccountsList.First(a => a.Username.Equals(AccountToModify.Username));
                AccountInTheList.Password = AccountToModify.Password;
                AccountInTheList.FirstName = AccountToModify.FirstName;
                AccountInTheList.LastName = AccountToModify.LastName;

                UserControl uc = parameters[0] as UserControl;
                Window window = Window.GetWindow(uc);
                window.Close();
            }
            else
            {
                MessageBox.Show("Error while modifying - server side");
            }
        }

        private bool ModifyAccountCanExecute(object parameter)
        {
            if (parameter == null || !(parameter is Object[] parameters) || parameters.Length != 1)
            {
                return false;
            }

            return true;
        }
    }
}
