using OpenImis.Modules.MasterDataManagementModule.Models;
using OpenImis.Modules.MasterDataManagementModule.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenImis.Modules.MasterDataManagementModule.Logic
{
	public class ConfirmationTypeLogic : IConfirmationTypeLogic
	{
		protected IConfirmationTypeRepository confirmationTypeRepository;
		protected IImisModules imisModules;

		public ConfirmationTypeLogic()
		{

		}

		public ConfirmationTypeLogic(IImisModules imisModules)
		{
			this.confirmationTypeRepository = new ConfirmationTypeRepository();
			this.imisModules = imisModules;
		}

		public async Task<ConfirmationTypeModel[]> GetAllConfirmationTypes()
		{
			// Authorize user

			// Validate input

			// Execute business behaviour

			ConfirmationTypeModel[] confirmationTypes;
			confirmationTypes = await confirmationTypeRepository.GetAllConfirmationsTypes();

			// Validate results

			// Validate data access rights

			// Return results 
			return confirmationTypes;
		}
	}
}
