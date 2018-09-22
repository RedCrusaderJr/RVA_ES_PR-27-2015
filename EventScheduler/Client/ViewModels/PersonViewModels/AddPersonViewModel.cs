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

namespace Client.ViewModels.PersonViewModels
{
    public class AddPersonViewModel
    {
        public ICommand AddPersonCommand { get; set; }
        public Window CurrentWindow { get; set; }
        public Person PersonToAdd { get; set; }
        public ERole SelectedOption { get; set; }


        public AddPersonViewModel()
        {
            AddPersonCommand = new RelayCommand(AddPersonExecute, AddPersonCanExecute);
            //SelectedOption = ERole.REGULAR;
        }

        private void AddPersonExecute(object parameter)
        {
            Object[] parameters = parameter as Object[];
            PersonToAdd = new Person()
            {
                //Username = parameters[0] as String,
                //Password = parameters[1] as String,
                FirstName = parameters[2] as String,
                LastName = parameters[3] as String,
                //Role = (bool)parameters[4] ? ERole.REGULAR : ERole.ADMIN,
            };

            /*
            if ((bool)parameters[4])
            {
                PersonToAdd.Role = ERole.REGULAR;
            }
            else if ((bool)parameters[5])
            {
                PersonToAdd.Role = ERole.ADMIN;
            }
            */

            /*
            if (Proxy.Instance.BasicOperations.AddPerson(PersonToAdd))
            {
                UserControl uc = parameters[6] as UserControl;
                CurrentWindow = Window.GetWindow(uc);
                CurrentWindow.Close();
            }
            else
            {
                MessageBox.Show("Error on server or username already exists.");
                UserControl uc = parameters[6] as UserControl;
                CurrentWindow = Window.GetWindow(uc);
                CurrentWindow.Close();

            }
            */
        }

        private bool AddPersonCanExecute(object parameter)
        {
            if(parameter == null || !(parameter is Object[] parameters) || (String)parameters[0] == String.Empty
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
