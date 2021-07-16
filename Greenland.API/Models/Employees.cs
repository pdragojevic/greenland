using System;
using System.Collections.Generic;

namespace Greenland.API.Models
{
    public partial class Employees
    {
        public Employees()
        {
            ForgotPassword = new HashSet<ForgotPassword>();
            TaskEmployee = new HashSet<TaskEmployee>();
            Team = new HashSet<Team>();
            WorkingGroup = new HashSet<WorkingGroup>();
        }

        public int IdEmployee { get; set; }
        public int? IdCompanyPosition { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }
        public DateTime? HireDate { get; set; }
        public int? IdTeam { get; set; }
        public string Email { get; set; }

        public virtual CompanyPosition IdCompanyPositionNavigation { get; set; }
        public virtual Team IdTeamNavigation { get; set; }
        public virtual ICollection<ForgotPassword> ForgotPassword { get; set; }
        public virtual ICollection<TaskEmployee> TaskEmployee { get; set; }
        public virtual ICollection<Team> Team { get; set; }
        public virtual ICollection<WorkingGroup> WorkingGroup { get; set; }
    }
}
