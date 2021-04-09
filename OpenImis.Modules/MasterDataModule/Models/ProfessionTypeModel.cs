using OpenImis.DB.SqlServer;

namespace OpenImis.Modules.MasterDataModule.Models
{
	public class ProfessionTypeModel
	{
		public int ProfessionId { get; set; }
		public string Profession { get; set; }
        public string SortOrder { get; set; }
        public string AltLanguage { get; set; }
	}
}
