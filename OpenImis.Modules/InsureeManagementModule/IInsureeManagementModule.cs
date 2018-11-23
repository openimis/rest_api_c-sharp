using OpenImis.Modules.InsureeManagementModule.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.InsureeManagementModule
{
    public interface IInsureeManagementModule
    {

		IInsureeLogic GetInsureeLogic();
		IInsureeManagementModule SetInsureeLogic(IInsureeLogic insureeLogic);
		IFamilyLogic GetFamilyLogic();
		IInsureeManagementModule SetFamilyLogic(IFamilyLogic familyLogic);
	}
}
