using System;
using System.Collections.Generic;

namespace Greenland.API.Models
{
    public partial class Task
    {
        public int IdTask { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public int? Priority { get; set; }
        public DateTime? Deadline { get; set; }
        public DateTime? CreationDate { get; set; }
        public string Status { get; set; }
    }
}
