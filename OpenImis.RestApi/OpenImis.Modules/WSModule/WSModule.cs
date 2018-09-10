using OpenImis.Modules.WSModule.Controllers;
using OpenImis.Modules.WSModule.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.WSModule
{
    public class WSModule:IWSModule
    {
		private IInsureeController _insureeController;
		private IFamilyController _familyController;

		private IWSModuleRepositories _repositories;
		protected IWSValidators _validators;

		public WSModule(IWSModuleRepositories repositories, IWSValidators validators)
		{
			_repositories = repositories;
			_validators = validators;
		}

		private IWSModuleRepositories GetRepositories()
		{
			if (_repositories == null)
			{
				_repositories = new WSModuleRepositories();
			}
			return _repositories;
		}

		private IWSValidators GetValidators()
		{
			if (_validators == null)
			{
				//_validators = new Validators.WSValidators();
			}
			return _validators;
		}

		public IInsureeController GetInsureeController()
		{
			if (_insureeController == null)
			{
				_insureeController = new InsureeController(GetRepositories());
			}
			return _insureeController;
		}

		public IFamilyController GetFamilyController()
		{
			if (_familyController == null)
			{
				_familyController = new FamilyController(GetRepositories(), GetValidators());
			}
			return _familyController;
		}

    }
}
