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
				familyModel = await (from i in imisContext.TblInsuree
									 join f in imisContext.TblFamilies on i.FamilyId equals f.FamilyId
									 where i.Chfid == insureeId
									 select new FamilyModel()
									 {
										FamilyId = f.FamilyId,
										LocationId = TypeCast.GetValue<int>(f.LocationId),
										Poverty = TypeCast.GetValue<bool>(f.Poverty),
										FamilyType = f.FamilyType == null ? ' ' : f.FamilyType[0],
										FamilyAddress = f.FamilyAddress,
										Ethnicity = f.Ethnicity,
										ConfirmationNo = f.ConfirmationNo,
										ConfirmationType = f.ConfirmationType,
										IsOffline = TypeCast.GetValue<bool>(f.IsOffline),
									}).FirstOrDefaultAsync();


				if (familyModel == null)
				{
					return null;
				}

				IEnumerable<InsureeModel> insurees = await (from i in imisContext.TblInsuree
												   where i.FamilyId == familyModel.FamilyId
												   && i.ValidityTo == null 
												   select new InsureeModel()
												   {
													   InsureeId = i.InsureeId,
														IdentificationNumber = i.Passport,
														CHFID = i.Chfid,
														LastName = i.LastName,
														OtherNames = i.OtherNames,
														DOB = i.Dob.ToShortDateString(),
														Gender = i.Gender == null ? ' ' : i.Gender[0],
														Marital = i.Marital == null ? ' ' : i.Marital[0],
														IsHead = i.IsHead,
														Phone = i.Phone,
														CardIssued = i.CardIssued,
														Relationship = TypeCast.GetValue<short>(i.Relationship),
														Profession = TypeCast.GetValue<short>(i.Profession),
														Education = TypeCast.GetValue<short>(i.Education),
														Email = i.Email,
														TypeOfId = i.TypeOfId == null ? ' ' : i.TypeOfId[0],
														HFID = TypeCast.GetValue<int>(i.Hfid),
														CurrentAddress = i.CurrentAddress,
														GeoLocation = i.GeoLocation,
														CurVillage = TypeCast.GetValue<string>(i.CurrentVillage), // todo: is there any link missing?
														PhotoPath = i.Photo.PhotoFileName,
														IdentificationTypes = i.TypeOf.IdentificationTypes,
														IsOffline = TypeCast.GetValue<bool>(i.IsOffline),
													}).ToListAsync();

				familyModel.Insurees = familyModel.Insurees.Concat(insurees);

			}

			return familyModel;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// TODO: add location constraint
		public async Task<FamilyModel[]> GetAllFamilies(int page = 1, int numberPerPage = 0)
		{
			FamilyModel[] families;

			using (var imisContext = new ImisDB())
			{
				families = await (from f in imisContext.TblFamilies
								  where f.ValidityTo == null
									  //group f.TblInsuree by f.FamilyId into i
									  //where i.Chfid == insureeId
								  select new FamilyModel()
									 {
										 FamilyId = f.FamilyId,
										 LocationId = TypeCast.GetValue<int>(f.LocationId),
										 Poverty = TypeCast.GetValue<bool>(f.Poverty),
										 FamilyType = f.FamilyType == null ? ' ' : f.FamilyType[0],
										 FamilyAddress = f.FamilyAddress,
										 Ethnicity = f.Ethnicity,
										 ConfirmationNo = f.ConfirmationNo,
										 ConfirmationType = f.ConfirmationType,
										 IsOffline = TypeCast.GetValue<bool>(f.IsOffline),
										 Insurees = imisContext.TblInsuree
													.Where(i => i.FamilyId == f.FamilyId && i.ValidityTo == null)
													.Select(i => new InsureeModel()
													  {
														  InsureeId = i.InsureeId,
														  IdentificationNumber = i.Passport,
														  CHFID = i.Chfid,
														  LastName = i.LastName,
														  OtherNames = i.OtherNames,
														  DOB = i.Dob.ToShortDateString(),
														  Gender = i.Gender == null ? ' ' : i.Gender[0],
														  Marital = i.Marital == null ? ' ' : i.Marital[0],
														  IsHead = i.IsHead,
														  Phone = i.Phone,
														  CardIssued = i.CardIssued,
														  Relationship = TypeCast.GetValue<short>(i.Relationship),
														  Profession = TypeCast.GetValue<short>(i.Profession),
														  Education = TypeCast.GetValue<short>(i.Education),
														Email = i.Email,
														TypeOfId = i.TypeOfId == null ? ' ' : i.TypeOfId[0],
														HFID = TypeCast.GetValue<int>(i.Hfid),
														CurrentAddress = i.CurrentAddress,
														GeoLocation = i.GeoLocation,
														CurVillage = imisContext.TblLocations
																	.Where(l => l.LocationId == i.CurrentVillage)
																	.Select(l => l.LocationName)
																	.FirstOrDefault(), // todo: is there any link missing?
														PhotoPath = i.Photo.PhotoFileName,
														IdentificationTypes = i.TypeOf.IdentificationTypes,
														IsOffline = TypeCast.GetValue<bool>(i.IsOffline),
													}).ToList()
								  }).ToArrayAsync();


				if (families == null)
				{
					return null;
				}

				//IEnumerable<InsureeModel> insurees;

				//foreach (FamilyModel family in families) 
				//{
				//	insurees = await (from i in imisContext.TblInsuree
				//					  where i.FamilyId == family.FamilyId
				//					  && i.ValidityTo == null
				//					  select new InsureeModel()
				//					  {
				//						  InsureeId = i.InsureeId,
				//						  IdentificationNumber = i.Passport,
				//						  CHFID = i.Chfid,
				//						  LastName = i.LastName,
				//						  OtherNames = i.OtherNames,
				//						  DOB = i.Dob.ToShortDateString(),
				//						  Gender = i.Gender == null ? ' ' : i.Gender[0],
				//						  Marital = i.Marital == null ? ' ' : i.Marital[0],
				//						  IsHead = i.IsHead,
				//						  Phone = i.Phone,
				//						  CardIssued = i.CardIssued,
				//						  Relationship = TypeCast.GetValue<short>(i.Relationship),
				//						  Profession = TypeCast.GetValue<short>(i.Profession),
				//						  Education = TypeCast.GetValue<short>(i.Education),
				//						  //Email = i.Email,
				//						  //TypeOfId = i.TypeOfId == null ? ' ' : i.TypeOfId[0],
				//						  //HFID = TypeCast.GetValue<string>(i.Hfid),
				//						  //CurrentAddress = i.CurrentAddress,
				//						  //GeoLocation = i.GeoLocation,
				//						  //CurVillage = TypeCast.GetValue<string>(i.CurrentVillage), // todo: is there any link missing?
				//						  //PhotoPath = i.Photo.PhotoFileName,
				//						  //IdentificationTypes = i.TypeOf.IdentificationTypes,
				//						  //IsOffline = TypeCast.GetValue<bool>(i.IsOffline),
				//					  }).ToListAsync();
				//	family.Insurees = family.Insurees.Concat(insurees);
				//}
			}

			return families;
		}

	}
}
