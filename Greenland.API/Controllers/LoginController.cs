using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Greenland.API.Business_Logic;
using Greenland.API.Business_Logic.JWT;
using Greenland.API.DB;
using Greenland.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic.CompilerServices;

namespace Greenland.API.Controllers
{
    /**
     * Controller that handles login
     */
    //[Route("api")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IJwtService _jwtService;

        public LoginController(IJwtService service)
        {
            this._jwtService = service;
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(AuthenticateRequest request)
        {
            using (var context = new greenlandDBContext())
            {
                /*Employees employeeDB = await context.Employees.FirstOrDefaultAsync(employee => employee.Username.Equals(employeeWeb.Username));
                if (employeeDB == null)
                {
                    return Unauthorized("Wrong username or password");
                }*/
                var response = await _jwtService.Authenticate(request);

                if (response == null)
                    return Unauthorized(new { message = "Username or password is incorrect" });

                return Ok(response);
            }
        }




    }
}
