using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Net;
using System.Net.Http;
using System.Web.Http;
using ImisRestApi.Security;

using ImisRestApi.Models.RequestModels;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;

using ImisRestApi.Models.Entities;
using ImisRestApi.Models.Interfaces;


namespace ImisRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMISContext _imisContext;
        private readonly IIMISRepository _imisRepository;


        public LoginController(IConfiguration configuration, IMISContext imisContext, IIMISRepository imisRepository)
        {
            _configuration = configuration;
            _imisContext = imisContext;
            _imisRepository = imisRepository;
        }

        /// <summary>
        /// This function creates JWT token for the user identified with the user's credentials 
        /// </summary>
        /// <param name="request">
        ///     The username and password as JSON
        /// </param>
        /// <returns>
        ///     200 {"token": "[JWT_ACCESS_TOKEN]", "expires": "[Token_expiration_DateTime]" } 
        ///     400 {"error": "The request body is invalid"} 
        ///     401 No body
        /// </returns>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] LoginRequestModel request)
        {

            if (request.Username==null || request.Password==null)
            {
                return BadRequest(new { error = "The request body is invalid"});
            }

            IUserRepository userRepository = _imisRepository.getUserRepository();

            TblUsers user = userRepository.GetByUsernameAndPassword(request.Username, request.Password);
             
            if (user!=null)
            {
                DateTime expirationDate = DateTime.Now.AddDays(double.Parse(_configuration["JwtExpireDays"]));

                IEnumerable<Claim> claims = new[]
                {
                    new Claim(ClaimTypes.Name, request.Username)
                };

                // save user roles
                var roles = user.getRolesStringArray();

                foreach (var role in roles)
                {
                    claims = claims.Append(new Claim(ClaimTypes.Role, role));
                }

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
                    token = new JwtSecurityTokenHandler().WriteToken(token), 
                    expires = expirationDate
                });
            }

            return Unauthorized();
        }
    }
}