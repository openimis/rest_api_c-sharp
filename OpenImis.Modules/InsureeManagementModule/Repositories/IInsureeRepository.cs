using OpenImis.Modules.InsureeManagementModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.InsureeManagementModule.Repositories
{
    public interface IInsureeRepository
    {

		Task<InsureeModel> GetInsureeByCHFIDAsync(string chfId);
		Task<IEnumerable<InsureeModel>> GetInsureeByFamilyIdAsync(int familyId);

	}
}
