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
using System.ComponentModel.DataAnnotations;
using OpenImis.Modules.InsureeManagementModule.Models.Constants;

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

		public async Task<FamilyModel> GetFamilyByInsureeId(string insureeNumber)
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
										.Where(i => i.ValidityTo == null && i.Chfid == insureeNumber)
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

		public async Task<FamilyModel> GetFamilyByFamilyId(int familyId)
		{
			FamilyModel familyModel;

			using (var imisContext = new ImisDB())
			{
				familyModel = await imisContext.TblFamilies
										.Where(f => f.ValidityTo == null && f.FamilyId == familyId)
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
		public async Task<FamilyModel[]> GetFamilies(int page = 1, int resultsPerPage = 20)
		{
			FamilyModel[] families;

			using (var imisContext = new ImisDB())
			{
				families = await imisContext.TblFamilies
								  .Where(f => f.ValidityTo == null)
								  .Include(f => f.TblInsuree)
								  .Page(resultsPerPage <= 0 ? 20 : resultsPerPage, page <= 1 ? 1 : page) /// TODO: remove this as validation in Logic?
								  .Select(f => FamilyModel.FromTblFamilies(f))
								  .ToArrayAsync();


				if (families == null)
				{
					return null;
				}
			}

			return families;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// TODO: add location constraint
		public async Task<int> GetFamiliesCount()
		{
			int familiesCount = 0;

			using (var imisContext = new ImisDB())
			{
				familiesCount = await imisContext.TblFamilies
								  .CountAsync(f => f.ValidityTo == null);

			}

			return familiesCount;
		}

		/// <summary>
		/// Adds a new Family and the associated Insurees into the database
		/// </summary>
		/// <param name="family">The Family to be added</param>
		/// <returns></returns>
		public async Task<FamilyModel> AddNewFamilyAsync(FamilyModel family)
		{
			using (var imisContext = new ImisDB())
			{
				var tblFamily = family.ToTblFamilies();
				
				imisContext.Add(tblFamily);
				await imisContext.SaveChangesAsync();

				foreach (TblInsuree tblInsuree in tblFamily.TblInsuree)
				{
					if (tblInsuree.IsHead)
					{
						tblFamily.InsureeId = tblInsuree.InsureeId;
						break;
					}
				}

				await imisContext.SaveChangesAsync();

				return FamilyModel.FromTblFamilies(tblFamily);

			}

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="familyId"></param>
		/// <param name="family"></param>
		/// <returns></returns>
		/// TODO: make a copy of the record before updating it 
		/// TODO: check if there are any insuree in the family to be updated
		public async Task<FamilyModel> UpdateFamilyAsync(int familyId, FamilyModel family)
		{
			using (var imisContext = new ImisDB())
			{
				TblFamilies familyToUpdate = await imisContext.TblFamilies
										.Where(f => f.ValidityTo == null && f.FamilyId == familyId)
										//.Include(f => f.TblInsuree)
										.Select(f => f)
										.FirstOrDefaultAsync();

				if (familyToUpdate == null)
				{
					throw new ValidationException(FamilyErrors.FAMILY_NOT_FOUND_ERROR);
				}

				familyToUpdate.FamilyType = family.FamilyType;
				familyToUpdate.LocationId = family.LocationId;
				familyToUpdate.Poverty = family.Poverty;
				familyToUpdate.FamilyAddress = family.FamilyAddress;
				familyToUpdate.Ethnicity = family.Ethnicity;
				familyToUpdate.ConfirmationNo = family.ConfirmationNo;
				familyToUpdate.ConfirmationType = family.ConfirmationType;
				familyToUpdate.IsOffline = family.IsOffline;

				//await imisContext.SaveChangesAsync();

				//foreach (TblInsuree tblInsuree in tblFamily.TblInsuree)
				//{
				//	if (tblInsuree.IsHead)
				//	{
				//		tblFamily.InsureeId = tblInsuree.InsureeId;
				//		break;
				//	}
				//}

				await imisContext.SaveChangesAsync();

				return FamilyModel.FromTblFamilies(familyToUpdate);

			}

		}

		public async Task DeleteFamilyAsync(int familyId)
		{
			using (var imisContext = new ImisDB())
			{
				TblFamilies familyToDelete = await imisContext.TblFamilies
										.Where(f => f.ValidityTo == null && f.FamilyId == familyId)
										.Include(f => f.TblInsuree)
										.Select(f => f)
										.FirstOrDefaultAsync();

				if (familyToDelete == null)
				{
					throw new ValidationException(FamilyErrors.FAMILY_NOT_FOUND_ERROR);
				}

				familyToDelete.ValidityTo = DateTime.Now;
				
				foreach (TblInsuree tblInsuree in familyToDelete.TblInsuree)
				{

					tblInsuree.ValidityTo = DateTime.Now;
					
				}

				await imisContext.SaveChangesAsync();
			}

		}
	}
}
