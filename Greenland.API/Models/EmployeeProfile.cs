using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Greenland.API.Models
{
    public class EmployeeProfile
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? HireDate { get; set; }
        public string Gender { get; set; }
        public string Position { get; set; }
        public string Team { get; set; }
        public string Email { get; set; }
        public int IdEmployee { get; set; }
    }
}
