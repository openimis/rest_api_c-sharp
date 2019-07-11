using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OpenImis.Modules.Helpers.Validators
{
    public class Validation
    {
        public virtual ValidationResult InsureeNumber(string insureeNumber)
        {
            if (insureeNumber != null)
            {
                if (insureeNumber.Length < 12 && insureeNumber.Length > 0)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("1:Wrong format of insurance number ");
            }

            return ValidationResult.Success;
        }

        public virtual ValidationResult OfficerCode(object value)
        {
            if (value != null)
            {
                if (value.ToString().Length < 8)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("3:Not valid enrolment officer code");
            }

            return ValidationResult.Success;
        }
    }
}
