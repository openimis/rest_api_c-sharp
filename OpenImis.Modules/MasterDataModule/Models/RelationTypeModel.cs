using OpenImis.DB.SqlServer;

namespace OpenImis.Modules.MasterDataModule.Models
{
	public class RelationTypeModel
	{
		public int RelationId { get; set; }
		public string Relation { get; set; }
        public string SortOrder { get; set; }
        public string AltLanguage { get; set; }
	}
}
