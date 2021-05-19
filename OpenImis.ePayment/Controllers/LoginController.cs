using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using OpenImis.ePayment.Data;
using OpenImis.ePayment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace OpenImis.ePayment.Controllers
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

            ImisValidate repo = new ImisValidate(Configuration);

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
            double TokenValidDays = 0;
            try
            {
                TokenValidDays = Convert.ToDouble(Configuration["JWT:validdays"]);
            }
            catch (Exception)
            {
                
            }

            var claimsArrayLength = user.Rights.Count + 1;
            var claims = new List<Claim>
            {
               new Claim("UserId", user.UserID),
               
            };

            foreach(var right in user.Rights) {
                claims.Add(new Claim(ClaimTypes.Role, right));
            }

            var JwtToken = new JwtSecurityToken(
                issuer:Configuration["JWT:issuer"],
                audience:Configuration["JWT:audience"],
                claims: claims,
                signingCredentials:credentials,expires:DateTime.Now.AddDays(TokenValidDays)
                )
            {

            };
 
            return new JwtSecurityTokenHandler().WriteToken(JwtToken);
        }
    }
}