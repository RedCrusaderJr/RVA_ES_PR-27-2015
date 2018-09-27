using Client.Commands;
using Client.Proxies;
using Common.IModels;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        

        public DeletePersonConfirmationViewModel(Person personToBeDeleted, ObservableCollection<Person> peopleList)
        {
            PersonToBeDeleted = personToBeDeleted;
            PeopleList = peopleList;

            DeletePersonCommand = new RelayCommand(DeleteAccountExecute, DeleteAccountCanExecute);
        }

        private void DeleteAccountExecute(object obj)
        {
            if (PersonProxy.Instance.PersonServices.DeletePerson(PersonToBeDeleted))
            {
                PeopleList.Remove(PersonToBeDeleted);
                object[] parameters = obj as object[];
                Window currentWindow = Window.GetWindow((UserControl)parameters[0]);
                currentWindow.Close();
            }
        }

        private bool DeleteAccountCanExecute(object arg)
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
