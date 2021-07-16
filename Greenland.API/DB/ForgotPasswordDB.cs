using Greenland.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Greenland.API.DB
{
    public class ForgotPasswordDB
    {
        private greenlandDBContext dbContext;

        public ForgotPasswordDB()
        {
            this.dbContext = new greenlandDBContext();
        }

        public async System.Threading.Tasks.Task Insert(ForgotPassword forgotPasswordRow)
        {
            dbContext.ForgotPassword.Add(forgotPasswordRow);
            await dbContext.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task<ForgotPassword> GetForgotPasswordRowByGuid(string guid)
        {

            return await dbContext.ForgotPassword.FirstOrDefaultAsync(fp => fp.Guid.Equals(guid));
        }
    }
}
