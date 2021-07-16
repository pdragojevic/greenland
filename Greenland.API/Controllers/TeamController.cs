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
namespace Greenland.API.Controllers
{
    //[Route("api/[controller]")]
    [Route("[controller]")]
    //localtesting
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly DB.greenlandDBContext _context; //ovo preseli u teamDB

        private readonly TeamDB teamDB;
        private readonly WorkingGroupDB workingGroupDB;
        private ActivityFeedDB feedDB;



        public TeamController()
        {
            _context = new DB.greenlandDBContext();
            teamDB = new TeamDB();
            workingGroupDB = new WorkingGroupDB();
            feedDB = new ActivityFeedDB();
        }


        //token za test Employee
        //eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjUxIiwibmJmIjoxNjA5MTczOTY4LCJleHAiOjE2MDk3Nzg3NjgsImlhdCI6MTYwOTE3Mzk2OH0.MERRoR5UsqLxfbIdX6N00bKs_MuIDm2zhsLrCiEWJPQ

        //token za test Koordinatora
        //eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjEzIiwibmJmIjoxNjA5MTc0MzM3LCJleHAiOjE2MDk3NzkxMzcsImlhdCI6MTYwOTE3NDMzN30.yTE8QltGmoSrzZWAJ0XGaoDgFrVlxbfdeIo4oqR5YUs

        //token za Upravu
        //eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjExIiwibmJmIjoxNjA5MTc0NTE2LCJleHAiOjE2MDk3NzkzMTYsImlhdCI6MTYwOTE3NDUxNn0.peA2FnYurn0Osbv5BBuJrLHdVuO9iof2UlQFp9eF_80
        // api/Team/get
        [Route("get")]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Employees>>> GetEmployees()
        {
            Employees emp = (Employees)HttpContext.Items["Employee"];
            IEnumerable<Employees> employess = await new EmployeesDB().GetEmpleyeesDependsWhoAsk(emp);

            if (employess == null) return NotFound("Niente good");

            return new ActionResult<IEnumerable<Employees>>(employess);
        }

        [Route("get/groups")]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<WorkingGroup>>> GetAllWorkingGroups()
        {
            return await workingGroupDB.GetAllWorkingGroupsAsync();
        }

        [Route("get/coordinators")]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Employees>>> GetAllCoordinators()
        {
            return await workingGroupDB.GetAllCoordinators();
        }

        [Route("get/leaders")]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Employees>>> GetAllLeaders()
        {
            return await teamDB.GetAllLeaders();
        }


        [Route("get/teams")]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Team>>> GetAllTeams()
        {
            return await teamDB.GetAllTeams();
        }

        [Route("add/wgroup")]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddWGroup(WorkingGroup wb)
        {
            await workingGroupDB.AddWorkingGroup(wb);
            ActivityFeed feed = new ActivityFeed()
            {
                IdEmployee = 0,
                Message = "We have new working group in company:  " + wb.WorkingGroupName,
                CreatedTime = DateTime.Now
            };
            feedDB.AddActivity(feed);
            return StatusCode(201, "WorkingGroup created!");
        }

        [Route("add/team")]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddTeam(Team team)
        {
            try
            {
                await teamDB.AddTeam(team);
                ActivityFeed feed = new ActivityFeed()
                {
                    IdEmployee = 0,
                    Message = "We have new team in company: " + team.TeamName + "!",
                    CreatedTime = DateTime.Now
                };
                feedDB.AddActivity(feed);
                return StatusCode(201, "Team created!");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [Route("update/wgroup")]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> UpdateWgroup(WorkingGroup wb)
        {
            await workingGroupDB.UpdateWorkingGroup(wb);
            return StatusCode(201, "WorkingGroup created!");
        }

        [Route("update/team")]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> UpdateTeam(Team team)
        {
            await teamDB.UpdateTeam(team);
            return StatusCode(201, "Team created!");
        }

    }
}
