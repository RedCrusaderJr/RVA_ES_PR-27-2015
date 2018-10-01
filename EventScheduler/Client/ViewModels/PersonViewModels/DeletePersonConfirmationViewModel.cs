using Client.Commands;
using Client.Proxies;
using Common.BaseCommandPattern;
using Common.Contracts;
using Common.IModels;
using Common.Models;
using Common.PersonCommands;
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
    class DeletePersonConfirmationViewModel
    {

        public ICommand DeletePersonCommand { get; set; }
        public Person PersonToBeDeleted { get; set; }
        public ObservableCollection<Person> PeopleList { get; set; }
        public IPersonServices PersonProxy { get; set; }
        public ICommandInvoker CommandInvoker { get; set; }

        public DeletePersonConfirmationViewModel(Person personToBeDeleted, ObservableCollection<Person> peopleList, IPersonServices personProxy, ICommandInvoker commandInvoker)
        {
            PersonToBeDeleted = personToBeDeleted;
            PeopleList = peopleList;

            DeletePersonCommand = new RelayCommand(DeletePersonConfirmationExecute, DeletePersonConfirmationCanExecute);

            PersonProxy = personProxy;
            CommandInvoker = commandInvoker;
        }

        private void DeletePersonConfirmationExecute(object obj)
        {
            Person deletedPerson = PersonProxy.DeletePerson(PersonToBeDeleted);
            if(deletedPerson != null)
            {
                CommandInvoker.RegisterCommand(new DeletePersonCommand(new PersonCommandReciever(), deletedPerson, PersonProxy));

                MessageBox.Show("Person successfully deleted.");
                object[] parameters = obj as object[];
                Window currentWindow = Window.GetWindow((UserControl)parameters[0]);
                currentWindow.Close();
            }
            else
            {
                MessageBox.Show("Error while deleting - server side");
                object[] parameters = obj as object[];
                Window currentWindow = Window.GetWindow((UserControl)parameters[0]);
                currentWindow.Close();
            }
        }

        private bool DeletePersonConfirmationCanExecute(object arg)
        {
            if (arg == null || !(arg is Object[] parameters) || parameters.Length != 1
                                                             || !(parameters[0] is UserControl))
            {
                return false;
            }

            return true;
        }
    }
}
