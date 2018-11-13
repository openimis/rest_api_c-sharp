using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.InsureeManagementModule.Validators
{
    public interface IValidator
    {
		void Validate(Object input);

	}
}
