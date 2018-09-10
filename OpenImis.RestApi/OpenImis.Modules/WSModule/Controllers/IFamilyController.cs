using OpenImis.Modules.WSModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.WSModule.Controllers
{
    public interface IFamilyController
    {

		Task<FamilyModel> GetFamily(string chfId);

	}
}
