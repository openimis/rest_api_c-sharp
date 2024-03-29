﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using OpenImis.Security;
using Microsoft.Extensions.Configuration;

namespace OpenImis.Security.Security
{
    /// <summary>
    /// This class replaces the default JWT token handler and is used to construct a custom token validator 
    /// </summary>
    public class IMISJwtSecurityTokenHandler : ISecurityTokenValidator
    {
        private IConfiguration _configuration;

        private int _maxTokenSizeInBytes = TokenValidationParameters.DefaultMaximumTokenSizeInBytes;
        private readonly JwtSecurityTokenHandler _tokenHandler;
        

        //private readonly ModulesV1.IImisModules _imisModulesV1;
        //private readonly ModulesV2.IImisModules _imisModulesV2;
        //private readonly ModulesV3.IImisModules _imisModulesV3;
        private readonly IHttpContextAccessor _httpContextAccessor;

        // public IMISJwtSecurityTokenHandler(ModulesV1.IImisModules imisModulesV1, ModulesV2.IImisModules imisModulesV2, ModulesV3.IImisModules imisModulesV3, IHttpContextAccessor httpContextAccessor)
        public IMISJwtSecurityTokenHandler(IHttpContextAccessor httpContextAccessor)
        {
            _tokenHandler = new JwtSecurityTokenHandler();
            _httpContextAccessor = httpContextAccessor;

            //_imisModulesV1 = imisModulesV1;
            //_imisModulesV2 = imisModulesV2;
            //_imisModulesV3 = imisModulesV3;
        }

        public bool CanValidateToken
        {
            get
            {
                return true;
            }
        }

        public int MaximumTokenSizeInBytes
        {
            get
            {
                return _maxTokenSizeInBytes;
            }

            set
            {
                _maxTokenSizeInBytes = value;
            }
        }

        public bool CanReadToken(string securityToken)
        {
            return _tokenHandler.CanReadToken(securityToken);
        }

        /// <summary>
        /// Validated tokens based on user's private key
        /// </summary>
        /// <param name="securityToken"></param>
        /// <param name="validationParameters"></param>
        /// <param name="validatedToken"></param>
        /// <returns>ClaimsPrincipal</returns>
        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            ClaimsPrincipal principal;

            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(securityToken) as JwtSecurityToken;
            
            string apiVersion = _httpContextAccessor.HttpContext.Request.Headers.Where(x => x.Key == "api-version").Select(s => s.Value).FirstOrDefault();

            UserData user;

            Guid userUUID = Guid.Parse(tokenS.Claims.Where(w => w.Type == "UserUUID").Select(x => x.Value).FirstOrDefault());
            var _ = new Logic.LoginLogic(_configuration); ;
            user = (UserData)_.GetByUUID(userUUID);

            //if (apiVersion == null || apiVersion.StartsWith("2"))
            //{
            //    Guid userUUID = Guid.Parse(tokenS.Claims.Where(w => w.Type == "UserUUID").Select(x => x.Value).FirstOrDefault());

            //    user = (UserData)_imisModulesV2.GetLoginModule().GetLoginLogic().GetByUUID(userUUID);
            //}
            //else if (apiVersion.StartsWith("3"))
            //{
            //    Guid userUUID = Guid.Parse(tokenS.Claims.Where(w => w.Type == "UserUUID").Select(x => x.Value).FirstOrDefault());

            //    user = (UserData)_imisModulesV3.GetLoginModule().GetLoginLogic().GetByUUID(userUUID);

            //}
            //else
            //{
            //    int userId = Convert.ToInt32(tokenS.Claims.Where(w => w.Type == "UserId").Select(x => x.Value).FirstOrDefault());

            //    user = (UserData)_imisModulesV1.GetLoginModule().GetLoginLogic().GetById(userId);
            //}

            if (user != null)
            {
                TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = validationParameters.ValidateIssuer,
                    ValidateAudience = validationParameters.ValidateAudience,
                    ValidateLifetime = validationParameters.ValidateLifetime,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = validationParameters.ValidIssuer,
                    ValidAudience = validationParameters.ValidAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(user.PrivateKey))
                };

                principal = _tokenHandler.ValidateToken(securityToken, tokenValidationParameters, out validatedToken);

            }
            else
            {
                principal = _tokenHandler.ValidateToken(securityToken, validationParameters, out validatedToken);
            }

            return principal;
        }
    }
}
