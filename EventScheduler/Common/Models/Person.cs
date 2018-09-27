using Common.Helpers;
using Common.IModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    [DataContract]
    public class Person : IPerson, INotifyPropertyChanged
    { 
        #region Fields
        private String _firstName;
        private String _lastName;
        private String _jmbg;
        private DateTime _lastEditTimeStamp;
        private List<Event> _scheduledEvents;
        #endregion

        #region Properties
        [Key]
        [DataMember]
        public string JMBG
        {
            get => _jmbg;
            set
            {
                _jmbg = value;
                OnPropertyChanged("JMBG");
            }
        }
        [DataMember]
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged("FirstName");
            }
        }
        [DataMember]
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged("LastName");
            }
        }
        [DataMember]
        public DateTime LastEditTimeStamp
        {
            get => _lastEditTimeStamp;
            set
            {
                _lastEditTimeStamp = value;
                OnPropertyChanged("LastEditTimeStamp");
            }
        }
        [DataMember]
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
            LastEditTimeStamp = DateTime.Now;
            ScheduledEvents = new List<Event>();
        }

        public Person(String jmbg, String firstName, String lastName)
        {
            JMBG = jmbg;
            LastEditTimeStamp = DateTime.Now;
            ScheduledEvents = new List<Event>();

            FirstName = firstName;
            LastName = lastName;
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

        public bool IsAvailableForEvent(DateTime? begining, DateTime? end)
        {
            if(begining == null || end == null)
            {
                return false;
            }

            foreach(Event e in ScheduledEvents)
            {
                if(e.ScheduledDateTimeBeging == null || e.ScheduledDateTimeEnd == null)
                {
                    return false;
                }

                //slobodno vreme pre sastanka?
                if(DateTime.Compare((DateTime)begining, (DateTime)e.ScheduledDateTimeBeging) < 0)
                {
                    if(DateTime.Compare((DateTime)end, (DateTime)e.ScheduledDateTimeBeging) > 0)
                    {
                        return false;
                    }
                }
                //slobodno vreme posle sastanka?
                else if(DateTime.Compare((DateTime)begining, (DateTime)e.ScheduledDateTimeBeging) > 0)
                {
                    if (DateTime.Compare((DateTime)begining, (DateTime)e.ScheduledDateTimeEnd) < 0)
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
