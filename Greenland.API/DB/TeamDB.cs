using Greenland.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Greenland.API.DB
{
    public class TeamDB
    {
        private greenlandDBContext dbContext;

        public TeamDB()
        {
            this.dbContext = new greenlandDBContext();
        }

        public string GetTeamNameFromIdSync(int? id)
        {
            return id == null ? "" : dbContext.Team.FirstOrDefault(team => team.IdTeam.Equals(id)).TeamName;
        }

        public async Task<Team> GetTeamByTeamLeader(Employees emp)
        {
            return await dbContext.Team.FirstOrDefaultAsync(team => team.IdTeamLeader == emp.IdEmployee);
        }

        public async Task<List<Employees>> GetAllLeaders()
        {
            return await dbContext.Employees.Where(emp => emp.IdCompanyPosition == 3).ToListAsync();
        }

        public async Task<List<Team>> GetAllTeams()
        {
            return await dbContext.Team.ToListAsync();
        }

        public async System.Threading.Tasks.Task UpdateTeam(Team team)
        {
            dbContext.Team.Update(team);
            await dbContext.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task AddTeam(Team team)
        {
            dbContext.Team.Add(team);
            await dbContext.SaveChangesAsync();
        }

        public async Task<List<Team>> GetAllTeamsFromCoordinatorId(int idCoordinator)
        {
            return await dbContext.Team.Where(team => team.IdWorkingGroup == dbContext.WorkingGroup.FirstOrDefault(wb => wb.IdCoordinator == idCoordinator).IdWorkingGroup).ToListAsync();
        }

    }
}

