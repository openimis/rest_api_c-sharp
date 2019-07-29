using Microsoft.Extensions.Configuration;
using OpenImis.ModulesV1.LoginModule.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV1.LoginModule
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
