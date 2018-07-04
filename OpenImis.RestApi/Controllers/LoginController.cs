using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Net;
using System.Net.Http;
using System.Web.Http;
using OpenImis.RestApi.Security;

using OpenImis.RestApi.Models.HTTPModels;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;

using OpenImis.RestApi.Models.Entities;
using OpenImis.RestApi.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace OpenImis.RestApi.Controllers
{
    [ApiVersion("1")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IIMISRepository _imisRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="imisContext"></param>
        /// <param name="imisRepository"></param>
        public LoginController(IConfiguration configuration, IIMISRepository imisRepository)
        {
            _configuration = configuration;
            _imisRepository = imisRepository;
        }

        /// <summary>
        /// Creates the JWT token 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// ### REMARKS ###
        /// The following codes are returned
        /// - 400 - No sub domain is found that matches the SubDomainName property
        /// - 200 - Updated an existing API object
        /// - 201 - Created a new API object
        /// </remarks>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(LoginResponseModel), 200)]
        [ProducesResponseType(typeof(LoginBadRequestModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody]LoginRequestModel request)
        {
          
            IUserRepository userRepository = _imisRepository.getUserRepository();

            TblUsers user = await userRepository.GetByUsernameAndPasswordAsync(request.Username, request.Password);
             
            if (user!=null)
            {
                DateTime expirationDate = DateTime.Now.AddDays(double.Parse(_configuration["JwtExpireDays"]));

                IEnumerable<Claim> claims = new[]
                {
                    new Claim(ClaimTypes.Name, request.Username)
                };

                var roles = user.GetRolesStringArray();

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

                return Ok(new LoginResponseModel 
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token), 
                    Expires = expirationDate
                });
            }

            return Unauthorized();
        }
    }
}