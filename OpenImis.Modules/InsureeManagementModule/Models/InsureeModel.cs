using OpenImis.Modules.Utils;
using OpenImis.DB.SqlServer;
using System;
using OpenImis.Modules.MasterDataManagementModule.Models;

namespace OpenImis.Modules.InsureeManagementModule.Models
{
    public class InsureeModel
    {
		public int InsureeId { get; set; }
		public string IdentificationNumber { get; set; }
		public string CHFID { get; set; }
		public string LastName { get; set; }
		public string OtherNames { get; set; }
		public DateTime DOB { get; set; }
		public string Gender { get; set; }
		public string Marital { get; set; }
		public bool IsHead { get; set; }
		public string Phone { get; set; }
		public bool CardIssued { get; set; }
		public short? Relationship { get; set; }
		public short? Profession { get; set; }
		public short? Education { get; set; }
		public string Email { get; set; }
		public string TypeOfId { get; set; }
		public int? HFID { get; set; }
		public string CurrentAddress { get; set; }
		public string GeoLocation { get; set; }
		public LocationModel CurrentVillage { get; set; }
		public PhotoModel Photo { get; set; }
		public string IdentificationTypes { get; set; }
		public bool IsOffline { get; set; }

		public DateTime ValidFrom { get; set; }
		public DateTime? ValidTo { get; set; }

		public InsureeModel()
		{
			ValidFrom = DateTime.Now;
			//this.CurrentVillage = new LocationModel();
		}

		//public InsureeModel(TblInsuree tblInsuree):base()
		//{
		//	this.FromTblInsuree(tblInsuree);	
		//}

		public static InsureeModel FromTblInsuree(TblInsuree tblInsuree)
		{
			if (tblInsuree == null)
			{
				return null;
			}

			InsureeModel insuree = new InsureeModel()
			{
				InsureeId = tblInsuree.InsureeId,
				IdentificationNumber = tblInsuree.Passport,
				CHFID = tblInsuree.Chfid,
				LastName = tblInsuree.LastName,
				OtherNames = tblInsuree.OtherNames,
				DOB = tblInsuree.Dob,
				IsHead = tblInsuree.IsHead,
				Phone = tblInsuree.Phone,
				Gender = tblInsuree.Gender,
				Marital = tblInsuree.Marital,
				TypeOfId = tblInsuree.TypeOfId,
				CardIssued = tblInsuree.CardIssued,
				Email = tblInsuree.Email,
				CurrentAddress = tblInsuree.CurrentAddress,
				GeoLocation = tblInsuree.GeoLocation,
				IdentificationTypes = tblInsuree.TypeOfId, /// todo: link to the table value
				
				ValidFrom = tblInsuree.ValidityFrom,
				ValidTo = TypeCast.GetValue<DateTime>(tblInsuree.ValidityTo)
			};
			if (tblInsuree.Relationship != null)
			{
				insuree.Relationship = (short)tblInsuree.Relationship; /// TODO: link to the detailed table 
			}
			if (tblInsuree.Profession != null)
			{
				insuree.Profession = TypeCast.GetValue<short>(tblInsuree.Profession);/// TODO: link to the detailed table 
			}
			if (tblInsuree.Education != null)
			{
				insuree.Education = TypeCast.GetValue<short>(tblInsuree.Education);/// TODO: link to the detailed table 
			}
			if (tblInsuree.Hfid != null)
			{
				insuree.HFID = (short)tblInsuree.Hfid;
			}
			if (tblInsuree.IsOffline != null)
			{
				insuree.IsOffline = (bool)tblInsuree.IsOffline;
			}
			if (tblInsuree.CurrentVillage != null)
			{
				insuree.CurrentVillage = new LocationModel()
				{
					LocationId = TypeCast.GetValue<int>(tblInsuree.CurrentVillage) // todo: is there any link missing?	
				};
			}

			//if (tblInsuree.Gender != null) {
			//	insuree.Gender = tblInsuree.Gender[0];
			//}
			//if (tblInsuree.Marital != null)
			//{
			//	insuree.Marital = tblInsuree.Marital[0];
			//}
			//if (tblInsuree.TypeOfId!= null)
			//{
			//	insuree.TypeOfId = tblInsuree.TypeOfId[0];
			//}
			if (tblInsuree.Photo != null)
			{
				insuree.Photo = PhotoModel.FromTblPhoto(tblInsuree.Photo);
			}
			return insuree;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// TODO: solve the TypeOf assign
		public TblInsuree ToTblInsuree()
		{
			TblInsuree tblInsuree = new TblInsuree()
			{
				InsureeId = this.InsureeId,
				Passport = this.IdentificationNumber,
				Chfid = this.CHFID,
				LastName = this.LastName,
				OtherNames = this.OtherNames,
				Dob = this.DOB,
				IsHead = this.IsHead,
				Phone = this.Phone,
				Gender = this.Gender,
				Marital = this.Marital,
				TypeOfId = this.TypeOfId,
				CardIssued = this.CardIssued,
				Relationship = this.Relationship,
				Profession = this.Profession,
				Education = this.Education,
				Email = this.Email,
				Hfid = this.HFID,
				CurrentAddress = this.CurrentAddress,
				GeoLocation = this.GeoLocation,
				//CurrentVillage = this.CurrentVillage == null ? DBNull.Value : this.CurrentVillage.LocationId, // todo: is there any link missing?
				//Photo = this.Photo.ToTblPhoto(),
				//TypeOf = this.TypeOf.IdentificationTypes,
				IsOffline = this.IsOffline,
			};
				
			if (this.CurrentVillage != null) {
				tblInsuree.CurrentVillage = this.CurrentVillage.LocationId; // todo: is there any link missing?
			}

			if (this.Photo != null)
			{
				tblInsuree.Photo = this.Photo.ToTblPhoto();
			}

			return tblInsuree;
		}


	}
}
