﻿using Microsoft.Extensions.Configuration;
using OpenImis.Security.Models;
using OpenImis.Security.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace OpenImis.Security.Logic
{
    public class LoginLogic : ILoginLogic
    {
        private IConfiguration _configuration;
        protected ILoginRepository loginRepository;

        public LoginLogic(IConfiguration configuration)
        {
            _configuration = configuration;
            loginRepository = new LoginRepository(_configuration);
        }

        public UserData GetByUUID(Guid userUUID)
        {
            return loginRepository.GetByUUID(userUUID);
        }

        public UserData FindUser(string UserName, string Password)
        {
            var users = loginRepository.FindUserByName(UserName);

            if (users.Count == 1)
            {
                UserData user = users.FirstOrDefault();

                bool validUser = ValidateLogin(user, Password);

                if (validUser)
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private bool ValidateLogin(UserData user, string password)
        {
            var generatedSHA = GenerateSHA256String(password + user.PrivateKey);
            if (generatedSHA == user.StoredPassword)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string GenerateSHA256String(string inputString)
        {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha256.ComputeHash(bytes);
            var stringBuilder = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                var str = hash[i].ToString("X2");
                stringBuilder.Append(str);
            }

            return stringBuilder.ToString();
        }

        public UserModel GetUserDetails(Guid userUUID)
        {
            return loginRepository.GetUserDetails(userUUID);
        }
    }
}