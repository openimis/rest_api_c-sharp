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
                return ValidationResult.Success;

            return new ValidationResult("Insuree Number can not be Empty.");
        }
    }
}
