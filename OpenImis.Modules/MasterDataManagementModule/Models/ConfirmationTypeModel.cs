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
	/// TODO: change the table structure to have ConfirmationTypeCode, ConfirmationType and language (EN, FR, etc.) 
	///			=> more than 2 languages 
	public class ConfirmationTypeModel
	{
		public string ConfirmationTypeCode { get; set; }
		public string ConfirmationType { get; set; }

		public static ConfirmationTypeModel FromTblConfirmationType(TblConfirmationTypes tblConfirmationType)
		{
			ConfirmationTypeModel confirmationType = new ConfirmationTypeModel()
			{
				ConfirmationType = tblConfirmationType.ConfirmationType,
				ConfirmationTypeCode = tblConfirmationType.ConfirmationTypeCode
			};

			return confirmationType;
		}
	}
}
