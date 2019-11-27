using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace OpenImis.ModulesV1.Helpers.Validators
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class ValidDateAttribute : ValidationAttribute
    {
        public ValidDateAttribute()
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            try
            {
                DateTime date = Convert.ToDateTime(value.ToString());
                DateTime Odate;

                if (date.Year > 1753)
                {
                    if (DateTime.TryParseExact(value.ToString(), "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out Odate))
                    {
                        return ValidationResult.Success;
                    }
                    else
                    {
                        return new ValidationResult(null);
                    }
                }
                else
                {
                    return new ValidationResult(null);
                }
            }
            catch (Exception)
            {
                return new ValidationResult(null);
            }
        }
    }
}
