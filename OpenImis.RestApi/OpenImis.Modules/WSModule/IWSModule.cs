using OpenImis.Modules.WSModule.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.WSModule
{
    public interface IWSModule
    {

		IInsureeController GetInsureeController();
		IWSModule SetInsureeController(IInsureeController insureeController);
		IFamilyController GetFamilyController();
		IWSModule SetFamilyController(IFamilyController familyController);
	}
}
