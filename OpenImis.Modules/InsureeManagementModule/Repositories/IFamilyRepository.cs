using OpenImis.Modules.InsureeManagementModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.InsureeManagementModule.Repositories
{
    /// <summary>
    /// This interface serves to define the methods which must be implemented in any specific instance 
    /// </summary>
    public interface IFamilyRepository
    {

		Task<FamilyModel> GetFamilyByInsureeId(string insureeId);

		Task<FamilyModel[]> GetFamilies(int page = 1, int resultsPerPage = 20);

		Task<int> GetFamiliesCount();

		Task AddNewFamily(FamilyModel family);

	}
}
