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
	public class ProfessionTypeRepository : IProfessionTypeRepository
	{
		public async Task<ProfessionTypeModel[]> GetAllProfessionTypes()
		{
			ProfessionTypeModel[] familyTypes;

			using (var imisContext = new ImisDB())
			{

				familyTypes = await imisContext.TblProfessions
								  .Select(p => ProfessionTypeModel.FromTblProfessions(p))
								  .ToArrayAsync();

			}

			return familyTypes;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// TODO: when changing the table to allow multiple languages then create this method
		public async Task<ProfessionTypeModel[]> GetProfessionTypesByLanguage(string language)
		{
			throw new NotImplementedException();
		}
	}
}
