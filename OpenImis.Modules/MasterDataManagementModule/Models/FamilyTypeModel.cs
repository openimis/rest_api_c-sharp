using OpenImis.DB.SqlServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.MasterDataManagementModule.Models
{
	/// <summary>
	/// 
	/// </summary>
	/// TODO: add alternative language field
	/// TODO: change the table structure to have FamilyTypeCode, FamilyType and language (EN, FR, etc.) 
	///			=> more than 2 languages 
	public class FamilyTypeModel
	{
		public string FamilyTypeCode { get; set; }
		public string FamilyType { get; set; }

		public static FamilyTypeModel FromTblFamilyType(TblFamilyTypes tblFamilyType)
		{
			FamilyTypeModel familyType = new FamilyTypeModel()
			{
				FamilyType = tblFamilyType.FamilyType,
				FamilyTypeCode = tblFamilyType.FamilyTypeCode
			};

			return familyType;
		}


	}
}
