using Microsoft.Extensions.Configuration;
using OpenImis.Modules.LoginModule.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.LoginModule
{
    public class LoginModule : ILoginModule
    {
        private IConfiguration Configuration;
        private ILoginLogic _loginLogic;

        public LoginModule(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public ILoginLogic GetLoginLogic()
        {
            if (_loginLogic == null)
            {
                _loginLogic = new LoginLogic(Configuration);
            }
            return _loginLogic;
        }
    }
}
