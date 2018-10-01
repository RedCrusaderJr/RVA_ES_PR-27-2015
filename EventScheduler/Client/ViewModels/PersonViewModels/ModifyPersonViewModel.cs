using Client.Commands;
using Client.Proxies;
using Common;
using Common.BaseCommandPattern;
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
    public class ModifyPersonViewModel
    {
        private static readonly ILog logger = Log4netHelper.GetLogger();

        public ICommand ModifyPersonCommand { get; set; }
        public Window CurrentWindow { get; set; }
        public Person PersonToModify { get; set; }
        public ObservableCollection<Person> PeopleList { get; set; }
        public IPersonServices PersonProxy { get; set; }
        public ICommandInvoker CommandInvoker { get; set; }

        public ModifyPersonViewModel(Person person, ObservableCollection<Person> peopleList, IPersonServices personProxy, ICommandInvoker commandInvoker)
        {
            ModifyPersonCommand = new RelayCommand(ModifyPersonExecute, ModifyPersonCanExecute);
            PeopleList = peopleList;
            PersonToModify = new Person()
            {
                FirstName = person.FirstName,
                LastName = person.LastName,
                JMBG = person.JMBG,
            };

            PersonProxy = personProxy;
            CommandInvoker = commandInvoker;

            logger.Debug("ModifyPersonViewModel constructor success.");
            LoggerHelper.Instance.LogMessage($"ModifyPersonViewModel constructor success.", EEventPriority.DEBUG, EStringBuilder.CLIENT);
        }

        private void ModifyPersonExecute(object parameter)
        {
            Object[] parameters = parameter as Object[];

            Person modifiedPerson = null;

            try
            {
                modifiedPerson = PersonProxy.ModifyPerson(PersonToModify);
            }
            catch (Exception e)
            {

                logger.Error("Person successfully modified.");
                LoggerHelper.Instance.LogMessage($"Person successfully modified.", EEventPriority.ERROR, EStringBuilder.CLIENT);
            }
            
            if (modifiedPerson != null)
            {
                CommandInvoker.RegisterCommand(new AddPersonCommand(new PersonCommandReciever(), modifiedPerson, PersonProxy));

                logger.Info("Person successfully modified.");
                LoggerHelper.Instance.LogMessage($"Person successfully modified.", EEventPriority.INFO, EStringBuilder.CLIENT);

                UserControl uc = parameters[0] as UserControl;
                Window window = Window.GetWindow(uc);
                window.Close();
            }
            else
            {
                logger.Error("Error while modifying - server side");
                LoggerHelper.Instance.LogMessage($"Error while modifying - server side", EEventPriority.ERROR, EStringBuilder.CLIENT);

                UserControl uc = parameters[0] as UserControl;
                Window window = Window.GetWindow(uc);
                window.Close();
            }
        }

        private bool ModifyPersonCanExecute(object parameter)
        {
            if (parameter == null || !(parameter is Object[] parameters) || parameters.Length != 1)
            {
                return false;
            }

            return true;
        }
    }
}
