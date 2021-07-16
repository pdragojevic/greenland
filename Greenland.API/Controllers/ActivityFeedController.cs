using Greenland.API.Business_Logic.JWT;
using Greenland.API.DB;
using Greenland.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Greenland.API.Controllers
{
    //[Route("api/[controller]")]
    [Route("[controller]")]
    [ApiController]
    public class ActivityFeedController : ControllerBase
    {

        private readonly ActivityFeedDB activityfeedDB;

        public ActivityFeedController()
        {
            activityfeedDB = new ActivityFeedDB();
        }
        [HttpGet]
        [Authorize]
        [Route("get")]

        //samojedna metoda  dohvati iz baze koja vraca listu stringova 
        public async Task<ActionResult<IEnumerable<string>>> GetActivityFeed()
        {
            try
            {
                return Ok(await activityfeedDB.GetMessageByIdEmployee((Employees)HttpContext.Items["Employee"]));
            }
            catch (NullReferenceException)
            {
                return StatusCode(500, "Cannot obtain feed!");
            }
        }
    }
}
