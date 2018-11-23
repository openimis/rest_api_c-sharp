using OpenImis.Modules.MasterDataManagementModule.Models;
using OpenImis.Modules.MasterDataManagementModule.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenImis.Modules.MasterDataManagementModule.Logic
{
	public class EducationLevelLogic : IEducationLevelLogic
	{
		protected IEducationLevelRepository educationLevelRepository;
		protected IImisModules imisModules;

		public EducationLevelLogic()
		{

		}

		public EducationLevelLogic(IImisModules imisModules)
		{
			this.educationLevelRepository = new EducationLevelRepository();
			this.imisModules = imisModules;
		}

		public async Task<EducationLevelModel[]> GetAllEducationLevels()
		{
			// Authorize user

			// Validate input

			// Execute business behaviour

			EducationLevelModel[] educationLevels;
			educationLevels = await educationLevelRepository.GetAllEducationLevels();

			// Validate results

			// Validate data access rights

			// Return results 
			return educationLevels;
		}
	}
}
