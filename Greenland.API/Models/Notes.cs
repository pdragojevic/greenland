using System;
using System.Collections.Generic;

namespace Greenland.API.Models
{
    public partial class Notes
    {
        public int IdNote { get; set; }
        public int IdEmployee { get; set; }
        public string Note { get; set; }
        public DateTime CreatedTime { get; set; }

        public virtual Employees IdEmployeeNavigation { get; set; }
    }
}
