using Greenland.API.DB;
using Greenland.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Greenland.API.Business_Logic.JWT
{
    public interface IJwtService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest request);
    }

    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            using (DB.greenlandDBContext dBContext = new DB.greenlandDBContext())
            {
                string hashedPassword = Util.hashSHA256(model.Password);
                var employee = await dBContext.Employees.FirstOrDefaultAsync(x => x.Username == model.Username && x.Password == hashedPassword);
                if (employee == null) return null;
                // authentication successful so generate jwt token
                var token = BuildToken(employee);
                return new AuthenticateResponse(token);
            }
        }
        public string BuildToken(Employees emp)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDes = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", emp.IdEmployee.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDes);
            return tokenHandler.WriteToken(token);
        }
    }
}
