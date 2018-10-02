using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImisRestApi.Data;
using ImisRestApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ImisRestApi.Controllers
{
    public class LoginController : Controller
    {
        private IConfiguration Configuration;

        public LoginController(IConfiguration configuration)
        {
            Configuration = configuration;
           
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Index([FromBody]LoginModel model)
        {
            IActionResult response = Unauthorized();

            Repository repo = new Repository(Configuration);

            var user = repo.FindUser(model.UserName, model.Password);

            if (user != null)
            {
                var jwtToken = JwtTokenBuilder(user);
                response = Ok(new { access_token = jwtToken,expires_on = DateTime.Now.AddDays(5)});
            }
            return response;
        }

        private string JwtTokenBuilder(UserData user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var JwtToken = new JwtSecurityToken(issuer:Configuration["JWT:issuer"],audience:Configuration["JWT:audience"],signingCredentials:credentials,expires:DateTime.Now.AddDays(5))
            {

            };
 
            return new JwtSecurityTokenHandler().WriteToken(JwtToken);
        }
    }
}