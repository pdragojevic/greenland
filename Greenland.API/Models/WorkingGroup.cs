using System;
using System.Collections.Generic;

namespace Greenland.API.Models
{
    public partial class WorkingGroup
    {
        public WorkingGroup()
        {
            Team = new HashSet<Team>();
        }

        public int IdWorkingGroup { get; set; }
        public string WorkingGroupName { get; set; }
        public int? IdCoordinator { get; set; }

        public virtual Employees IdCoordinatorNavigation { get; set; }
        public virtual ICollection<Team> Team { get; set; }
    }
}
