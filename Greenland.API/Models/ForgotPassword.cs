using System;
using System.Collections.Generic;

namespace Greenland.API.Models
{
    public partial class ForgotPassword
    {
        public string Guid { get; set; }
        public DateTime TimeCreated { get; set; }
        public int IdEmployee { get; set; }

        public virtual Employees IdEmployeeNavigation { get; set; }
    }
}
