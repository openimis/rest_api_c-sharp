using Microsoft.EntityFrameworkCore;
using OpenImis.DB.SqlServer;
using OpenImis.Modules.MasterDataManagementModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenImis.Modules.MasterDataManagementModule.Repositories
{
	public class EducationLevelRepository : IEducationLevelRepository
	{
		public async Task<EducationLevelModel[]> GetAllEducationLevels()
		{
			EducationLevelModel[] educationLevels;

			using (var imisContext = new ImisDB())
			{
				educationLevels = await imisContext.TblEducations
								  .Select(e => EducationLevelModel.FromTblEducations(e))
								  .ToArrayAsync();
			}

			return educationLevels;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// TODO: when changing the table to allow multiple languages then create this method
		public async Task<EducationLevelModel[]> GetEducationLevelsByLanguage(string language)
		{
			throw new NotImplementedException();
		}
	}
}
