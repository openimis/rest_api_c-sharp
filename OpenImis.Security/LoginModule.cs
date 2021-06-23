using Microsoft.Extensions.Configuration;
using OpenImis.Security.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Security
{
    public class LoginModule : ILoginModule
    {
        private IConfiguration _configuration;
        private ILoginLogic _loginLogic;

        public LoginModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ILoginLogic GetLoginLogic()
        {
            if (_loginLogic == null)
            {
                _loginLogic = new LoginLogic(_configuration);
            }
            return _loginLogic;
        }

        public ILoginModule SetLoginLogic(ILoginLogic loginLogic)
        {
            _loginLogic = loginLogic;
            return this;
        }
    }
}
