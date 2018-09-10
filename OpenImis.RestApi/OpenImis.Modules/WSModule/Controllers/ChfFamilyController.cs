using OpenImis.Modules.WSModule.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.WSModule.Controllers
{
    public class ChfFamilyController: FamilyController
	{

		public ChfFamilyController(IWSModuleRepositories repositories, IWSValidators validators):base(repositories, validators)
		{
			this._repositories = new WSModuleRepositories();
			this._validators = new Validators.ChfValidators();
		}

	}
}
