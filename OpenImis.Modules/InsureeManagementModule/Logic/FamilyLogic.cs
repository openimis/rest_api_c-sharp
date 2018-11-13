using OpenImis.Modules.InsureeManagementModule.Models;
using OpenImis.Modules.InsureeManagementModule.Repositories;
using OpenImis.Modules.InsureeManagementModule.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.InsureeManagementModule.Logic
{
    public class FamilyLogic:IFamilyLogic
    {
		protected IValidator _insureeNumberValidator;
		protected IFamilyRepository _familyRepository;

		public FamilyLogic()
		{
			_familyRepository = new FamilyRepository();
			_insureeNumberValidator = new InsureeNumberValidator(null);
		}

		public async Task<FamilyModel> GetFamilyByInsureeId(string insureeId)
		{
			// Authorize user

			// Validate input
			_insureeNumberValidator.Validate(insureeId);


			// Execute business behaviour
			FamilyModel familyModel;
			familyModel = await _familyRepository.GetFamilyByInsureeId(insureeId);

			// Validate results

			// Validate data access rights

			// Return results 
			return familyModel;
		}

		public async Task<FamilyModel[]> GetAllFamilies(int page = 1, int numberPerPage = 0)
		{
			// Authorize user

			// Validate input
			
			// Execute business behaviour
			FamilyModel[] families;
			families = await _familyRepository.GetAllFamilies();

			// Validate results

			// Validate data access rights

			// Return results 
			return families;
		}

	}
}
