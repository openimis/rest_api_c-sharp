using OpenImis.Modules.InsureeManagementModule.Models;
using OpenImis.Modules.InsureeManagementModule.Protocol;
using OpenImis.Modules.InsureeManagementModule.Repositories;
using OpenImis.Modules.InsureeManagementModule.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.InsureeManagementModule.Logic
{
    public class FamilyLogic:IFamilyLogic
    {
		protected IValidator insureeNumberValidator;
		protected IFamilyRepository familyRepository;
		protected IImisModules imisModules;

		public FamilyLogic()
		{

		}

		public FamilyLogic(IImisModules imisModules)
		{
			this.familyRepository = new FamilyRepository();
			this.insureeNumberValidator = new InsureeNumberValidator(null);
			this.imisModules = imisModules;
		}

		public async Task<FamilyModel> GetFamilyByInsureeId(string insureeId)
		{
			// Authorize user

			// Validate input
			insureeNumberValidator.Validate(insureeId);


			// Execute business behaviour
			FamilyModel familyModel;
			familyModel = await familyRepository.GetFamilyByInsureeId(insureeId);

			// Validate results

			// Validate data access rights

			// Return results 
			return familyModel;
		}

		public async Task<FamilyModel> GetFamilyByFamilyId(int familyId)
		{
			// Authorize user

			// Validate input
			
			// Execute business behaviour
			FamilyModel familyModel;
			familyModel = await familyRepository.GetFamilyByFamilyId(familyId);

			// Validate results

			// Validate data access rights

			// Return results 
			return familyModel;
		}

		public async Task<GetFamiliesResponse> GetFamilies(int page = 1, int resultsPerPage = 20)
		{
			// Authorize user

			// Validate input
			if (resultsPerPage <= 0)
			{
				resultsPerPage = 20;
			}

			if (page <= 1)
			{
				page = 1;
			}

			// Execute business behaviour
			GetFamiliesResponse getFamiliesResponse = new GetFamiliesResponse();

			FamilyModel[] families;
			families = await familyRepository.GetFamilies(page, resultsPerPage);

			getFamiliesResponse.Families = families;

			int familyCount = await familyRepository.GetFamiliesCount();

			getFamiliesResponse.Pager.CurrentPage = page;
			getFamiliesResponse.Pager.ResultsPerPage = resultsPerPage;
			getFamiliesResponse.Pager.TotalResults = familyCount;
			getFamiliesResponse.Pager.TotalPages = familyCount / resultsPerPage + (familyCount % resultsPerPage > 0 ? 1 : 0);

			// Validate results

			// Validate data access rights

			// Return results 
			return getFamiliesResponse;
		}

		public async Task<FamilyModel> AddFamily(FamilyModel family)
		{
			// Authorize user

			// Validate input
			#region Validate input

			/// TODO: think of a strategy for validation => one solution to have one validation class that creates only
			/// one SQL call to check different validations => only one SQL access for validation 

			// check at least one insuree in the family 
			NotEmptyFamilyValidator notEmptyFamilyValidator = new NotEmptyFamilyValidator(null);
			OnlyOneHeadInFamilyValidator onlyOneHeadInFamilyValidator = new OnlyOneHeadInFamilyValidator(notEmptyFamilyValidator);
			onlyOneHeadInFamilyValidator.Validate(family);

			// check each insuree number is correct and unique
			/// TODO: add only one validator for the Insuree class as it will be used here and in the InsureeLogic 
			IInsureeLogic insureeLogic = this.imisModules.GetInsureeManagementModule().GetInsureeLogic();
			UniqueInsureeNumberValidator uniqueInsureeNumberValidator = new UniqueInsureeNumberValidator(insureeLogic, insureeNumberValidator);
			foreach (InsureeModel insuree in family.Insurees) {
				await uniqueInsureeNumberValidator.ValidateAsync(insuree.CHFID);
			}

			#endregion

			// Execute business behaviour
			FamilyModel newFamily = await familyRepository.AddNewFamilyAsync(family);

			// Validate results

			// Validate data access rights

			// Return results 
			return newFamily;
		}

		public async Task<FamilyModel> UpdateFamilyAsync(int familyId, FamilyModel family)
		{
			// Authorize user

			// Validate input
			#region Validate input

			/// TODO: think of a strategy for validation => one solution to have one validation class that creates only
			/// one SQL call to check different validations => only one SQL access for validation 

			#endregion

			// Execute business behaviour
			FamilyModel updatedFamily = await familyRepository.UpdateFamilyAsync(familyId, family);

			// Validate results

			// Validate data access rights

			// Return results 
			return updatedFamily;
		}

		public async Task DeleteFamilyAsync(int familyId)
		{
			// Authorize user

			// Validate input
			#region Validate input

			/// TODO: think of a strategy for validation => one solution to have one validation class that creates only
			/// one SQL call to check different validations => only one SQL access for validation 

			#endregion

			// Execute business behaviour
			await familyRepository.DeleteFamilyAsync(familyId);

			// Validate results

			// Validate data access rights

			// Return results 
			
		}
	}
}
