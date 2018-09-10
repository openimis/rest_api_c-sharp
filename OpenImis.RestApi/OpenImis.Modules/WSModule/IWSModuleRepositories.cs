using OpenImis.Modules.WSModule.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.WSModule
{
    public interface IWSModuleRepositories
    {
		IFamilyRepository GetFamilyRepository();
		IInsureeRepository GetInsureeRepository();
	}
}
