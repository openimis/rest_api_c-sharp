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
	public class RelationTypeRepository : IRelationTypeRepository
	{
		public async Task<RelationTypeModel[]> GetAllRelationTypes()
		{
			RelationTypeModel[] relationTypes;

			using (var imisContext = new ImisDB())
			{
				relationTypes = await imisContext.TblRelations
								  .Select(r => RelationTypeModel.FromTblRelations(r))
								  .ToArrayAsync();

			}

			return relationTypes;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// TODO: when changing the table to allow multiple languages then create this method
		public async Task<RelationTypeModel[]> GetRelationTypesByLanguage(string language)
		{
			throw new NotImplementedException();
		}
	}
}
