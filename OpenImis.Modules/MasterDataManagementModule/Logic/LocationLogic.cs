using OpenImis.Modules.MasterDataManagementModule.Models;
using OpenImis.Modules.MasterDataManagementModule.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenImis.Modules.MasterDataManagementModule.Logic
{
	public class LocationLogic : ILocationLogic
	{
		protected IImisModules imisModules;
		protected ILocationRepository locationRepository;

		public LocationLogic()
		{

		}

		public LocationLogic(IImisModules imisModules)
		{
			this.locationRepository = new LocationRepository();
			this.imisModules = imisModules;
		}

		public async Task<LocationModel[]> GetAllLocations()
		{
			// Authorize user

			// Validate input

			// Execute business behaviour
			LocationModel[] locations;

			locations = await locationRepository.GetAllLocations();

			// Validate results

			// Validate data access rights

			// Return results 
			return locations;
		}

	}
}
