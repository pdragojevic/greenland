using System;
using System.Collections.Generic;

namespace Greenland.API.Models
{
    public partial class TaskTeam
    {
        public int IdTask { get; set; }
        public int IdTeam { get; set; }

        public TaskTeam(int IdTask, int IdTeam)
        {
            this.IdTask = IdTask;
            this.IdTeam = IdTeam;
        }

        public virtual Team IdTeamNavigation { get; set; }
    }
}
