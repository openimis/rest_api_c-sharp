using Microsoft.EntityFrameworkCore;
using OpenImis.DB.SqlServer;
using OpenImis.Modules.MasterDataManagementModule.Models;
using OpenImis.Modules.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenImis.Modules.MasterDataManagementModule.Repositories
{
	public class LocationRepository : ILocationRepository
	{

		public async Task<LocationModel[]> GetAllLocations()
		{
			LocationModel[] locations;

			using (var imisContext = new ImisDB())
			{

				TblLocations[] allLocations = await imisContext.TblLocations
														.Where(l => l.ValidityTo == null)
														.Select(l => l)
														.ToArrayAsync();

				locations = imisContext.TblLocations
								  .Where(l => l.ParentLocationId == null)
								  .Select(l => new LocationModel()
								  {
									  LocationId = l.LocationId,
									  LocationCode = l.LocationCode,
									  LocationName = l.LocationName,
									  LocationType = l.LocationType,
									  ValidFrom = l.ValidityFrom,
									  ValidTo = l.ValidityTo,
									  MalePopulation = TypeCast.GetValue<int>(l.MalePopulation),
									  FemalePopulation = TypeCast.GetValue<int>(l.FemalePopulation),
									  OtherPopulation = TypeCast.GetValue<int>(l.OtherPopulation),
									  Families = TypeCast.GetValue<int>(l.Families),
									  Locations = GetChildLocations(allLocations, l.LocationId)
								  })
								  .ToArray();


				if (locations == null)
				{
					return null;
				}
			}

			return locations;
		}

		private static List<LocationModel> GetChildLocations(TblLocations[] allLocations, int parentId)
		{
			using (var imisContext = new ImisDB())
			{
				return allLocations
					.Where(l => l.ParentLocationId == parentId)
					.Select(l => new LocationModel()
					{
						LocationId = l.LocationId,
						LocationCode = l.LocationCode,
						LocationName = l.LocationName,
						LocationType = l.LocationType,
						ValidFrom = l.ValidityFrom,
						ValidTo = l.ValidityTo,
						MalePopulation = TypeCast.GetValue<int>(l.MalePopulation),
						FemalePopulation = TypeCast.GetValue<int>(l.FemalePopulation),
						OtherPopulation = TypeCast.GetValue<int>(l.OtherPopulation),
						Families = TypeCast.GetValue<int>(l.Families),
						Locations = GetChildLocations(allLocations, l.LocationId)
					})
					.ToList();
			}
		}
	}
}
