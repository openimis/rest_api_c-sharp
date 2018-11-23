using OpenImis.Modules.MasterDataManagementModule.Models;
using OpenImis.Modules.MasterDataManagementModule.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenImis.Modules.MasterDataManagementModule.Logic
{
	public class IdentificationTypeLogic : IIdentificationTypeLogic
	{
		protected IIdentificationTypeRepository identificationTypeRepository;
		protected IImisModules imisModules;

		public IdentificationTypeLogic()
		{

		}

		public IdentificationTypeLogic(IImisModules imisModules)
		{
			this.identificationTypeRepository = new IdentificationTypeRepository();
			this.imisModules = imisModules;
		}

		public async Task<IdentificationTypeModel[]> GetAllIdentificationTypes()
		{
			// Authorize user

			// Validate input

			// Execute business behaviour

			IdentificationTypeModel[] identificationTypes;
			identificationTypes = await identificationTypeRepository.GetAllIdentificationTypes();

			// Validate results

			// Validate data access rights

			// Return results 
			return identificationTypes;
		}
	}
}
