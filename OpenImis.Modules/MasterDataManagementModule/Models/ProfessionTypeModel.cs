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
	/// TODO: change the table structure to have ProfessionTypeId, ProfessionType and language (EN, FR, etc.) 
	///			=> more than 2 languages 
	public class ProfessionTypeModel
	{
		public int ProfessionTypeId { get; set; }
		public string ProfessionType { get; set; }

		public static ProfessionTypeModel FromTblProfessions(TblProfessions tblProfessions)
		{
			ProfessionTypeModel professionType = new ProfessionTypeModel()
			{
				ProfessionType = tblProfessions.Profession,
				ProfessionTypeId = tblProfessions.ProfessionId
			};

			return professionType;
		}

	}
}
