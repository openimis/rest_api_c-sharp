using OpenImis.Modules.UserModule.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.UserModule
{
	public interface IUserModule
	{

		IUserController GetUserController();
		IUserModule SetUserController(IUserController userController);
	}
}
