using OpenImis.Modules.WSModule.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.WSModule
{
    public class WSValidators:IWSValidators
    {
		public IValidator GetInsureeNumberValidator(IValidator validator)
		{
			return new InsureeNumberValidator(validator);
		}
    }
}
