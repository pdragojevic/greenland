using Greenland.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Greenland.API.DB
{
    public class WorkingGroupDB
    {
        private greenlandDBContext dbContext;

        public WorkingGroupDB()
        {
            dbContext = new greenlandDBContext();
        }

        public async Task<WorkingGroup> GetWorkingGroupByCoordinator(Employees emp)
        {
            return await dbContext.WorkingGroup.FirstOrDefaultAsync<WorkingGroup>(wb => wb.IdCoordinator == emp.IdEmployee);
        }

        public async Task<List<WorkingGroup>> GetAllWorkingGroupsAsync()
        {
            return await dbContext.WorkingGroup.ToListAsync<WorkingGroup>();
        }

        public async Task<List<Employees>> GetAllCoordinators()
        {
            return await dbContext.Employees.Where(emp => dbContext.WorkingGroup.Select(wb => wb.IdCoordinator).Contains(emp.IdEmployee)).ToListAsync();
        }

        public async System.Threading.Tasks.Task AddWorkingGroup(WorkingGroup wb)
        {
            dbContext.WorkingGroup.Add(wb);
            await dbContext.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task UpdateWorkingGroup(WorkingGroup wb)
        {
            dbContext.WorkingGroup.Update(wb);
            await dbContext.SaveChangesAsync();
        }
    }

}