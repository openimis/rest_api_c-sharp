using OpenImis.Modules.Utils;
using OpenImis.Modules.WSModule.Models;
using OpenImis.DB.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OpenImis.Modules.WSModule.Repositories
{
    public class InsureeRepository: IInsureeRepository
    {

		public async Task<InsureeModel> GetInsureeByCHFIDAsync(string chfId)
		{
			InsureeModel insuree;

			using (var imisContext = new ImisDB())
			{
				insuree = await (from i in imisContext.TblInsuree
									 where i.Chfid == chfId
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
										 HFID = TypeCast.GetValue<string>(i.Hfid),
										 CurrentAddress = i.CurrentAddress,
										 GeoLocation = i.GeoLocation,
										 CurVillage = TypeCast.GetValue<string>(i.CurrentVillage), // todo: is there any link missing?
										 PhotoPath = i.Photo.PhotoFileName,
										 IdentificationTypes = i.TypeOf.IdentificationTypes,
										 IsOffline = TypeCast.GetValue<bool>(i.IsOffline)
									 }).FirstOrDefaultAsync();
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
								HFID = TypeCast.GetValue<string>(i.Hfid),
								CurrentAddress = i.CurrentAddress,
								GeoLocation = i.GeoLocation,
								CurVillage = TypeCast.GetValue<string>(i.CurrentVillage), // todo: is there any link missing?
								PhotoPath = i.Photo.PhotoFileName,
								IdentificationTypes = i.TypeOf.IdentificationTypes,
								IsOffline = TypeCast.GetValue<bool>(i.IsOffline)
							}).ToListAsync();

			}

			return insureeList;
		}
	}
}
