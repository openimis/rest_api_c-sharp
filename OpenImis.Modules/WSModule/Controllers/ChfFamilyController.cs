using OpenImis.Modules.WSModule.Repositories;
using OpenImis.Modules.WSModule.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.WSModule.Controllers
{
    public class ChfFamilyController: FamilyController
	{

		public ChfFamilyController()
		{
			_familyRepository = new FamilyRepository();
			_insureeNumberValidator = new ChfInsureeNumberValidator(null);
		}

	}
}
