using OpenImis.Modules.WSModule.Models;
using OpenImis.Modules.WSModule.Repositories;
using OpenImis.Modules.WSModule.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.WSModule.Controllers
{
    public class FamilyController:IFamilyController
    {
		protected IValidator _insureeNumberValidator;
		protected IFamilyRepository _familyRepository;

		public FamilyController()
		{
			_familyRepository = new FamilyRepository();
			_insureeNumberValidator = new InsureeNumberValidator(null);
		}

		public async Task<FamilyModel> GetFamily(string chfId)
		{
			// Authorize user

			// Validate input
			_insureeNumberValidator.Validate(chfId);


			// Execute business behaviour
			FamilyModel familyModel;
			familyModel = await _familyRepository.GetFamily(chfId);

			// Validate results

			// Validate data access rights

			// Return results 
			return familyModel;
		}

	}
}
