using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.WSModule.Validators
{
    public class ChfValidators:IWSValidators
    {
		public IValidator GetInsureeNumberValidator(IValidator validator)
		{
			return new ChfInsureeNumberValidator(validator);
		}
	}
}
