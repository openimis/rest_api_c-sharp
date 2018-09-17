using OpenImis.Modules.Utils;
using OpenImis.DB.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.WSModule.Models
{
    public class FamilyModel
    {
		public int FamilyId { get; set; }
		public IEnumerable<InsureeModel> Insurees { get; set; }
		public int LocationId { get; set; }
		public bool Poverty { get; set; }
		public char FamilyType { get; set; }
		public string FamilyAddress { get; set; }
		public string Ethnicity { get; set; }
		public string ConfirmationNo { get; set; }
		public string ConfirmationType { get; set; }
		public bool IsOffline { get; set; }


		public FamilyModel()
		{
			this.Insurees = new List<InsureeModel>();
		}

		public FamilyModel(TblFamilies tblFamilies):this()
		{
			this.ConvertFromTblFamilies(tblFamilies);
		}

		public void ConvertFromTblFamilies(TblFamilies tblFamilies) 
		{
			this.FamilyId = tblFamilies.FamilyId;
			this.LocationId = TypeCast.GetValue<int>(tblFamilies.LocationId);
			this.Poverty = TypeCast.GetValue<bool>(tblFamilies.Poverty);
			this.FamilyType = tblFamilies.FamilyType == null ? ' ' : tblFamilies.FamilyType[0];
			this.FamilyAddress = tblFamilies.FamilyAddress;
			this.Ethnicity = tblFamilies.Ethnicity;
			this.ConfirmationNo = tblFamilies.ConfirmationNo;
			this.ConfirmationType = tblFamilies.ConfirmationType;
			this.IsOffline = TypeCast.GetValue<bool>(tblFamilies.IsOffline);

		}
    }
}
