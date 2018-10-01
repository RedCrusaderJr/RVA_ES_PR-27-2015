using Client.Commands;
using Common;
using Common.Contracts;
using Common.Helpers;
using Common.Models;
using log4net;
using log4net.Repository.Hierarchy;
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
    public class SearchPeopleViewModel
    {
        private static readonly ILog logger = Log4netHelper.GetLogger();

        public ICommand SearchPeopleCommand { get; set; }
        public ICommand CloseSearchPersonCommand { get; set; }
        public Window CurrentWindow { get; set; }
        public Person SelectedPerson { get; set; }
        public Person ToBeSearchedPerson { get; set; }
        public ObservableCollection<Person> ListOfPeople { get; set; }
        public IPersonServices PersonProxy { get; set; }

        public SearchPeopleViewModel(ObservableCollection<Person> peopleList, IPersonServices personProxy)
        {
            SearchPeopleCommand = new RelayCommand(SearchPeopleExecute, SearchPeopleCanExecute);
            CloseSearchPersonCommand = new RelayCommand(CloseSearchPersonExecute, CloseSearchPersonCanExecute);

            PersonProxy = personProxy;
            ToBeSearchedPerson = new Person();
            SelectedPerson = new Person();
            ListOfPeople = new ObservableCollection<Person>(peopleList);

            logger.Debug("SearchPeopleViewModel constructor success.");
            LoggerHelper.Instance.LogMessage($"SearchPeopleViewModel constructor success.", EEventPriority.DEBUG, EStringBuilder.CLIENT);
        }

        private void SearchPeopleExecute(object parameter)
        {
            String firstNameSearch = ToBeSearchedPerson.FirstName == null ? "" : ToBeSearchedPerson.FirstName;
            String lastNameSearch = ToBeSearchedPerson.LastName == null ? "" : ToBeSearchedPerson.LastName;
            String jmbgNameSearch = ToBeSearchedPerson.JMBG == null ? "" : ToBeSearchedPerson.JMBG;

            ListOfPeople.Clear();
            PersonProxy.GetAllPeople().ForEach(p => ListOfPeople.Add(p));

            if(firstNameSearch.Equals("") && lastNameSearch.Equals("") && jmbgNameSearch.Equals(""))
            {
                return;
            }

            foreach (Person p in ListOfPeople.ToList())
            {
                if(!p.FirstName.Contains(firstNameSearch))
                {
                    ListOfPeople.Remove(p);
                }

                if (!p.LastName.Contains(lastNameSearch))
                {
                    ListOfPeople.Remove(p);
                }

                if (!p.JMBG.Contains(jmbgNameSearch))
                {
                    ListOfPeople.Remove(p);
                }
            }

            logger.Info("Search results");
            LoggerHelper.Instance.LogMessage($"Search results.", EEventPriority.INFO, EStringBuilder.CLIENT);
        }

        private bool SearchPeopleCanExecute(object parameter)
        {
            /*
            if (parameter == null || !(parameter is Object[] parameters) || parameters.Length != 3 
                                                                         || !(parameters[0] is String)
                                                                         || !(parameters[1] is String)
                                                                         || !(parameters[2] is String)
                                                                         || ((String)parameters[0] == String.Empty 
                                                                            && (String)parameters[1] == String.Empty 
                                                                            && (String)parameters[2] == String.Empty))
            {
                return false;
            }
            */
            return true;
        }

        private void CloseSearchPersonExecute(object obj)
        {
            object[] parameters = obj as object[];
            Window currentWindow = Window.GetWindow((UserControl)parameters[0]);
            currentWindow.Close();
        }

        private bool CloseSearchPersonCanExecute(object arg)
        {
            if (arg == null || !(arg is Object[] parameters) || parameters.Length != 1 || !(parameters[0] is UserControl))
            {
                return false;
            }

            return true;
        }
    }
}
