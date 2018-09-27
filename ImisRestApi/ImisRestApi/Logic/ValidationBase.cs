using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Logic
{
    public class ValidationBase
    {
        public virtual ValidationResult InsureeNumber(string insureeNumber)
        {
            if (insureeNumber != null)
            {
                if (insureeNumber.Length < 12 && insureeNumber.Length > 0)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("001:Wrong format of insurance number ");
            }

            return new ValidationResult("001:Wrong format of insurance number ");
        }

        public virtual ValidationResult OfficerCode(object value)
        {
            if (value != null)
            {
                if (value.ToString().Length < 8)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("003:Not valid enrolment officer code");
            }
            return ValidationResult.Success;
        }
    }
}
