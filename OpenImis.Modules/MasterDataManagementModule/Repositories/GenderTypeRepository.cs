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
	public class GenderTypeRepository : IGenderTypeRepository
	{
		public async Task<GenderTypeModel[]> GetAllGenderTypes()
		{
			GenderTypeModel[] genderTypes;

			using (var imisContext = new ImisDB())
			{
				genderTypes = await imisContext.TblGender
								  .OrderBy(g => g.SortOrder)
								  .Select(g => GenderTypeModel.FromTblGender(g))
								  .ToArrayAsync();
			}

			return genderTypes;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// TODO: when changing the table to allow multiple languages then create this method
		public async Task<GenderTypeModel[]> GetGenderTypesByLanguage(string language)
		{
			throw new NotImplementedException();
		}
	}
}
