using OpenImis.Modules.UserModule.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.UserModule
{
    public class UserModuleRepositories: IUserModuleRepositories
	{
		private IUserRepository _userRepository;

		public UserModuleRepositories()
		{
			
		}

		public IUserRepository GetUserSql()
		{
			if (_userRepository == null)
			{
				_userRepository = new UserSqlServer();
			}
			return _userRepository;
		}

    }
}
