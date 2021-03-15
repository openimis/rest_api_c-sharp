using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ImisRestApi.Repo
{
    public class TokenValidatorImis : ISecurityTokenValidator
    {
        public bool CanValidateToken => throw new NotImplementedException();

        public int MaximumTokenSizeInBytes { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool CanReadToken(string securityToken)
        {
            throw new NotImplementedException();
        }

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            throw new NotImplementedException();
        }
    }
}
