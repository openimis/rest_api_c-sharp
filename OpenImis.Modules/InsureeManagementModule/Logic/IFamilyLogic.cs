using OpenImis.Modules.InsureeManagementModule.Models;
using OpenImis.Modules.InsureeManagementModule.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.InsureeManagementModule.Logic
{
    public interface IFamilyLogic
    {

		Task<FamilyModel> GetFamilyByInsureeId(string insureeId);

		Task<GetFamiliesResponse> GetFamilies(int page = 1, int resultsPerPage = 20);

		Task AddFamily(FamilyModel family);
	}
}
