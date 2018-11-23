using OpenImis.Modules.InsureeManagementModule.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace OpenImis.Modules.InsureeManagementModule.Validators
{
	public class NotEmptyFamilyValidator : IValidator
	{
		protected IValidator validator;

		public NotEmptyFamilyValidator(IValidator validator)
		{
			this.validator = validator;
		}

		public void Validate(Object input)
		{
			if (validator != null)
			{
				validator.Validate(input);
			}

			FamilyModel family = (FamilyModel)input;

			if (!family.Insurees.Any())
			{
				throw new ValidationException("The Family must have at least one Insuree");
			}
		}
	}
}
