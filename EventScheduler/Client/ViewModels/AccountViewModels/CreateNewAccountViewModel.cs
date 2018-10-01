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
    class CreateNewAccountViewModel
    {
        private static readonly ILog logger = Log4netHelper.GetLogger();

        public ICommand CreateNewAccountCommand { get; set; }

        public Window CurrentWindow { get; set; }
        public Account AccountToAdd { get; set; }
        public ERole SelectedOption { get; set; }
        public ObservableCollection<Account> AccountsList { get; set; }
        public IAccountServices AccountProxy { get; set; }


        public CreateNewAccountViewModel(ObservableCollection<Account> accounts, IAccountServices accountProxy)
        {
            CreateNewAccountCommand = new RelayCommand(CreateNewAccountExecute, CreateNewAccountCanExecute);
            AccountsList = accounts;
            AccountProxy = accountProxy;
        }

        private void CreateNewAccountExecute(object parameter)
        {
            Object[] parameters = parameter as Object[];
            String username = parameters[0] as String;
            String password = parameters[1] as String;
            String firstName = parameters[2] as String;
            String lastName = parameters[3] as String;
            object roleRegular = parameters[4];
            object roleAdmin = parameters[5];
            UserControl CreateNewAccountUserControl = parameters[6] as UserControl;

            AccountToAdd = new Account(username)
            {
                FirstName = firstName,
                LastName = lastName,
                Password = password,
                Role = (bool)roleRegular ? ERole.REGULAR : ERole.ADMIN,
            };


            Account createdAccount = null;
            try
            {
                createdAccount = AccountProxy.CreateNewAccount(AccountToAdd);
            }
            catch (Exception e)
            {

                logger.Error($"Error while deleting - server side. Message: {e.Message}");
                LoggerHelper.Instance.LogMessage($"Error while deleting - server side. Message: {e.Message}", EEventPriority.ERROR, EStringBuilder.CLIENT);
            }
            if (createdAccount != null)
            {
                AccountsList.Add(createdAccount);

                logger.Error($"Account successfuly added.");
                LoggerHelper.Instance.LogMessage($"Account successfuly added.", EEventPriority.ERROR, EStringBuilder.CLIENT);

                CurrentWindow = Window.GetWindow(CreateNewAccountUserControl);
                CurrentWindow.Close();
            }
            else
            {
                logger.Error($"Error on server or username already exists.");
                LoggerHelper.Instance.LogMessage($"Error on server or username already exists.", EEventPriority.ERROR, EStringBuilder.CLIENT);

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
                                                                        || (bool)parameters[4] == (bool)parameters[5])
            {
                return false;
            }

            return true;
        }
    }
}
