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

		public async Task<FamilyModel[]> GetAllFamilies(int page = 1, int numberPerPage = 20)
		{
			// Authorize user

			// Validate input
			
			// Execute business behaviour
			FamilyModel[] families;
			families = await _familyRepository.GetAllFamilies(page, numberPerPage);

			// Validate results

			// Validate data access rights

			// Return results 
			return families;
		}

		public async Task AddFamily(FamilyModel family)
		{
			// Authorize user

			// Validate input
			// check if the insuree number is correct 
			foreach (InsureeModel insuree in family.Insurees) { 
				_insureeNumberValidator.Validate(insuree.CHFID);
			}	

			// Execute business behaviour
			await _familyRepository.AddNewFamily(family);

			// Validate results

			// Validate data access rights

			// Return results 
		}
	}
}
