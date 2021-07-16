using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Greenland.API.DB;
using Greenland.API.Models;
using Greenland.API.Business_Logic;
using Greenland.API.Exceptions;
using Greenland.API.Business_Logic.JWT;

namespace Greenland.API.Controllers
{
    //[Route("api/[controller]")]
    [Route("[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeesDB employeesDB;
        private readonly TeamDB teamDB;
        private readonly CompanyPositionDB companyPositionDB;
        private readonly ForgotPasswordDB forgotPasswordDB;
        private readonly WorkingGroupDB workingGroupDB;
        private ActivityFeedDB feedDB;

        public EmployeesController()
        {
            employeesDB = new EmployeesDB();
            teamDB = new TeamDB();
            companyPositionDB = new CompanyPositionDB();
            forgotPasswordDB = new ForgotPasswordDB();
            workingGroupDB = new WorkingGroupDB();
            feedDB = new ActivityFeedDB();
        }

        [HttpGet]
        [Route("logged")]
        [Authorize]
        public ActionResult GetLoggedEmployee()
        {
            return Ok(HttpContext.Items["Employee"]);
        }

        // GET: api/Employees
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Employees>>> GetEmployees()
        {
            return Ok(await employeesDB.GetAllEmployees());
        }

        [HttpGet]
        [Route("profile")]
        [Authorize]
        public async Task<ActionResult> GetEmployeeProfile()
        {
            Employees emp = (Employees)HttpContext.Items["Employee"];
            EmployeeProfile empProfile = new EmployeeProfile();
            empProfile.BirthDate = emp.BirthDate;
            empProfile.Email = emp.Email;
            empProfile.FirstName = emp.FirstName;
            empProfile.Gender = emp.Gender;
            empProfile.HireDate = emp.HireDate;
            empProfile.LastName = emp.LastName;
            empProfile.Position = companyPositionDB.GetPositionNameFromIdSync(emp.IdCompanyPosition);
            empProfile.Username = emp.Username;
            empProfile.IdEmployee = emp.IdEmployee;
            if (emp.IdCompanyPosition == 3) //if is team leader than return which team he leads
            {
                Team leadsTeam = await teamDB.GetTeamByTeamLeader(emp);
                empProfile.Team = leadsTeam.TeamName;
            }
            else if (emp.IdCompanyPosition == 2) //coordinator
            {
                WorkingGroup wb = await workingGroupDB.GetWorkingGroupByCoordinator(emp);
                empProfile.Team = wb.WorkingGroupName;
            }
            else
            {
                empProfile.Team = teamDB.GetTeamNameFromIdSync(emp.IdTeam);
            }
            return Ok(empProfile);
        }

        [HttpGet]
        [Route("username")]
        [Authorize]
        public async Task<ActionResult<Employees>> GetEmployees(string username)
        {
            Employees employee = await employeesDB.GetEmployeeByUsername(username);


            if (employee == null)
            {
                return NotFound();
            }
            return employee;
        }

        [HttpPut]
        [Route("updateProfile")]
        [Authorize]
        public async Task<IActionResult> changePassword(ChangePasswordModel newEmployee)
        {
            Employees employee = await employeesDB.GetEmployeeByUsername(newEmployee.Username);
            if (Util.hashSHA256(newEmployee.OldPassword).Equals(employee.Password))
            {
                employee.Password = Util.hashSHA256(newEmployee.NewPassword);
                try
                {
                    await employeesDB.UpdateEmployee(employee);
                    return Ok();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return StatusCode(500);
                }
            }
            else
            {
                //dodaj cacheanje nekad
                return Unauthorized("You entered wrong password");
            }
        }

        [HttpPut]
        [Route("prom/dev")]
        [Authorize]
        public async Task<ActionResult> PromoteDevelopers(string username)
        {
            Employees emp = await employeesDB.GetEmployeeByUsername(username);
            emp.IdCompanyPosition = 3; //postavi ih u leadere
            emp.IdTeam = null; //nemaju pridijeljen novi team
            await employeesDB.UpdateEmployee(emp);
            ActivityFeed feed = new ActivityFeed()
            {
                IdEmployee = 0,
                Message = emp.FirstName + " " + emp.LastName + " was promoted in team leader!",
                CreatedTime = DateTime.Now
            };
            feedDB.AddActivity(feed);
            return Ok();
        }

        [HttpPut]
        [Route("prom/leader")]
        [Authorize]
        public async Task<ActionResult> PromoteLeaders(string username)
        {
            Employees emp = await employeesDB.GetEmployeeByUsername(username);
            emp.IdCompanyPosition = 2; //sad idu coordinatore
            Team team = await teamDB.GetTeamByTeamLeader(emp); //get team with team leader
            if (team != null)
            {
                team.IdTeamLeader = null;
                await teamDB.UpdateTeam(team);
            }
            await employeesDB.UpdateEmployee(emp);
            ActivityFeed feed = new ActivityFeed()
            {
                IdEmployee = 0,
                Message = emp.FirstName + " " + emp.LastName + " was promoted in coordinator!",
                CreatedTime = DateTime.Now
            };
            feedDB.AddActivity(feed);
            return Ok();
        }

        [HttpGet]
        [Route("get/developers")]
        [Authorize]
        public async Task<ActionResult> GetDevelopersFromTeam(string team)
        {
            string teamName = team;
            if (team.Equals("default"))
            {
                Employees emp = ((Employees)HttpContext.Items["Employee"]);
                if (emp.IdCompanyPosition == 4)
                {
                    int? teamId = emp.IdTeam;
                    IEnumerable<Employees> employees = await employeesDB.GetAllDevelopersFromTeam(teamId);
                    return Ok(employees);
                }
                if (emp.IdCompanyPosition == 3)
                {
                    Team team1 = await new TeamDB().GetTeamByTeamLeader(emp);
                    return Ok(await employeesDB.GetAllDevelopersFromTeam(team1.TeamName));
                }
            }
            List<Employees> emps = await employeesDB.GetAllDevelopersFromTeam(team);
            return Ok(emps);
        }

        [HttpPost]
        [Route("add")]
        [Authorize]
        public async Task<ActionResult> AddEmployee(Employees emp)
        {
            emp.Password = Util.hashSHA256(emp.Password);
            await employeesDB.AddEmployee(emp);
            ActivityFeed feed = new ActivityFeed()
            {
                IdEmployee = 0,
                Message = emp.FirstName + " " + emp.LastName + " joined our company!",
                CreatedTime = DateTime.Now
            };
            feedDB.AddActivity(feed);
            return Ok(Newtonsoft.Json.JsonConvert.SerializeObject("Employee inserted!"));
        }

        [HttpGet]
        [Route("get/leader")]
        [Authorize]
        public async Task<ActionResult> GetTeamLeaderForTeam(string team)
        {
            return Ok(await employeesDB.GetTeamLeaderForTeam(team));
        }

        [HttpGet]
        [Route("get/teams")]
        [Authorize]
        public async Task<ActionResult> GetTeamsFromCoordinator(int coordinator)
        {
            return Ok(await teamDB.GetAllTeamsFromCoordinatorId(coordinator));
        }




    }
}
