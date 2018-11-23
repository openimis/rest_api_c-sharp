using OpenImis.Modules.MasterDataManagementModule.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenImis.Modules.MasterDataManagementModule.Logic
{
	public interface ILocationLogic
	{
		Task<LocationModel[]> GetAllLocations();
	}
}
