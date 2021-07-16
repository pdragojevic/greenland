using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Greenland.API.DB;
using Greenland.API.Models;
using Task = Greenland.API.Models.Task;
using Greenland.API.Business_Logic;
using Greenland.API.Business_Logic.JWT;
using Newtonsoft.Json;

namespace Greenland.API.Controllers
{
    //[Route("api/[controller]")]
    [Route("[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TaskDB _context;
        private ActivityFeedDB feedDB;

        public TasksController()
        {
            _context = new TaskDB();
            feedDB = new ActivityFeedDB();
        }

        // GET: api/Tasks
        [HttpGet]
        [Authorize]
        [Route("get")]
        public async Task<ActionResult<IEnumerable<Task>>> GetTask(int team)
        {
            int? myTeam = team;
            if (string.IsNullOrEmpty(Request.QueryString.Value))
            {
                Employees emp = (Employees)HttpContext.Items["Employee"];
                if (emp.IdCompanyPosition == 1 || emp.IdCompanyPosition == 2)
                {
                    return StatusCode(500, "Coordinators and board cannot implicitly see tasks");
                }
                if (emp.IdCompanyPosition == 3)
                {
                    Team teamObj = await new TeamDB().GetTeamByTeamLeader(emp);
                    myTeam = teamObj.IdTeam;
                }
                else
                {
                    myTeam = emp.IdTeam;
                }
            }
            var tasks = await new TaskDB().GetTaskBYIDteam(myTeam);
            if (tasks == null) return NotFound();
            return new ActionResult<IEnumerable<Models.Task>>(tasks);
            //return await _context.Task.ToListAsync();
        }

        //ovo ni ne treba zasto to?
        // GET: api/Tasks/byDate?date=2020-12-07
        [HttpGet("ByDate")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Task>>> GetTask(string date)
        {
            DateTime myDate;
            if (!DateTime.TryParse(date, out myDate))
            {
                return UnprocessableEntity();
            }

            Employees emp = (Employees)HttpContext.Items["Employee"];
            var tasks = await new TaskDB().GetTaskForCreationDateTimeForTeamID(myDate, emp.IdTeam);
            if (tasks == null)
            {
                return NotFound();
            }
            return new ActionResult<IEnumerable<Models.Task>>(tasks);
        }

        // GET: api/Tasks/Between?start=2020-12-07&end=2020-12-10
        [HttpGet("Between")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Task>>> GetTask(string start, string end, int team)
        {
            int? myTeam = team;
            if (!Request.QueryString.Value.Contains("team")) //ako ne zapocinje sa team
            {
                Employees emp = (Employees)HttpContext.Items["Employee"];
                if (emp.IdCompanyPosition == 1 || emp.IdCompanyPosition == 2)
                {
                    return StatusCode(500, "Coordinators and board cannot implicitly see tasks");
                }
                if (emp.IdCompanyPosition == 3)
                {
                    Team teamObj = await new TeamDB().GetTeamByTeamLeader(emp);
                    myTeam = teamObj.IdTeam;
                }
                else
                {
                    myTeam = emp.IdTeam;
                }
            }
            DateTime startdate;
            if (!DateTime.TryParse(start, out startdate))
            {
                return UnprocessableEntity();
            }
            DateTime enddate;
            if (!DateTime.TryParse(end, out enddate))
            {
                return UnprocessableEntity();
            }

            var tasks = await _context.GetTaskBetweenCreationDateTimesForTeamID(startdate, enddate, myTeam);
            if (tasks == null)
            {
                return NotFound();
            }
            return new ActionResult<IEnumerable<Models.Task>>(tasks);
        }

        // POST: api/Tasks
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Task>> PostTask(Task task)
        {
            task.CreationDate = DateTime.Today;
            Employees emp = (Employees)HttpContext.Items["Employee"];
            if (emp.IdCompanyPosition == 1 || emp.IdCompanyPosition == 2) //uprava ili koordinator
            {
                return StatusCode(500, "You are not allowed to insert task!");
            }
            int? idTeam = emp.IdTeam;
            if (emp.IdCompanyPosition == 3) //ako je team leader onda insertaj teamu kojem je leader
            {
                Team team = await new TeamDB().GetTeamByTeamLeader(emp);
                idTeam = team.IdTeam;
            }
            Task task1 = await _context.AddTask(task, emp.IdEmployee, idTeam);
            ActivityFeed feed = new ActivityFeed()
            {
                IdEmployee = 3,
                Message = emp.FirstName + " " + emp.LastName + " created task " + task.Summary + "!",
                CreatedTime = DateTime.Now
            };
            feedDB.AddActivity(feed);
            return StatusCode(201, task1);
        }

        //DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Boolean>> DeleteTask(int id)
        {
            Boolean succes = await _context.DeleteTaskByID(id);
            return succes == false ? StatusCode(400, "Task not found") : (ActionResult<bool>)StatusCode(204, "Deleted");
        }


        /*private bool TaskExists(int id) ovo ispravi
        {
            return _context.Task.Any(e => e.IdTask == id);
        } */

        // GET: api/Tasks/UpdateStatus?id=111&newStatus=proba
        [HttpPut("UpdateStatus")]
        [Authorize]
        public async Task<ActionResult<Boolean>> UpdateStatusTask(int id, string newStatus)
        {
            Boolean succes = await _context.updateStatusByIDTask(id, newStatus);
            if (succes == false) return StatusCode(400, "Task not found");
            return StatusCode(200, JsonConvert.SerializeObject("Task Updated"));

        }

        // GET: api/Tasks/UpdateTask?id=111&key=priority&value=3
        //key => imena stupaca malim slovima
        [HttpPut("UpdateTask")]
        [Authorize]
        public async Task<ActionResult<Task>> UpdateTask(int id, string key, string value)
        {
            try
            {
                Task result = await _context.updateTaskByKey(id, key, value);
                ActivityFeed feed = new ActivityFeed()
                {
                    IdEmployee = 4,
                    Message = "Value for task property " + key + " updated to " + value + "!",
                    CreatedTime = DateTime.Now
                };
                feedDB.AddActivity(feed);
                return StatusCode(200, result);
            }
            catch (ArgumentException e)
            {
                return StatusCode(400, e.Message);
            }

        }

        [HttpPut("Update")]
        [Authorize]
        public async Task<ActionResult<Task>> Update(Task task)
        {
            Models.Task result = await _context.update(task);
            ActivityFeed feed = new ActivityFeed()
            {
                IdEmployee = 4,
                Message = "Task updated!",
                CreatedTime = DateTime.Now
            };
            feedDB.AddActivity(feed);
            if (result == null) return StatusCode(400, "Task not updated");
            return StatusCode(200, result);
        }

        [HttpPut("update/task/employee")]
        [Authorize]
        public async Task<ActionResult> Update(TaskEmployee taskEmployee)
        {
            Models.TaskEmployee result = await _context.updateTaskEmployee(taskEmployee);
            if (result == null) return StatusCode(400, "No such task");
            return StatusCode(200);
        }

        [HttpGet]
        [Authorize]
        [Route("get/employee/task")]
        public async Task<Employees> GetEmployeeForTask(int taskId)
        {
            return await _context.GetEmployeeForTask(taskId);
        }

        [HttpGet]
        [Authorize]
        [Route("get/taskemployee")]
        public TaskEmployee GetTaskEmployee(int taskId)
        {
            return _context.GetTaskEmployee(taskId);
        }
    }
}
