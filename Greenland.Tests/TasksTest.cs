using Greenland.API.Business_Logic;
using Greenland.API.DB;
using Greenland.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace Greenland.Tests
{
    public class TasksTest
    {
        [Fact]
        public void TestGetTask()
        {
            Task task = new Task();
            task.IdTask = 111;

            var taskContext = new TaskDB();

            var result = taskContext.GetTask(111);

            Assert.Equal(task, result.Result);
        }
        [Fact]
        public void TestGetTaskById()
        {
            var taskContext = new TaskDB();

            var result = taskContext.GetTaskByIdEmployee(49);

            var res = result.Result;
            Task task = result.Result.ToList().ElementAt(0);

            Assert.Equal(222, task.IdTask);

        }
        [Fact]
        public void TestGetTaskBYIDteam()
        {
            var taskContext = new TaskDB();

            var result = taskContext.GetTaskBYIDteam(5);

            var res = result.GetAwaiter().GetResult();
            Task task1 = res.ElementAt(0);
            Task task2 = res.ElementAt(1);

            Assert.Equal(111, task1.IdTask);
            Assert.Equal(222, task2.IdTask);
        }




        [Fact]
        public void TestGetTasksForDates()
        {
            var taskContext = new TaskDB();

            var result = taskContext.GetTaskBetweenCreationDateTimes(DateTime.Parse("2020-10-20"), DateTime.Parse("2020-11-30"));

            var res = result.Result;
            Task task1 = result.Result.ToList().ElementAt(0);
            Task task2 = result.Result.ToList().ElementAt(1);

            Assert.Equal(111, task1.IdTask);
            Assert.Equal(222, task2.IdTask);
        }

        [Fact]
        public void TestGetTasksForDatesAndTeam()
        {
            var taskContext = new TaskDB();

            var result = taskContext.GetTaskBetweenCreationDateTimesForTeamID(DateTime.Parse("2020-09-30"), DateTime.Parse("2021-10-31"), 5);

            var res = result.Result;
            Task task1 = result.Result.ToList().ElementAt(0);

            Assert.Equal(111, task1.IdTask);
        }

        [Fact]
        public void TestGetTasksForSpecificDate()
        {
            var taskContext = new TaskDB();

            var result = taskContext.GetTaskByCreationDateTime(DateTime.Parse("2020-12-30"));

            var res = result.Result;
            Task task1 = result.Result.ToList().ElementAt(0);

            Assert.Equal(334, task1.IdTask);
        }

        [Fact]
        public async void TestUpdateStatusByIDTask()
        {
            var taskContext = new TaskDB();
            var result = taskContext.GetTask(111).Result;
            string oldStatus = result.Status;
            await taskContext.updateStatusByIDTask(result.IdTask, "NEW STATUS");
            Assert.Equal("NEW STATUS", taskContext.GetTask(111).Result.Status);
            await taskContext.updateStatusByIDTask(result.IdTask, oldStatus);
            Assert.Equal(oldStatus, taskContext.GetTask(111).Result.Status);
        }



    }

}
