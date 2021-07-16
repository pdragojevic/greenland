using System;
using System.Collections.Generic;

namespace Greenland.API.Models
{
    public partial class Meeting
    {
        public int IdMeeting { get; set; }
        public int? Organizer { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public DateTime? Started { get; set; }
        public DateTime? Ended { get; set; }

        public virtual Employees OrganizerNavigation { get; set; }
    }
}
