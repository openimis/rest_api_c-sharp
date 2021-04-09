using OpenImis.DB.SqlServer;

namespace OpenImis.Modules.MasterDataModule.Models
{
	public class ConfirmationTypeModel
	{
		public string ConfirmationTypeCode { get; set; }
		public string ConfirmationType { get; set; }
        public string SortOrder { get; set; }
        public string AltLanguage { get; set; }
	}
}
