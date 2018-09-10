using OpenImis.Modules.UserModule.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.UserModule
{
    public class UserModule:IUserModule
    {
		private IUserController _userController;
		private readonly IUserModuleRepositories _userModuleRepository;

		public UserModule(IUserModuleRepositories userModuleRepository)
		{
			_userModuleRepository = userModuleRepository;
		}

		public IUserController GetUserController()
		{
			if (_userController == null)
			{
				_userController = new UserController(_userModuleRepository);
			}
			return _userController;
		}

    }
}
