using OpenImis.Modules.UserModule.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.UserModule
{
    public class UserModuleRepository
	{
		private readonly IUserRepository _userRepository;

		public UserModuleRepository()
		{
			_userRepository = new UserSqlServer();
		}

		public IUserRepository GetUserSql()
		{
			return _userRepository;
		}

    }
}
