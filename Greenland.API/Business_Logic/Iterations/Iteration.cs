using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Greenland.API.Business_Logic
{
    public class Iteration
    {
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public int num { get; set; }

        public Iteration() { }

        public Iteration(Iteration itr)
        {
            this.start = itr.start;
            this.end = itr.end;
            this.num = itr.num;
        }

        public Iteration(DateTime start, DateTime end, int num)
        {
            this.end = end;
            this.start = start;
            this.num = num;
        }
    }
}
