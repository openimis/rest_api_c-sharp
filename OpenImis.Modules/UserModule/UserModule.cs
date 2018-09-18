using OpenImis.Modules.UserModule.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.UserModule
{
    public class UserModule:IUserModule
    {
		private IUserController _userController;
		
		public UserModule()
		{
			
		}

		public IUserController GetUserController()
		{
			if (_userController == null)
			{
				_userController = new UserController();
			}
			return _userController;
		}

		public IUserModule SetUserController(IUserController userController)
		{
			_userController = userController;
			return this;
		}


	}
}
