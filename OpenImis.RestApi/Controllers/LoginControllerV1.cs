using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using OpenImis.RestApi.Protocol.LoginModel;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;

using OpenImis.RestApi.Models.Entities;
using OpenImis.Modules;
using System.ComponentModel.DataAnnotations;
using OpenImis.Modules.UserModule.Entities;

namespace OpenImis.RestApi.Controllers
{
    [ApiVersion("1")]
    [Route("api/login")]
    [ApiController]
    public class LoginControllerV1 : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IImisModules _imisModules;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="configuration"></param>
		/// <param name="imisModules"></param>
		public LoginControllerV1(IConfiguration configuration, IImisModules imisModules)
        {
            _configuration = configuration;
            _imisModules = imisModules;
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
          
            //IUserSQL userRepository = _imisRepository.getUserRepository();

            User user = await _imisModules.getUserModule().GetUserController().GetByUsernameAndPasswordAsync(request.Username, request.Password);
             
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