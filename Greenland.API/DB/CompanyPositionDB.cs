using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Greenland.API.DB
{
    public class CompanyPositionDB
    {
        private greenlandDBContext dbContext;

        public CompanyPositionDB()
        {
            this.dbContext = new greenlandDBContext();
        }

        public string GetPositionNameFromIdSync(int? id)
        {
            return dbContext.CompanyPosition.FirstOrDefault(cp => cp.IdCompanyPosition.Equals(id)).NameCompanyPosition;
        }
    }
}
