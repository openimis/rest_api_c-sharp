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
	/// TODO: change the table structure to have EducationLevelId, EducationLevel and language (EN, FR, etc.) 
	///			=> more than 2 languages 
	public class EducationLevelModel
	{
		public int EducationLevelId { get; set; }
		public string EducationLevel { get; set; }

		public static EducationLevelModel FromTblEducations(TblEducations tblEducations)
		{
			EducationLevelModel educationType = new EducationLevelModel()
			{
				EducationLevel = tblEducations.Education,
				EducationLevelId = tblEducations.EducationId
			};

			return educationType;
		}
	}
}
