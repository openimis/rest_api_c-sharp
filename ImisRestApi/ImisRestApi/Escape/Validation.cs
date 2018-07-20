using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Escape
{
    public static class Validation
    {
        public static ValidationResult InsureeNumber(string insureeNumber)
        {
            return ValidationResult.Success;
        }
    }
}
