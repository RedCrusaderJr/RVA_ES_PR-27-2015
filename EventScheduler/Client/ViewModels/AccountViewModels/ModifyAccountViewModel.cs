using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Client.Commands;
using Client.Proxies;
using Common.Contracts;
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
        public IAccountServices AccountProxy { get; set; }

        public ModifyAccountViewModel(Account selectedAccount, ObservableCollection<Account> accountsList, IAccountServices accountProxy)
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
            AccountProxy = accountProxy;
        }

        private void ModifyAccountExecute(object parameter)
        {
            Object[] parameters = parameter as Object[];

            Account modifiedAccount = AccountProxy.ModifyAccount(AccountToModify);
            if (modifiedAccount != null)
            {
                Account foundAccount = AccountsList.FirstOrDefault(a => a.Username.Equals(modifiedAccount.Username));
                AccountsList.Remove(foundAccount);
                AccountsList.Add(modifiedAccount);

                MessageBox.Show("Account successfully modified.");
                UserControl uc = parameters[0] as UserControl;
                Window window = Window.GetWindow(uc);
                window.Close();
            }
            else
            {
                MessageBox.Show("Error while modifying - server side");
                UserControl uc = parameters[0] as UserControl;
                Window window = Window.GetWindow(uc);
                window.Close();
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
