using OpenImis.DB.SqlServer;

namespace OpenImis.Modules.MasterDataModule.Models
{
	public class GenderTypeModel
	{
		public string Code { get; set; }
		public string Gender { get; set; }
        public string AltLanguage { get; set; }
        public string SortOrder { get; set; }
	}
}
