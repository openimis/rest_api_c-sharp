using OpenImis.Modules.Utils;
using OpenImis.Modules.InsureeManagementModule.Models;
using OpenImis.DB.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OpenImis.Modules.InsureeManagementModule.Repositories
{
    public class InsureeRepository: IInsureeRepository
    {

		public InsureeModel GetInsureeByCHFID(string chfId)
		{
			InsureeModel insuree;

			using (var imisContext = new ImisDB())
			{
				insuree = (from i in imisContext.TblInsuree
									 where i.Chfid == chfId
									 && i.ValidityTo == null
									 select InsureeModel.FromTblInsuree(i))
									 .FirstOrDefault();
			}

			return insuree;
		}

		public async Task<InsureeModel> GetInsureeByCHFIDAsync(string chfId)
		{
			InsureeModel insuree;

			using (var imisContext = new ImisDB())
			{
				insuree = await (from i in imisContext.TblInsuree
								 where i.Chfid == chfId
								 && i.ValidityTo == null
								 select InsureeModel.FromTblInsuree(i))
									 .FirstOrDefaultAsync();
			}

			return insuree;
		}


		public async Task<IEnumerable<InsureeModel>> GetInsureeByFamilyIdAsync(int familyId)
		{
			IEnumerable<InsureeModel> insureeList;

			using (var imisContext = new ImisDB())
			{
				
				insureeList = await (from i in imisContext.TblInsuree
							where i.FamilyId == familyId
							&& i.ValidityTo == null
							select InsureeModel.FromTblInsuree(i))
							.ToListAsync();

			}

			return insureeList;
		}
	}
}
