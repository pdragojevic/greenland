using Greenland.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Greenland.API.DB
{
    public class ActivityFeedDB
    {
        private greenlandDBContext dbContext;

        public ActivityFeedDB()
        {
            this.dbContext = new greenlandDBContext();
        }

        public async Task<IEnumerable<string>> GetMessageByIdEmployee(Employees employee)
        {
            return await dbContext.ActivityFeed.Where(d => d.IdEmployee <= employee.IdCompanyPosition).Select(o => o.Message).ToListAsync();
        }

        public async Task<ActivityFeed> GetActivityFeedByIdEmployee(int idEmployee)
        {
            ActivityFeed ac = await dbContext.ActivityFeed.FirstOrDefaultAsync(ac => ac.IdEmployee.Equals(idEmployee));
            return ac;
        }


        // da vrati odreden broj poruka

        //kako upisat u bazu kada ima identity postavljen
        //ovo sto se autommatski stvara id u bazi to se zove identity
        public ActivityFeed AddActivity(ActivityFeed feed)
        {
            dbContext.ActivityFeed.Add(feed);
            dbContext.SaveChanges();
            return feed;
        }

    }
}
