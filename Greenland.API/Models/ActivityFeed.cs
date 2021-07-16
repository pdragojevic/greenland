using System;
using System.Collections.Generic;

namespace Greenland.API.Models
{
    public partial class ActivityFeed
    {
        public int IdFeed { get; set; }
        public int? IdEmployee { get; set; }
        public string Message { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}
