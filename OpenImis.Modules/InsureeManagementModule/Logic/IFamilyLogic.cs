using OpenImis.Modules.InsureeManagementModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.InsureeManagementModule.Logic
{
    public interface IFamilyLogic
    {

		Task<FamilyModel> GetFamilyByInsureeId(string insureeId);

		Task<FamilyModel[]> GetAllFamilies(int page = 1, int numberPerPage = 20);

		Task AddFamily(FamilyModel family);
	}
}
