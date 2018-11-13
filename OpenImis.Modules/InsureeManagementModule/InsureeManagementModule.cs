using OpenImis.Modules.InsureeManagementModule.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.InsureeManagementModule
{
    public class WSModule:IInsureeManagementModule
    {
		private IInsureeLogic _insureeLogic;
		private IFamilyLogic _familyLogic;

		public WSModule()
		{

		}

		public IInsureeLogic GetInsureeLogic()
		{
			if (_insureeLogic == null)
			{
				_insureeLogic = new InsureeLogic();
			}
			return _insureeLogic;
		}

		public IInsureeManagementModule SetInsureeLogic(IInsureeLogic insureeLogic)
		{
			_insureeLogic = insureeLogic;
			return this;
		}

		public IFamilyLogic GetFamilyLogic()
		{
			if (_familyLogic == null)
			{
				_familyLogic = new FamilyLogic();
			}
			return _familyLogic;
		}

		public IInsureeManagementModule SetFamilyLogic(IFamilyLogic familyLogic)
		{
			_familyLogic = familyLogic;
			return this;
		}
	}
}
