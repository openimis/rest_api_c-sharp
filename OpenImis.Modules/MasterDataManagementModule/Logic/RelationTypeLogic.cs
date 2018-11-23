using OpenImis.Modules.MasterDataManagementModule.Models;
using OpenImis.Modules.MasterDataManagementModule.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenImis.Modules.MasterDataManagementModule.Logic
{
	public class RelationTypeLogic : IRelationTypeLogic
	{
		protected IRelationTypeRepository relationTypeRepository;
		protected IImisModules imisModules;

		public RelationTypeLogic()
		{

		}

		public RelationTypeLogic(IImisModules imisModules)
		{
			this.relationTypeRepository = new RelationTypeRepository();
			this.imisModules = imisModules;
		}

		public async Task<RelationTypeModel[]> GetAllRelationTypes()
		{
			// Authorize user

			// Validate input

			// Execute business behaviour

			RelationTypeModel[] relationTypes;
			relationTypes = await relationTypeRepository.GetAllRelationTypes();

			// Validate results

			// Validate data access rights

			// Return results 
			return relationTypes;
		}
	}
}
