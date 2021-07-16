using System;
using System.Collections.Generic;

namespace Greenland.API.Models
{
    public partial class CompanyPosition
    {
        public CompanyPosition()
        {
            Employees = new HashSet<Employees>();
        }

        public int IdCompanyPosition { get; set; }
        public string NameCompanyPosition { get; set; }

        public virtual ICollection<Employees> Employees { get; set; }
    }
}
