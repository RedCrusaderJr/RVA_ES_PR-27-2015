using Client.Commands;
using Client.Proxies;
using Common;
using Common.BaseCommandPattern;
using Common.BaseCommandPattern.PersonCommands;
using Common.Contracts;
using Common.Helpers;
using Common.Models;
using Common.PersonCommands;
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

namespace Client.ViewModels.PersonViewModels
{
    public class AddPersonViewModel
    {
        private static readonly ILog logger = Log4netHelper.GetLogger();

        public ICommand AddPersonCommand { get; set; }
        public Window CurrentWindow { get; set; }
        public Person PersonToAdd { get; set; }
        public ERole SelectedOption { get; set; }
        public ObservableCollection<Person> PeopleList { get; set; }

        public IPersonServices PersonProxy { get; set; }
        public ICommandInvoker CommandInvoker { get; set; }

        public AddPersonViewModel(ObservableCollection<Person> peopleList, IPersonServices personProxy, ICommandInvoker commandInvoker)
        {
            AddPersonCommand = new RelayCommand(AddPersonExecute, AddPersonCanExecute);
            PeopleList = peopleList;
            PersonProxy = personProxy;
            CommandInvoker = commandInvoker;

            logger.Debug("AddPersonViewModel constructor successful call.");
            LoggerHelper.Instance.LogMessage("AddPersonViewModel constructor successful call.", EEventPriority.DEBUG, EStringBuilder.CLIENT);
        }

        private void AddPersonExecute(object parameter)
        {
            Object[] parameters = parameter as Object[];
            PersonToAdd = new Person()
            {  
                FirstName = parameters[0] as String,
                LastName = parameters[1] as String,
                JMBG = parameters[2] as String,
            };

            Person addedPerson = null;
            try
            {
                addedPerson = PersonProxy.AddPerson(PersonToAdd);
            }
            catch(Exception e)
            {
                logger.Error("AddPerson error on server.");
                LoggerHelper.Instance.LogMessage($"AddPerson error on server. Message: {e.Message}", EEventPriority.ERROR, EStringBuilder.CLIENT);
            }
            
            if (addedPerson != null)
            {
                CommandInvoker.RegisterCommand(new AddPersonCommand(new PersonCommandReciever(), PersonToAdd, PersonProxy));

                logger.Error("Person successfully added.");
                LoggerHelper.Instance.LogMessage($"Person successfully added.", EEventPriority.INFO, EStringBuilder.CLIENT);

                UserControl uc = parameters[3] as UserControl;
                CurrentWindow = Window.GetWindow(uc);
                CurrentWindow.Close();
            }
            else
            {
                logger.Error("Error on server or username already exists.");
                LoggerHelper.Instance.LogMessage($"Error on server or username already exists.", EEventPriority.ERROR, EStringBuilder.CLIENT);
                
                UserControl uc = parameters[3] as UserControl;
                CurrentWindow = Window.GetWindow(uc);
                CurrentWindow.Close();

            }
        }

        private bool AddPersonCanExecute(object parameter)
        {
            if(parameter == null || !(parameter is Object[] parameters) || (String)parameters[0] == String.Empty
                                                                        || (String)parameters[1] == String.Empty
                                                                        || (String)parameters[2] == String.Empty
                                                                        || !(parameters[3] is UserControl))
            {
                return false;
            }

            return true;
        }
    }
}
