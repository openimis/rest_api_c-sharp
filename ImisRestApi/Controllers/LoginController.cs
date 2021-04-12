using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OpenImis.Modules;
using OpenImis.Modules.LoginModule.Models;

namespace ImisRestApi.Controllers
{
    //[ApiVersion("2")]
    [Authorize]
    [Route("api/")]
    //[ApiController]
    [EnableCors("AllowSpecificOrigin")]
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IImisModules _imisModules;

        public LoginController(IConfiguration configuration, IImisModules imisModules)
        {
            _configuration = configuration;
            _imisModules = imisModules;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public IActionResult Index([FromBody] LoginModel model)
        {
            var user = _imisModules.GetLoginModule().GetLoginLogic().FindUser(model.UserName, model.Password);
            if (user != null)
            {
                DateTime expirationDate = DateTime.Now.AddDays(double.Parse(_configuration["JwtExpireDays"]));

                List<Claim> claims = new List<Claim>()
                {
                    new Claim("UserId", user.UserID),
                    new Claim("UserUUID", user.UserUUID.ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(user.PrivateKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["JwtIssuer"],
                    audience: _configuration["JwtIssuer"],
                    claims: claims,
                    expires: expirationDate,
                    signingCredentials: creds);

                return Ok(new
                {
                    access_token = new JwtSecurityTokenHandler().WriteToken(token),
                    expires_on = expirationDate
                });
            }

            return Unauthorized();
        }
    }
}