using System;
using System.Collections.Generic;

namespace Greenland.API.Models
{
    public partial class Team
    {
        public Team()
        {
            Employees = new HashSet<Employees>();
            TaskTeam = new HashSet<TaskTeam>();
        }

        public int IdTeam { get; set; }
        public string TeamName { get; set; }
        public int? IdTeamLeader { get; set; }
        public int? IdWorkingGroup { get; set; }

        public virtual Employees IdTeamLeaderNavigation { get; set; }
        public virtual WorkingGroup IdWorkingGroupNavigation { get; set; }
        public virtual ICollection<Employees> Employees { get; set; }
        public virtual ICollection<TaskTeam> TaskTeam { get; set; }
    }
}
