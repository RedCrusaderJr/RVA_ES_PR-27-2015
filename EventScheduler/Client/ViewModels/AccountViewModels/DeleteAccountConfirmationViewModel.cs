using Client.Commands;
using Client.Proxies;
using Common.Contracts;
using Common.Models;
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

namespace Client.ViewModels.AccountViewModels
{
    class DeleteAccountConfirmationViewModel
    {
        public ICommand DeleteAccountCommand { get; set; }
        public Account AccountToDelete { get; set; }
        public ObservableCollection<Account> Accounts { get; set; }
        public IAccountServices AccountProxy { get; set; }

        public DeleteAccountConfirmationViewModel(Account accountToDelete, ObservableCollection<Account> accounts, IAccountServices accountProxy)
        {
            AccountToDelete = accountToDelete;
            Accounts = accounts;

            DeleteAccountCommand = new RelayCommand(DeleteAccountExecute, DeleteAccountCanExecute);

            AccountProxy = accountProxy;
        }

        private void DeleteAccountExecute(object obj)
        {
            Account deletedAccount = AccountProxy.DeleteAccount(AccountToDelete);
            if(deletedAccount != null)
            {
                Account foundAccount = Accounts.FirstOrDefault(a => a.Username.Equals(deletedAccount.Username));
                Accounts.Remove(foundAccount);

                MessageBox.Show("Account successfully deleted.");
                object[] parameters = obj as object[];
                Window currentWindow = Window.GetWindow((UserControl)parameters[0]);
                currentWindow.Close();
            }
            else
            {
                MessageBox.Show("Error while deleting - server side.");
                object[] parameters = obj as object[];
                Window currentWindow = Window.GetWindow((UserControl)parameters[0]);
                currentWindow.Close();
            }
        }

        private bool DeleteAccountCanExecute(object arg)
        {
            if(arg == null || !(arg is Object[] parameters) || parameters.Length != 1
                                                            || !(parameters[0] is UserControl))
            {
                return false;
            }

            return true;
        }
    }
}
