using OpenImis.Modules.InsureeManagementModule.Repositories;
using OpenImis.Modules.InsureeManagementModule.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.InsureeManagementModule.Logic
{
    public class FamilyLogicForChf: FamilyLogic
	{

		public FamilyLogicForChf(IImisModules imisModules)
		{
			this.familyRepository = new FamilyRepository();
			this.insureeNumberValidator = new ChfInsureeNumberValidator(null);
			this.imisModules = imisModules;
		}

	}
}
