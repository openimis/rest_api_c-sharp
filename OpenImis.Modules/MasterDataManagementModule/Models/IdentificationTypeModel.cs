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
	/// TODO: change the table structure to have IdentificationTypeCode, IdentificationType and language (EN, FR, etc.) 
	///			=> more than 2 languages 
	public class IdentificationTypeModel
	{
		public string IdentificationTypeCode { get; set; }
		public string IdentificationType { get; set; }

		public static IdentificationTypeModel FromTblIdentificationTypes(TblIdentificationTypes tblIdentificationType)
		{
			IdentificationTypeModel identificationType = new IdentificationTypeModel()
			{
				IdentificationType = tblIdentificationType.IdentificationTypes,
				IdentificationTypeCode = tblIdentificationType.IdentificationCode
			};

			return identificationType;
		}

	}
}
