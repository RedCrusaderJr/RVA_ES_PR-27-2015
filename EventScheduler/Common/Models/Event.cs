using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace Common.Models
{
    public class Event
    {
        [Key]
        public Int32 EventID { get; private set; }
        [Required]
        public Int32 TimeStamp { get; private set; }
        public String Description { get; set; }

        public Event()
        {
            EventID = this.GetHashCode();
            TimeStamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
    }
}
