using OpenImis.Modules.MasterDataManagementModule.Models;
using OpenImis.Modules.MasterDataManagementModule.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenImis.Modules.MasterDataManagementModule.Logic
{
	public class FamilyTypeLogic : IFamilyTypeLogic
	{
		protected IFamilyTypeRepository familyTypeRepository;
		protected IImisModules imisModules;

		public FamilyTypeLogic()
		{

		}

		public FamilyTypeLogic(IImisModules imisModules)
		{
			this.familyTypeRepository = new FamilyTypeRepository();
			this.imisModules = imisModules;
		}

		public async Task<FamilyTypeModel[]> GetAllFamilyTypes()
		{
			// Authorize user

			// Validate input

			// Execute business behaviour

			FamilyTypeModel[] familyTypes;
			familyTypes = await familyTypeRepository.GetAllFamilyTypes();
						
			// Validate results

			// Validate data access rights

			// Return results 
			return familyTypes;
		}
	}
}
