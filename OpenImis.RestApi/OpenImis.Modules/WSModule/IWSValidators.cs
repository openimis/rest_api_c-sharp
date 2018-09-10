using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenImis.Modules.WSModule.Validators;

namespace OpenImis.Modules.WSModule
{
    public interface IWSValidators
    {
		IValidator GetInsureeNumberValidator(IValidator validator);
	}
}
