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
using Common;
using Common.Contracts;
using Common.Helpers;
using Common.Models;
using log4net;

namespace Client.ViewModels.AccountViewModels
{
    class ModifyAccountViewModel
    {
        private static readonly ILog logger = Log4netHelper.GetLogger();

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


            Account modifiedAccount = null;
            try
            {
                modifiedAccount = AccountProxy.ModifyAccount(AccountToModify);
            }
            catch (Exception e)
            {

                logger.Error($"Error while deleting - server side. Message: {e.Message}");
                LoggerHelper.Instance.LogMessage($"Error while deleting - server side. Message: {e.Message}", EEventPriority.ERROR, EStringBuilder.CLIENT);
            }
            
            if (modifiedAccount != null)
            {
                Account foundAccount = AccountsList.FirstOrDefault(a => a.Username.Equals(modifiedAccount.Username));
                AccountsList.Remove(foundAccount);
                AccountsList.Add(modifiedAccount);

                logger.Info($"Account successfully modified.");
                LoggerHelper.Instance.LogMessage($"Account successfully modified.", EEventPriority.INFO, EStringBuilder.CLIENT);
                UserControl uc = parameters[0] as UserControl;
                Window window = Window.GetWindow(uc);
                window.Close();
            }
            else
            {

                logger.Error($"Error while deleting - server side.");
                LoggerHelper.Instance.LogMessage($"Error while deleting - server side.", EEventPriority.ERROR, EStringBuilder.CLIENT);
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
