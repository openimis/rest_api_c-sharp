using OpenImis.Modules.Utils;
using OpenImis.DB.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.InsureeManagementModule.Models
{
    public class FamilyModel
    {
		public int FamilyId { get; set; }
		public IEnumerable<InsureeModel> Insurees { get; set; }
		public int LocationId { get; set; } /// TODO: set to Location type
		public bool Poverty { get; set; }
		public string FamilyType { get; set; }
		public string FamilyAddress { get; set; }
		public string Ethnicity { get; set; }
		public string ConfirmationNo { get; set; }
		public string ConfirmationType { get; set; }
		public bool IsOffline { get; set; }


		public FamilyModel()
		{
			this.Insurees = new List<InsureeModel>();
		}

		//public FamilyModel(TblFamilies tblFamilies):this()
		//{
		//	this.ConvertFromTblFamilies(tblFamilies);
		//}

		public static FamilyModel FromTblFamilies(TblFamilies tblFamilies)
		{
			FamilyModel familyModel = new FamilyModel()
			{
				FamilyId = tblFamilies.FamilyId,
				LocationId = TypeCast.GetValue<int>(tblFamilies.LocationId),
				Poverty = TypeCast.GetValue<bool>(tblFamilies.Poverty),
				FamilyType = tblFamilies.FamilyType,
				FamilyAddress = tblFamilies.FamilyAddress,
				Ethnicity = tblFamilies.Ethnicity,
				ConfirmationNo = tblFamilies.ConfirmationNo,
				ConfirmationType = tblFamilies.ConfirmationType,
				IsOffline = TypeCast.GetValue<bool>(tblFamilies.IsOffline),
				Insurees = tblFamilies.TblInsuree
						.Where(i => i.ValidityTo == null)
						.Select(i => InsureeModel.FromTblInsuree(i))
						.ToList()
			};
			return familyModel;
		}

		public TblFamilies ToTblFamilies()
		{
			TblFamilies tblFamilies = new TblFamilies()
			{
				FamilyId = this.FamilyId,
				LocationId =this.LocationId,
				Poverty = this.Poverty,
				FamilyType = this.FamilyType,
				FamilyAddress = this.FamilyAddress,
				Ethnicity = this.Ethnicity,
				ConfirmationNo = this.ConfirmationNo,
				ConfirmationType = this.ConfirmationType,
				IsOffline = this.IsOffline,
				TblInsuree = this.Insurees
						//.Where(i => i.ValidTo == null)
						.Select(i => i.ToTblInsuree())
						.ToList(),
				
		};
			tblFamilies.InsureeId = tblFamilies.TblInsuree
						.Where(i => i.ValidityTo == null && i.IsHead)
						.Select(i => i.InsureeId)
						.FirstOrDefault();
			return tblFamilies;
		}
    }
}
