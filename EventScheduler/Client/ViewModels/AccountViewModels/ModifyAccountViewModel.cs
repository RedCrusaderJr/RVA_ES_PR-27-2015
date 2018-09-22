using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Client.Commands;
using Common.Models;

namespace Client.ViewModels.AccountViewModels
{
    class ModifyAccountViewModel
    {
        public ICommand ModifyPersonCommand { get; set; }
        public Window CurrentWindow { get; set; }
        public PersonWithAccount SelectedPersonWithAccount { get; set; }
        public Person PersonToModify { get; set; }

        public ModifyAccountViewModel(PersonWithAccount selectedPersonWithAccount)
        {
            ModifyPersonCommand = new RelayCommand(ModifyPersonExecute, ModifyPersonCanExecute);
            SelectedPersonWithAccount = selectedPersonWithAccount;
            PersonToModify = new PersonWithAccount()
            {
                Username = selectedPersonWithAccount.Username,
                Password = selectedPersonWithAccount.Password,
                FirstName = selectedPersonWithAccount.FirstName,
                LastName = selectedPersonWithAccount.LastName,
                Role = selectedPersonWithAccount.Role,
            };
        }

        private void ModifyPersonExecute(object parameter)
        {
            Object[] parameters = parameter as Object[];

            if (PersonProxy.Instance.PersonServices.ModifyPerson(PersonToModify))
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
