using OpenImis.Modules.MasterDataManagementModule.Models;
using OpenImis.Modules.MasterDataManagementModule.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenImis.Modules.MasterDataManagementModule.Logic
{
	public class ProfessionTypeLogic : IProfessionTypeLogic
	{
		protected IProfessionTypeRepository professionTypeRepository;
		protected IImisModules imisModules;

		public ProfessionTypeLogic()
		{

		}

		public ProfessionTypeLogic(IImisModules imisModules)
		{
			this.professionTypeRepository = new ProfessionTypeRepository();
			this.imisModules = imisModules;
		}

		public async Task<ProfessionTypeModel[]> GetAllProfessionTypes()
		{
			// Authorize user

			// Validate input

			// Execute business behaviour

			ProfessionTypeModel[] professionTypes;
			professionTypes = await professionTypeRepository.GetAllProfessionTypes();

			// Validate results

			// Validate data access rights

			// Return results 
			return professionTypes;
		}
	}
}
