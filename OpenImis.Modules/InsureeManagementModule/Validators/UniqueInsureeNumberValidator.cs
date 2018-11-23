using OpenImis.Modules.InsureeManagementModule.Logic;
using OpenImis.Modules.InsureeManagementModule.Models;
using OpenImis.Modules.InsureeManagementModule.Models.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace OpenImis.Modules.InsureeManagementModule.Validators
{
	public class UniqueInsureeNumberValidator
	{

		private readonly IValidator validator;
		private readonly IInsureeLogic insureeLogic;

		public UniqueInsureeNumberValidator(IInsureeLogic insureeLogic,IValidator validator)
		{
			this.validator = validator;
			this.insureeLogic = insureeLogic;
		}

		public async Task ValidateAsync(Object input)
		{
			if (validator != null)
			{
				validator.Validate(input);
			}

			string insureeNumber = (string)input;

			if (insureeNumber.Length == 0)
			{
				throw new ValidationException(InsureeErrors.MISSING_INSUREE_NUMBER_ERROR);
			}

			InsureeModel insuree = await this.insureeLogic.GetInsureeByInsureeIdAsync(insureeNumber);

			if (insuree != null)
			{
				throw new ValidationException(InsureeErrors.INSUREE_NUMBER_EXISTS_ERROR, null, insureeNumber);
			}

		}

	}
}
