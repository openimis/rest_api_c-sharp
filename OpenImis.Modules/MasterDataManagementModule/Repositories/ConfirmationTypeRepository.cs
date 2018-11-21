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
	public class ConfirmationTypeRepository : IConfirmationTypeRepository
	{
		public async Task<ConfirmationTypeModel[]> GetAllConfirmationsTypes()
		{
			ConfirmationTypeModel[] confirmationTypes;

			using (var imisContext = new ImisDB())
			{

				confirmationTypes = await imisContext.TblConfirmationTypes
								  .Select(ct => ConfirmationTypeModel.FromTblConfirmationType(ct))
								  .ToArrayAsync();


			}

			return confirmationTypes;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// TODO: when changing the table to allow multiple languages then create this method
		public async Task<ConfirmationTypeModel[]> GetConfirmationTypesByLanguage(string language)
		{
			
			throw new NotImplementedException();

			
		}
	}
}
