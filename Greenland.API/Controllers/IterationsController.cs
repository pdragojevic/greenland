using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greenland.API.Business_Logic;
using Greenland.API.Business_Logic.Iterations;
using Greenland.API.Business_Logic.JWT;
using Greenland.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Greenland.API.Controllers
{
    //[Route("api/[controller]")]
    [Route("[controller]")]
    [ApiController]
    public class IterationsController : ControllerBase
    {

        private IterationSettings settings;


        public IterationsController(IOptions<IterationSettings> options)
        {
            this.settings = options.Value;
        }


        [HttpPost]
        [Route("get")]
        [Authorize]
        public ActionResult GetIterations(Iteration currentIteration)
        {
            return Ok(new IterationsUtil(settings).GetClosestIterations(currentIteration));
        }

        [HttpGet]
        [Route("get/current")]
        [Authorize]
        public ActionResult GetCurrent()
        {
            return Ok(new IterationsUtil(settings).GetClosestIteration());
        }
    }
}
