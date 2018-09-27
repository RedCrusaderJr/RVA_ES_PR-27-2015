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
    public class ModifyPersonViewModel
    {
        public ICommand ModifyPersonCommand { get; set; }
        public Window CurrentWindow { get; set; }
        public Person PersonToModify { get; set; }
        public ObservableCollection<Person> PeopleList { get; set; }

        public ModifyPersonViewModel(Person person, ObservableCollection<Person> peopleList)
        {
            ModifyPersonCommand = new RelayCommand(ModifyPersonExecute, ModifyPersonCanExecute);
            PeopleList = peopleList;
            PersonToModify = new Person()
            {
                FirstName = person.FirstName,
                LastName = person.LastName,
                JMBG = person.JMBG,
            };
            
        }

        private void ModifyPersonExecute(object parameter)
        {
            Object[] parameters = parameter as Object[];

            if (PersonProxy.Instance.PersonServices.ModifyPerson(PersonToModify))
            {
                PersonToModify = PersonProxy.Instance.PersonServices.GetSinglePerson(PersonToModify.JMBG);

                Person personInList = PeopleList.First(p => p.JMBG.Equals(PersonToModify.JMBG));
                personInList.FirstName = PersonToModify.FirstName;
                personInList.LastName = PersonToModify.LastName;
                personInList.LastEditTimeStamp = PersonToModify.LastEditTimeStamp;

                UserControl uc = parameters[0] as UserControl;
                Window window = Window.GetWindow(uc);
                window.Close();
            }
            else
            {
                MessageBox.Show("Error while modifying - server side");
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
