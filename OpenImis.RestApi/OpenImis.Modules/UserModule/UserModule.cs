using OpenImis.Modules.UserModule.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.UserModule
{
    public class UserModule:IUserModule
    {
		private readonly IUserController _userController;

		public UserModule()
		{
			_userController = new UserController();
		}

		public IUserController GetUserController()
		{
			return _userController;
		}

    }
}
