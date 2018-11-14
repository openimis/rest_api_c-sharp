using OpenImis.DB.SqlServer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenImis.Modules.Utils;
using OpenImis.Modules.InsureeManagementModule.Models;

namespace OpenImis.Modules.InsureeManagementModule.Repositories
{
	/// <summary>
	/// This class is actual implementation of IFamilyRepository methods for Master Version implementation 
	/// </summary>
	public class FamilyRepository : IFamilyRepository
	{

		public FamilyRepository()
		{
		}

		public async Task<FamilyModel> GetFamilyByInsureeId(string insureeId)
		{
			FamilyModel familyModel;

			using (var imisContext = new ImisDB())
			{
				familyModel = await
									 //(from i in imisContext.TblInsuree
									 //				 join f in imisContext.TblFamilies on i.FamilyId equals f.FamilyId
									 //				 where i.ValidityTo == null && i.Chfid == insureeId && f.ValidityTo == null
									 //				 select FamilyModel.FromTblFamilies(f))
									 imisContext.TblInsuree
										.Where(i => i.ValidityTo == null && i.Chfid == insureeId)
										.Join(imisContext.TblFamilies, i => i.FamilyId, f => f.FamilyId, (i, f) => f)
										.Where(f => f.ValidityTo == null)
										.Include(f => f.TblInsuree)
										.Select(f => FamilyModel.FromTblFamilies(f))
										.FirstOrDefaultAsync();


				if (familyModel == null)
				{
					return null;
				}

			}

			return familyModel;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// TODO: add location constraint
		public async Task<FamilyModel[]> GetAllFamilies(int page = 1, int numberPerPage = 20)
		{
			FamilyModel[] families;

			using (var imisContext = new ImisDB())
			{
				families = await imisContext.TblFamilies
								  .Where(f => f.ValidityTo == null)
								  .Include(f => f.TblInsuree)
								  .Page(numberPerPage <= 0 ? 20 : numberPerPage, page <= 1 ? 1 : page)
								  .Select(f => FamilyModel.FromTblFamilies(f))
								  .ToArrayAsync();


				if (families == null)
				{
					return null;
				}
			}

			return families;
		}

		public async Task AddNewFamily(FamilyModel family)
		{
			using (var imisContext = new ImisDB())
			{
				var tblFamily = family.ToTblFamilies();
				//	new TblFamilies()
				//{
				//	LocationId = family.LocationId,
				//	Poverty = family.Poverty,
				//	FamilyType = family.FamilyType.ToString(),
				//	FamilyAddress = family.FamilyAddress,
				//	Ethnicity = family.Ethnicity,
				//	ConfirmationNo = family.ConfirmationNo,
				//	ConfirmationType = family.ConfirmationType,
				//	IsOffline = family.IsOffline,
				//	TblInsuree = new List<TblInsuree>()
				//};
				//// add first all insurees
				//foreach (InsureeModel insuree in family.Insurees)
				//{
				//	var tblInsuree = insuree.ToTblInsuree();
				//	if (insuree.IsHead)
				//	{
				//		tblFamily.Insuree = tblInsuree;
				//	}
				//	tblFamily.TblInsuree.Add(tblInsuree);
				//}

				imisContext.Add(tblFamily);
				await imisContext.SaveChangesAsync();

			}

		}

	}
}
