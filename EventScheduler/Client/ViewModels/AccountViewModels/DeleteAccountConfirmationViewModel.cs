using Client.Commands;
using Client.Proxies;
using Common;
using Common.Contracts;
using Common.Helpers;
using Common.Models;
using log4net;
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
        private static readonly ILog logger = Log4netHelper.GetLogger();

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
            Account deletedAccount = null;
            try
            {
                deletedAccount = AccountProxy.DeleteAccount(AccountToDelete);
            }
            catch (Exception e)
            {

                logger.Error($"Error while deleting - server side. Message: {e.Message}");
                LoggerHelper.Instance.LogMessage($"Error while deleting - server side. Message: {e.Message}", EEventPriority.ERROR, EStringBuilder.CLIENT);

            }

            if (deletedAccount != null)
            {
                Account foundAccount = Accounts.FirstOrDefault(a => a.Username.Equals(deletedAccount.Username));
                Accounts.Remove(foundAccount);

                logger.Info("Account successfully deleted.");
                LoggerHelper.Instance.LogMessage($"Account successfully deleted.", EEventPriority.INFO, EStringBuilder.CLIENT);
                
                object[] parameters = obj as object[];
                Window currentWindow = Window.GetWindow((UserControl)parameters[0]);
                currentWindow.Close();
            }
            else
            {
                logger.Error("Error while deleting - server side.");
                LoggerHelper.Instance.LogMessage($"Account successfully deleted.", EEventPriority.ERROR, EStringBuilder.CLIENT);

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
