using Client.Commands;
using Client.Proxies;
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
    public class AddPersonViewModel
    {
        public ICommand AddPersonCommand { get; set; }
        public Window CurrentWindow { get; set; }
        public Person PersonToAdd { get; set; }
        public ERole SelectedOption { get; set; }
        public ObservableCollection<Person> PeopleList { get; set; }

        public AddPersonViewModel(ObservableCollection<Person> peopleList)
        {
            AddPersonCommand = new RelayCommand(AddPersonExecute, AddPersonCanExecute);
            PeopleList = peopleList;
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

            if (PersonProxy.Instance.PersonServices.AddPerson(PersonToAdd))
            {
                PeopleList.Add(PersonToAdd);

                MessageBox.Show("Person successfully added.");
                UserControl uc = parameters[3] as UserControl;
                CurrentWindow = Window.GetWindow(uc);
                CurrentWindow.Close();
            }
            else
            {
                MessageBox.Show("Error on server or username already exists.");
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
