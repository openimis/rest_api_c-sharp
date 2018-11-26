using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Logic
{
    public class Validation:ValidationBase
    {
        public override ValidationResult InsureeNumber(string insureeNumber)
        {
            if (insureeNumber.Length != 9)
                return new ValidationResult("Invalid CHF ID");
            String Chfid;
            int Part1, Part2;
            Part1 = int.Parse(insureeNumber.ToString()) / 10;
            Part2 = Part1 % 7;

            Chfid = insureeNumber.Substring(0, 8) + Part2.ToString();
            if (insureeNumber.Equals(Chfid))
            {
                return ValidationResult.Success;

            }
            else{
                return new ValidationResult("Invalid CHF ID");
            }
        }
    }
}
