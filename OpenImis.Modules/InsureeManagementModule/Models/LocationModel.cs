using OpenImis.DB.SqlServer;
using OpenImis.Modules.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.InsureeManagementModule.Models
{
	public class LocationModel
	{
		public int LocationId { get; set; }
		public string LocationCode { get; set; }
		public string LocationName { get; set; }
		public LocationModel ParentLocation { get; set; }
		public string LocationType { get; set; } /// TODO: this is char or string?
		public DateTime ValidFrom { get; set; }
		public DateTime ValidTo { get; set; }
		public int LegacyId { get; set; }
		//public int AuditUserId { get; set; }
		//public DateTime RowId { get; set; }
		public int MalePopulation { get; set; }
		public int FemalePopulation { get; set; }
		public int OtherPopulation { get; set; }
		public int Families { get; set; }


		public static LocationModel FromTblLocation(TblLocations tblLocation)
		{
			LocationModel locationModel = new LocationModel()
			{
				LocationId = tblLocation.LocationId,
				LocationCode = tblLocation.LocationCode,
				LocationName = tblLocation.LocationName,
				LocationType = tblLocation.LocationType,
				ValidFrom = TypeCast.GetValue<DateTime>(tblLocation.ValidityFrom),
				ValidTo = TypeCast.GetValue<DateTime>(tblLocation.ValidityTo),
				MalePopulation = TypeCast.GetValue<int>(tblLocation.MalePopulation),
				FemalePopulation = TypeCast.GetValue<int>(tblLocation.FemalePopulation),
				OtherPopulation = TypeCast.GetValue<int>(tblLocation.OtherPopulation),
				Families = TypeCast.GetValue<int>(tblLocation.Families)
			};
			return locationModel;
		}


	}
}
