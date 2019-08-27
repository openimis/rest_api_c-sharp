using OpenImis.DB.SqlServer;

namespace OpenImis.ModulesV2.MasterDataModule.Models
{
	public class FamilyTypeModel
	{
		public string FamilyTypeCode { get; set; }
		public string FamilyType { get; set; }
        public string SortOrder { get; set; }
        public string AltLanguage { get; set; }
	}
}
