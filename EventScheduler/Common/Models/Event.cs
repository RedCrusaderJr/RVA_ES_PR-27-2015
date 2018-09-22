using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using Common.Helpers;
using Common.IModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models
{
    public class Event : IEvent
    {
        #region Fields
        private int _eventID;
        private DateTime _createdTimeStamp;
        private DateTime _lastEditTimeStamp;
        private DateTime _scheduledDateTimeBeging;
        private DateTime _scheduledDateTimeEnd;
        private string _eventTitle;
        private string _description;
        private List<Person> _participants;
        #endregion

        #region Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 EventID { get => _eventID; set => _eventID = value; }
        [Required]
        public DateTime CreatedTimeStamp
        {
            get => _createdTimeStamp;
            private set => _createdTimeStamp = value;
        }
        public DateTime LastEditTimeStamp
        {
            get => _lastEditTimeStamp;
            set => _lastEditTimeStamp = value;
        }
        public DateTime ScheduledDateTimeBeging
        {
            get => _scheduledDateTimeBeging;
            set => _scheduledDateTimeBeging = value;
        }
        public DateTime ScheduledDateTimeEnd
        {
            get => _scheduledDateTimeEnd;
            set => _scheduledDateTimeEnd = value;
        }
        public string EventTitle
        {
            get => _eventTitle;
            set => _eventTitle = value;
        }
        public String Description
        {
            get => _description;
            set => _description = value;
        }
        public List<Person> Participants
        {
            get => _participants;
            private set => _participants = value;
        }
        #endregion

        public Event()
        {
            Participants = new List<Person>();

            CreatedTimeStamp = DateTime.Now;
            LastEditTimeStamp = CreatedTimeStamp;
        }

        public Event(DateTime scheduledDateTimeBegining, DateTime scheduledDateTimeEnd)
        {
            EventID = this.GetHashCode();
            //Participants = new List<Person>();
           
            CreatedTimeStamp = DateTime.Now;
            LastEditTimeStamp = CreatedTimeStamp;
            ScheduledDateTimeBeging = scheduledDateTimeBegining;
            ScheduledDateTimeEnd = scheduledDateTimeEnd;
        }

        public Event(DateTime scheduledDateTimeBegining, DateTime scheduledDateTimeEnd, String description)
        {
            EventID = this.GetHashCode();
            //Participants = new List<Person>();

            CreatedTimeStamp = DateTime.Now;
            LastEditTimeStamp = CreatedTimeStamp;
            ScheduledDateTimeBeging = scheduledDateTimeBegining;
            ScheduledDateTimeEnd = scheduledDateTimeEnd;
            Description = description;
        }

        public bool AddParticipant(Person participant)
        {
            if(ScheduledDateTimeBeging == null || ScheduledDateTimeEnd == null)
            {
                //throw new Exception("Time of begining or end of the event not specified.");
                return false;
            }

            if(Participants.Contains(participant, new PersonComparer()))
            {
                //throw new Exception("Person already added to the event.");
                return false;
            }

            if(!participant.IsAvailableForEvent(ScheduledDateTimeBeging, ScheduledDateTimeEnd))
            {
                //throw new Exception("Person is not available at that time.");
                return false;
            }

            Participants.Add(participant);
            return true;
        }
        
        public bool RemoveParticipant(Person participant)
        {
            if (!Participants.Contains(participant, new PersonComparer()))
            {
                //throw new Exception("Person is not participating in this event.");
                return false;
            }

            Participants.Remove(participant);
            return true;
        }
    }
}
