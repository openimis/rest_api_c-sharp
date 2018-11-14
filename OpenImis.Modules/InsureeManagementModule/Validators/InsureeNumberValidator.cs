using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.InsureeManagementModule.Validators
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

			string insuranceNumber = (string)input;

			if (insuranceNumber.Length == 0)
			{
				throw new ValidationException("The insuree number is missing");
			}

			if (!this.IsValidInsuranceNumber(insuranceNumber))
			{
				throw new ValidationException("The insuree number is not valid");
			}


			//if (!isValid) throw new ValidationException();
		}

		private bool IsValidInsuranceNumber(string insuranceNumber)
		{
			if (insuranceNumber.Length > 12) return false;

			return true;			
		}

	}
}
