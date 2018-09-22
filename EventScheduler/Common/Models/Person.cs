using Common.Helpers;
using Common.IModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Person : IPerson, INotifyPropertyChanged
    {

        #region Fields
        private Int32 _personID;
        private String _firstName;
        private String _lastName;
        private List<Event> _scheduledEvents;
        #endregion

        #region Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 PersonID
        {
            get => _personID;
            set
            {
                _personID = value;
            }
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged("LastName");
            }
        }

        public List<Event> ScheduledEvents
        {
            get
            {
                return _scheduledEvents;
            }

            private set
            {
                _scheduledEvents = value;
            }
        }
        #endregion

        public Person()
        {
            ScheduledEvents = new List<Event>();
        }

        public bool ScheduleParticipationInEvent(Event e)
        {
            if (ScheduledEvents.Contains(e, new EventComparer()))
            {
                //throw new Exception("Participation aready scheduled.");
                return false;
            }

            if(IsAvailableForEvent(e.ScheduledDateTimeBeging, e.ScheduledDateTimeEnd))
            {
                //throw new Exception("Person is not available at that time.");
                return false;
            }

            ScheduledEvents.Add(e);
            return true;
        }

        public bool CancleParticipationInEvent(Event e)
        {
            if (!ScheduledEvents.Contains(e, new EventComparer()))
            {
                //throw new Exception("Person is not participating in this event.");
                return false;
            }

            ScheduledEvents.Remove(e);
            return true;
        }

        public bool IsAvailableForEvent(DateTime begining, DateTime end)
        {
            foreach(Event e in ScheduledEvents)
            {
                //slobodno vreme pre sastanka?
                if(DateTime.Compare(begining, e.ScheduledDateTimeBeging) < 0)
                {
                    if(DateTime.Compare(end, e.ScheduledDateTimeBeging) > 0)
                    {
                        return false;
                    }
                }
                //slobodno vreme posle sastanka?
                else if(DateTime.Compare(begining, e.ScheduledDateTimeBeging) > 0)
                {
                    if (DateTime.Compare(begining, e.ScheduledDateTimeEnd) < 0)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
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
