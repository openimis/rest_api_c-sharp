using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.Modules.WSModule.Validators
{
	public class ChfInsureeNumberValidator : IValidator
	{
		protected IValidator validator;

		public ChfInsureeNumberValidator(IValidator validator)
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
			if (insuranceNumber.Length != 9) return false;

			string chfid;
			
			int part1 = int.Parse(insuranceNumber) / 10;
			int part2 = part1 % 7;

			chfid = insuranceNumber.Substring(0, 8) + part2.ToString();

			return insuranceNumber.Equals(chfid);
		}

	}
}
