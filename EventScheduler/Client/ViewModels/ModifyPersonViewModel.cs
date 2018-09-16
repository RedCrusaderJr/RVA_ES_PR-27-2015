using Client.Commands;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Client.ViewModels
{
    public class ModifyPersonViewModel
    {
        public ICommand ModifyPersonCommand { get; set; }
        public Window CurrentWindow { get; set; }
        public Person LoggedInPerson { get; set; }
        public Person PersonToModify { get; set; }

        public ModifyPersonViewModel(Person person)
        {
            ModifyPersonCommand = new RelayCommand(ModifyPersonExecute, ModifyPersonCanExecute);
            LoggedInPerson = person;
            PersonToModify = new Person()
            {
                Username = person.Username,
                Password = person.Password,
                FirstName = person.FirstName,
                LastName = person.LastName,
            };
        }

        private void ModifyPersonExecute(object parameter)
        {
            Object[] parameters = parameter as Object[];

            if (Proxy.Instance.BasicOperations.ModifyPerson(PersonToModify))
            {
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
