using Greenland.API.Models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Greenland.API.DB
{
    public class EmployeesDB
    {

        private greenlandDBContext dbContext;

        public EmployeesDB()
        {
            this.dbContext = new greenlandDBContext();
        }
        public async Task<Employees> GetEmployeeByUsername(string username)
        {
            Employees emp = await dbContext.Employees.FirstOrDefaultAsync(emp => emp.Username.Equals(username));
            return emp;

        }

        public async Task<IEnumerable<int>> GetEmployeesBYIDteam(int team)
        {

            return await dbContext.Employees.Where(employees => employees.IdTeam == team).Select(o => o.IdEmployee).ToListAsync();
        }


        public async Task<Employees> GetEmployeeById(int id)
        {

            return await dbContext.Employees.FirstOrDefaultAsync(emp => emp.IdEmployee.Equals(id));
        }
        public async Task<IEnumerable<Employees>> GetAllEmployees()
        {

            return await dbContext.Employees.ToListAsync();
        }

        public async System.Threading.Tasks.Task UpdateEmployee(Employees employee)
        {
            dbContext.Employees.Update(employee);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Employees> GetEmployeeByMail(string mail)
        {

            return await dbContext.Employees.FirstOrDefaultAsync(emp => emp.Email.Equals(mail));
        }

        public Employees GetEmployeeByIdSync(int idEmployee)
        {

            return dbContext.Employees.FirstOrDefault(emp => emp.IdEmployee.Equals(idEmployee));
        }

        public Employees GetEmployeeByMailSync(string mail)
        {

            return dbContext.Employees.FirstOrDefault(emp => emp.Email.Equals(mail));
        }

        public async Task<List<Employees>> GetAllDevelopersFromTeam(string teamName)
        {
            return await dbContext.Employees.Where(emp => emp.IdCompanyPosition == 4 && emp.IdTeam == dbContext.Team.FirstOrDefault(team1 => team1.TeamName.Equals(teamName)).IdTeam).ToListAsync();
        }

        public async Task<List<Employees>> GetAllDevelopersFromTeam(int? idTeam)
        {
            return await dbContext.Employees.Where(emp => emp.IdCompanyPosition == 4 && emp.IdTeam == idTeam).ToListAsync();
        }

        public async System.Threading.Tasks.Task AddEmployee(Employees emp)
        {
            dbContext.Employees.Add(emp);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Employees> GetTeamLeaderForTeam(string teamName)
        {
            return await dbContext.Employees.FirstOrDefaultAsync(emp => emp.IdEmployee == dbContext.Team.FirstOrDefault(team => team.TeamName.Equals(teamName)).IdTeamLeader);
        }

        public async Task<IEnumerable<Employees>> GetEmpleyeesDependsWhoAsk(Employees emp)
        {
            int? empPosition = emp.IdCompanyPosition;

            if (empPosition == 4)
            {
                int? teamId = emp.IdTeam;
                IEnumerable<Employees> employees = await dbContext.Employees.Where(e => e.IdTeam == teamId).ToListAsync();
                return employees;
            }
            if (empPosition == 3)
            {
                Team team = await new TeamDB().GetTeamByTeamLeader(emp);
                return await GetAllDevelopersFromTeam(team.TeamName);
            }
            if (empPosition == 2)
            {
                int? idWorkinGroup = dbContext.WorkingGroup.Where(w => w.IdCoordinator == emp.IdEmployee).FirstOrDefault().IdWorkingGroup;
                List<int?> idLeaders = dbContext.Team.Where(t => t.IdWorkingGroup == idWorkinGroup).Select(t => t.IdTeamLeader).ToList();
                IEnumerable<Employees> leaders = await dbContext.Employees.Where(e => idLeaders.Contains(e.IdEmployee)).ToListAsync();
                return leaders;
            }
            if (empPosition == 1)
            {
                IEnumerable<Employees> cordinators = await dbContext.Employees.Where(e => e.IdCompanyPosition == 2).ToListAsync();
                return cordinators;
            }

            return null;
        }
    }
}
