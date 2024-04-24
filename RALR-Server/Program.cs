using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RALR_Server
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost, Route("login")]
        public IActionResult Login([FromBody] LoginModel user)
        {
            if (user == null)
            {
                return BadRequest("Invalid client request");
            }

            // Should be replaced with logic to validate the user credentials
            // and retrieve the password hash and salt from your MSSQL database.
            if (user.Username == "user" && user.Password == "password")
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var tokeOptions = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: new List<Claim>{new Claim("Permissions", "7")},
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new { Token = tokenString });
            }
            else
            {
                return Unauthorized();
            }
        }

        [Authorize]
        [HttpGet, Route("hosts")]
        public IActionResult GetHosts()
        {
            //Send applicable request
            return Ok("You are authorized!");
        }

        [Authorize]
        [HttpGet("host/{id}/appsList")]
        public IActionResult GetAppsList(int id)
        {
            //Send applicable request
            return Ok("You are authorized!");
        }

        [Authorize]
        [HttpGet("host/{id}/logs")]
        public IActionResult GetLogs(int id)
        {
            //Send applicable request
            return Ok("You are authorized!");
        }

        [Authorize]
        [HttpPut("host/{id}/allow/{base64Path}")]
        public IActionResult AllowApp(int id, string base64Path)
        {
            //Send applicable request
            return Ok("You are authorized!");
        }

        [Authorize]
        [HttpPut("host/{id}/deny/{base64Path}")]
        public IActionResult DenyApp(int id, string base64Path)
        {
            //Send applicable request
            return Ok("You are authorized!");
        }

        [Authorize]
        [HttpDelete("host/{id}/deletePathRule/{base64Path}")]
        public IActionResult DeletePathRule(int id, string base64Path)
        {
            //Send applicable request
            return Ok("You are authorized!");
        }

    }
}
