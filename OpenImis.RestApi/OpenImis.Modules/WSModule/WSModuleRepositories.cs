using OpenImis.Modules.WSModule.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.WSModule
{
    public class WSModuleRepositories:IWSModuleRepositories
	{
		private IFamilyRepository _familyRepository;
		private IInsureeRepository _insureeRepository;

		public WSModuleRepositories()
		{

		}

		public IFamilyRepository GetFamilyRepository()
		{
			if (_familyRepository == null)
			{
				_familyRepository = new FamilyRepository();
			}
			return _familyRepository;
		}

		public IInsureeRepository GetInsureeRepository()
		{
			if (_insureeRepository == null)
			{
				_insureeRepository = new InsureeRepository();
			}
			return _insureeRepository;
		}

    }
}
