using OpenImis.Modules.WSModule.Models;
using OpenImis.Modules.WSModule.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.WSModule.Controllers
{
    public class FamilyController:IFamilyController
    {
		protected IWSModuleRepositories _repositories;
		protected IWSValidators _validators;

		public FamilyController(IWSModuleRepositories repositories, IWSValidators validators)
		{
			_repositories = repositories;
			_validators = validators;
		}

		public async Task<FamilyModel> GetFamily(string chfId)
		{
			// Validate input
			IValidator validator = _validators.GetInsureeNumberValidator(null);
			validator.Validate(chfId);


			// Execute business behaviour
			FamilyModel familyModel;
			familyModel = await _repositories.GetFamilyRepository().GetFamily(chfId);

			// Validate results

			// Validate data access rights

			// Return results 
			return familyModel;
		}

	}
}
