using System;
using System.Collections.Generic;

namespace Greenland.API.Models
{
    public partial class TaskEmployee
    {
        public int IdTask { get; set; }
        public int IdEmployee { get; set; }

        public virtual Employees IdEmployeeNavigation { get; set; }
    }
}
