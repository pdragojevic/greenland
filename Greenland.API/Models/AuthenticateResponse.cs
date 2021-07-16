using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Greenland.API.Models
{
    public class AuthenticateResponse
    {
        public string Token { get; set; }

        public AuthenticateResponse(string token)
        {
            Token = token;
        }
    }
}
