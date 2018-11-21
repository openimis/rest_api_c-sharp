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
	public class IdentificationTypeRepository : IIdentificationTypeRepository
	{
		public async Task<IdentificationTypeModel[]> GetAllIdentificationTypes()
		{
			IdentificationTypeModel[] identificationTypes;

			using (var imisContext = new ImisDB())
			{

				identificationTypes = await imisContext.TblIdentificationTypes
								  .Select(it => IdentificationTypeModel.FromTblIdentificationTypes(it))
								  .ToArrayAsync();

			}

			return identificationTypes;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// TODO: when changing the table to allow multiple languages then create this method
		public async Task<IdentificationTypeModel[]> GetIdentificationTypesByLanguage(string language)
		{
			throw new NotImplementedException();
		}
	}
}
