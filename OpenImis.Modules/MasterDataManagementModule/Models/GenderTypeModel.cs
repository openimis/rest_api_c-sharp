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
	/// TODO: change the table structure to have GenderTypeCode, GenderType and language (EN, FR, etc.) 
	///			=> more than 2 languages 
	public class GenderTypeModel
	{
		public string GenderTypeCode { get; set; }
		public string GenderType { get; set; }

		public static GenderTypeModel FromTblGender(TblGender tblGender)
		{
			GenderTypeModel genderType = new GenderTypeModel()
			{
				GenderType = tblGender.Gender,
				GenderTypeCode = tblGender.Code
			};

			return genderType;
		}

	}
}
