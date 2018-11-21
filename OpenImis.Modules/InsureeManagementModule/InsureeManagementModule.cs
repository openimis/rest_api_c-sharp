using OpenImis.Modules.InsureeManagementModule.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.InsureeManagementModule
{
    public class InsureeManagementModule : IInsureeManagementModule
    {
		private IInsureeLogic _insureeLogic;
		private IFamilyLogic _familyLogic;
		protected readonly IImisModules imisModules;

		public InsureeManagementModule(IImisModules imisModules)
		{
			this.imisModules = imisModules;
		}

		public IInsureeLogic GetInsureeLogic()
		{
			if (_insureeLogic == null)
			{
				_insureeLogic = new InsureeLogic(this.imisModules);
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
				_familyLogic = new FamilyLogic(this.imisModules);
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
