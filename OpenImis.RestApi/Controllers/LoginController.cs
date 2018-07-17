using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    [Route("api/{version:apiVersion}/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ICoreRepository _imisRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="imisContext"></param>
        /// <param name="imisRepository"></param>
        public LoginController(IConfiguration configuration, ICoreRepository imisRepository)
        {
            _configuration = configuration;
            _imisRepository = imisRepository;
        }

		/// <summary>
		/// Creates the JWT token 
		/// </summary>
		/// <remarks>
		/// ### REMARKS ###
		/// The following codes are returned
		/// - 200 - New generated token 
		/// - 400 - The request is invalid
		/// - 401 - Login credentials are invalid
		/// </remarks>
		/// <param name="request">The LoginRequestModel containing the username and password</param>
		/// <returns>The token related information</returns>
		/// <response code="200">Returns the token</response>
		/// <response code="400">If the request in incomplete</response>      
		/// <response code="401">If the login credentials are wrong</response>      
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