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
	public class FamilyTypeRepository : IFamilyTypeRepository
	{
		public async Task<FamilyTypeModel[]> GetAllFamilyTypes()
		{
			FamilyTypeModel[] familyTypes;

			using (var imisContext = new ImisDB())
			{

				familyTypes = await imisContext.TblFamilyTypes
								  .Select(ft => FamilyTypeModel.FromTblFamilyType(ft))
								  .ToArrayAsync();

				
			}

			return familyTypes;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// TODO: when changing the table to allow multiple languages then create this method
		public async Task<FamilyTypeModel[]> GetFamilyTypesByLanguage(string language)
		{
			throw new NotImplementedException();
		}
	}
}
