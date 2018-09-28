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
using System.Runtime.Serialization;

namespace Common.Models
{
    [DataContract]
    public class Event : IEvent
    {
        #region Fields
        private int _eventId;
        private DateTime _createdTimeStamp;
        private DateTime _lastEditTimeStamp;
        private DateTime? _scheduledDateTimeBeging;
        private DateTime? _scheduledDateTimeEnd;
        private string _eventTitle;
        private string _description;
        private List<Person> _participants;
        #endregion

        #region Properties
        [Key]
        [DataMember]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 EventId
        {
            get => _eventId;
            set => _eventId = value;
        }
        [Required]
        [DataMember]
        public DateTime CreatedTimeStamp
        {
            get => _createdTimeStamp;
            private set => _createdTimeStamp = value;
        }
        [DataMember]
        public DateTime LastEditTimeStamp
        {
            get => _lastEditTimeStamp;
            set => _lastEditTimeStamp = value;
        }
        [DataMember]
        public DateTime? ScheduledDateTimeBeging
        {
            get => _scheduledDateTimeBeging;
            set => _scheduledDateTimeBeging = value;
        }
        [DataMember]
        public DateTime? ScheduledDateTimeEnd
        {
            get => _scheduledDateTimeEnd;
            set => _scheduledDateTimeEnd = value;
        }
        [DataMember]
        public String EventTitle
        {
            get => _eventTitle;
            set => _eventTitle = value;
        }
        [DataMember]
        public String Description
        {
            get => _description;
            set => _description = value;
        }
        [DataMember]
        //[InverseProperty("ScheduledEvents")]
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
            Participants = new List<Person>();
           
            CreatedTimeStamp = DateTime.Now;
            LastEditTimeStamp = CreatedTimeStamp;
            ScheduledDateTimeBeging = scheduledDateTimeBegining;
            ScheduledDateTimeEnd = scheduledDateTimeEnd;
        }

        public Event(DateTime scheduledDateTimeBegining, DateTime scheduledDateTimeEnd, String title, String description)
        {
            Participants = new List<Person>();

            CreatedTimeStamp = DateTime.Now;
            LastEditTimeStamp = CreatedTimeStamp;
            ScheduledDateTimeBeging = scheduledDateTimeBegining;
            ScheduledDateTimeEnd = scheduledDateTimeEnd;
            EventTitle = title;
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

            Person participantToBeRemoved = Participants.FirstOrDefault(p => p.JMBG.Equals(participant.JMBG));
            Participants.Remove(participantToBeRemoved);
            return true;
        }
    }
}
