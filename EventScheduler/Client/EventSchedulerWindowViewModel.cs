using Client.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Client
{
    public class EventSchedulerWindowViewModel : INotifyPropertyChanged
    {
        private UserControl _mainUserControl;
        private UserControl _messagesUserControl;

        public UserControl MainUserControl
        {
            get => _mainUserControl;
            set
            {
                _mainUserControl = value;
                OnPropertyChanged("MainUserControl");
            }
        }
        public UserControl MessagesUserControl
        {
            get => _messagesUserControl;
            set 
            {
                _messagesUserControl = value;
                OnPropertyChanged("MessagesUserControl");
            }
        }

        public EventSchedulerWindowViewModel()
        {
            MessagesUserControl = new UserControl()
            {
                Content = new MessagesViewModel(),
            };

            MainUserControl = new UserControl()
            {
                Content = new LoginViewModel(MessagesUserControl),
            };
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion
    }
}
