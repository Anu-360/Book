using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Book_Coding_Challenge.Repository;
using Book_Coding_Challenge.Models;

namespace Book_Coding_Challenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUser _user;

        public AuthController(IConfiguration config, IUser user)
        {
            _config = config;
            _user = user;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Auth([FromBody] UserLogin login)
        {
            IActionResult response = Unauthorized();
            var dbUser = await _user.ValidateUser(login.UserMail, login.Password);

            if (dbUser != null)
            {
                var issuer = _config["Jwt:Issuer"];
                var audience = _config["Jwt:Audience"];
                var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
                var signingCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature);

                var subject = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, dbUser.UserId.ToString()),
                    new Claim(ClaimTypes.Name, dbUser.UserName)
                };

                if (dbUser.User_Type != null)
                {
                    subject.Add(new Claim(ClaimTypes.Role, dbUser.User_Type.ToString()));
                }

                var expires = DateTime.UtcNow.AddDays(10);
                var tokenDesc = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(subject),
                    Expires = expires,
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = signingCredentials
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDesc);
                var jwtToken = tokenHandler.WriteToken(token);

                return Ok(new { Token = jwtToken });
            }

            return response;
        }
    }
}

