using OpenImis.Modules.UserModule.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.UserModule
{
    public interface IUserModuleRepositories
    {
		IUserRepository GetUserSql();
	}
}
