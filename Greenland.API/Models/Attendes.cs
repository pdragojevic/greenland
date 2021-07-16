using System;
using System.Collections.Generic;

namespace Greenland.API.Models
{
    public partial class Attendes
    {
        public int IdMeeting { get; set; }
        public int Attendee { get; set; }

        public virtual Employees AttendeeNavigation { get; set; }
    }
}
