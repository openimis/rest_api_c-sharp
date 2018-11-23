using OpenImis.Modules.MasterDataManagementModule.Models;
using OpenImis.Modules.MasterDataManagementModule.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenImis.Modules.MasterDataManagementModule.Logic
{
	public class GenderTypeLogic : IGenderTypeLogic
	{
		protected IGenderTypeRepository genderTypeRepository;
		protected IImisModules imisModules;

		public GenderTypeLogic()
		{

		}

		public GenderTypeLogic(IImisModules imisModules)
		{
			this.genderTypeRepository = new GenderTypeRepository();
			this.imisModules = imisModules;
		}

		public async Task<GenderTypeModel[]> GetAllGenderTypes()
		{
			// Authorize user

			// Validate input

			// Execute business behaviour

			GenderTypeModel[] genderTypes;
			genderTypes = await genderTypeRepository.GetAllGenderTypes();

			// Validate results

			// Validate data access rights

			// Return results 
			return genderTypes;
		}
	}
}
