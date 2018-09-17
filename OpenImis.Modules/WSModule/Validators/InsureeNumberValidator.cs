using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.WSModule.Validators
{
	public class InsureeNumberValidator : IValidator
	{
		protected IValidator validator;

		public InsureeNumberValidator(IValidator validator)
		{
			this.validator = validator;
		}

		public void Validate(Object input)
		{
			if (validator != null)
			{
				validator.Validate(input);
			}

		}
	}
}
