using OpenImis.DB.SqlServer;

namespace OpenImis.Modules.MasterDataModule.Models
{
	public class IdentificationTypeModel
	{
		public string IdentificationCode { get; set; }
		public string IdentificationTypes { get; set; }
        public string SortOrder { get; set; }
        public string AltLanguage { get; set; }
	}
}
