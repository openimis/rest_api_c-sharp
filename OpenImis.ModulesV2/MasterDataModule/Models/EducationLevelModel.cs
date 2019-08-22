using OpenImis.DB.SqlServer;

namespace OpenImis.ModulesV2.MasterDataModule.Models
{
	public class EducationLevelModel
	{
		public int EducationId { get; set; }
		public string Education { get; set; }
        public string SortOrder { get; set; }
        public string AltLanguage { get; set; }
	}
}
