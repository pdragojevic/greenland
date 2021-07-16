using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Greenland.API.Business_Logic.Iterations
{
    public class IterationsUtil
    {

        private IterationSettings settings;

        public IterationsUtil(IterationSettings options)
        {
            this.settings = options;
        }

        public List<Iteration> GetClosestIterations(Iteration currentIteration)
        {
            //we assume that iteration finished and we need to calculate new iterations
            DateTime today = DateTime.Today;
            Iteration newCurrentIteration = currentIteration;
            while (today > newCurrentIteration.end)
            {
                newCurrentIteration.start = newCurrentIteration.start.AddDays(14);
                newCurrentIteration.end = newCurrentIteration.end.AddDays(14);
                newCurrentIteration.num++;
            }
            List<Iteration> solution = new List<Iteration>();
            Iteration pointer = new Iteration(newCurrentIteration);
            pointer.num -= (settings.numberOfPastIterations);
            pointer.start = pointer.start.AddDays(-14 * (settings.numberOfPastIterations));
            pointer.end = pointer.end.AddDays(-14 * (settings.numberOfPastIterations));
            for (int i = 0; i < settings.numberOfPastIterations; i++)
            {
                solution.Add(new Iteration(pointer));
                pointer.start = pointer.start.AddDays(14);
                pointer.end = pointer.end.AddDays(14);
                pointer.num++;
            }
            pointer = new Iteration(newCurrentIteration);
            solution.Add(new Iteration(newCurrentIteration));
            for (int i = 0; i < settings.numberOfFutureIterations; i++)
            {
                pointer.start = pointer.start.AddDays(14);
                pointer.end = pointer.end.AddDays(14);
                pointer.num++;
                solution.Add(new Iteration(pointer));
            }
            return solution;
        }

        public Iteration GetClosestIteration()
        {
            DateTime today = DateTime.Today;
            while (true)
            {
                if (today.DayOfWeek == DayOfWeek.Monday)
                {
                    break;
                }
                today = today.AddDays(-1);
            }
            return new Iteration(today, today.AddDays(11), settings.numberOfPastIterations + 1);
        }








    }
}
