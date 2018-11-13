using OpenImis.Modules.Utils;
using OpenImis.DB.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.InsureeManagementModule.Models
{
    public class InsureeModel
    {
		public int InsureeId { get; set; }
		public string IdentificationNumber { get; set; }
		public string CHFID { get; set; }
		public string LastName { get; set; }
		public string OtherNames { get; set; }
		public string DOB { get; set; }
		public char Gender { get; set; }
		public char Marital { get; set; }
		public bool IsHead { get; set; }
		public string Phone { get; set; }
		public bool CardIssued { get; set; }
		public int Relationship { get; set; }
		public int Profession { get; set; }
		public int Education { get; set; }
		public string Email { get; set; }
		public char TypeOfId { get; set; }
		public int HFID { get; set; }
		public string CurrentAddress { get; set; }
		public string GeoLocation { get; set; }
		public string CurVillage { get; set; }
		public string PhotoPath { get; set; }
		public string IdentificationTypes { get; set; }
		public bool IsOffline { get; set; }

		public InsureeModel()
		{

		}

		public InsureeModel(TblInsuree tblInsuree):base()
		{
			this.ConvertFromTblInsuree(tblInsuree);	
		}

		public InsureeModel ConvertFromTblInsuree(TblInsuree tblInsuree)
		{
			this.InsureeId = tblInsuree.InsureeId;
			this.IdentificationNumber = tblInsuree.Passport;
			this.CHFID = tblInsuree.Chfid;
			this.LastName = tblInsuree.LastName;
			this.OtherNames = tblInsuree.OtherNames;
			this.DOB = tblInsuree.Dob.ToShortDateString();
			this.Gender = tblInsuree.Gender == null ? ' ' : tblInsuree.Gender[0];
			this.Marital = tblInsuree.Marital == null ? ' ' : tblInsuree.Marital[0];
			this.IsHead = tblInsuree.IsHead;
			this.Phone = tblInsuree.Phone;
			this.CardIssued = tblInsuree.CardIssued;
			this.Relationship = TypeCast.GetValue<int>(tblInsuree.Relationship);
			this.Profession = TypeCast.GetValue<int>(tblInsuree.Profession);
			this.Education = TypeCast.GetValue<int>(tblInsuree.Education);
			this.Email = tblInsuree.Email;
			this.TypeOfId = tblInsuree.TypeOfId == null ? ' ' : tblInsuree.TypeOfId[0];
			this.HFID = TypeCast.GetValue<short>(tblInsuree.Hfid);
			this.CurrentAddress = tblInsuree.CurrentAddress;
			this.GeoLocation = tblInsuree.GeoLocation;
			this.CurVillage = TypeCast.GetValue<string>(tblInsuree.CurrentVillage); // todo: is there any link missing?
			this.PhotoPath = tblInsuree.Photo.PhotoFileName;
			this.IdentificationTypes = tblInsuree.TypeOf.IdentificationTypes;
			this.IsOffline = TypeCast.GetValue<bool>(tblInsuree.IsOffline);

			return this;
		}


}
}
