﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OpenImis.Security.Models;
using OpenImis.ModulesV3;
using OpenImis.Security;
using Microsoft.Extensions.Logging;
using OpenImis.RestApi.Util.ErrorHandling;
using Microsoft.AspNetCore.Http;

namespace OpenImis.RestApi.Controllers.V3
{
    [ApiVersion("3")]
    [Authorize]
    [Route("api/")]
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILoginModule _loginModule;
        private readonly ILogger _logger;

        public LoginController(IConfiguration configuration, ILoginModule loginModule, ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _loginModule = loginModule;
            _logger = loggerFactory.CreateLogger<LoginController>();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public IActionResult Index([FromBody] LoginModel model)
        {

            var user = _loginModule.GetLoginLogic().FindUser(model.UserName, model.Password);

            if (user != null)
            {
                DateTime expirationDate = DateTime.Now.AddDays(double.Parse(_configuration["JwtExpireDays"]));

                List<Claim> claims = new List<Claim>()
                {
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

        [AllowAnonymous]
        [HttpPost]
        [Route("validate/credentials")]
        public virtual IActionResult Validate_Credentials([FromBody] UserLogin userlogin)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new { error_occured = true, error_message = error });
            }

            ValidateCredentialsResponse response = new ValidateCredentialsResponse() { success = false };

            var user = _loginModule.GetLoginLogic().FindUser(userlogin.UserID, userlogin.Password);

            if (user != null)
            {
                response.success = true;
                response.ErrorOccured = false;
            }

            return Json(response);
        }


        [HttpPost]
        [Route("userinfo")]
        public IActionResult GetUserInfo()
        {
            Guid userUUID;

            var user = new UserModel();

            userUUID = Guid.Parse(HttpContext.User.Claims.Where(w => w.Type == "UserUUID").Select(x => x.Value).FirstOrDefault());
            user = _loginModule.GetLoginLogic().GetUserDetails(userUUID);
            return Ok(user);

        }

    }

    
}