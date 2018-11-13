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

		Task<FamilyModel[]> GetAllFamilies(int page = 1, int numberPerPage = 0);

	}
}
