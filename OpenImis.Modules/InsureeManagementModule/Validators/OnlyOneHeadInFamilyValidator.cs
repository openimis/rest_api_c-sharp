using OpenImis.Modules.InsureeManagementModule.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace OpenImis.Modules.InsureeManagementModule.Validators
{
	public class OnlyOneHeadInFamilyValidator : IValidator
	{
		protected IValidator validator;

		public OnlyOneHeadInFamilyValidator(IValidator validator)
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
			int familyHeadCount = family.Insurees.Count(i => i.IsHead);
			if (familyHeadCount == 0)
			{
				throw new ValidationException("The Family must have a head Insuree");
			}
			if (familyHeadCount > 1)
			{
				throw new ValidationException("The Family has more than one head Insuree");
			}
		}
	}
}
