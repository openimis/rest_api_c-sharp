using OpenImis.DB.SqlServer;
using OpenImis.ModulesV3.Utils;
using System;
using System.Collections.Generic;

namespace OpenImis.ModulesV3.MasterDataModule.Models
{
	public class LocationModel
	{
		public int LocationId { get; set; }
		public string LocationCode { get; set; }
		public string LocationName { get; set; }
		public int? ParentLocationId { get; set; }
		public string LocationType { get; set; }
	}
}
