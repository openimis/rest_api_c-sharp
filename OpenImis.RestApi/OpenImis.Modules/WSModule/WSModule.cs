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

		public WSModule()
		{

		}

		public IInsureeController GetInsureeController()
		{
			if (_insureeController == null)
			{
				_insureeController = new InsureeController();
			}
			return _insureeController;
		}

		public IWSModule SetInsureeController(IInsureeController insureeController)
		{
			_insureeController = insureeController;
			return this;
		}

		public IFamilyController GetFamilyController()
		{
			if (_familyController == null)
			{
				_familyController = new FamilyController();
			}
			return _familyController;
		}

		public IWSModule SetFamilyController(IFamilyController familyController)
		{
			_familyController = familyController;
			return this;
		}
	}
}
