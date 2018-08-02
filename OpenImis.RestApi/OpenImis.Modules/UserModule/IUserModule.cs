using OpenImis.Modules.UserModule.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.UserModule
{
    public interface IUserModule
    {

		IUserController GetUserController();

	}
}
