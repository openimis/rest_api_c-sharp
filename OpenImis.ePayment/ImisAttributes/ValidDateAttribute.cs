using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;


namespace OpenImis.ePayment.ImisAttributes
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
                    if (DateTime.TryParseExact(value.ToString(), "yyyy/MM/dd", CultureInfo.InvariantCulture,DateTimeStyles.None, out Odate))
                    {
                        return ValidationResult.Success;
                    }
                    else if (DateTime.TryParseExact(value.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out Odate))
                    {
                        return ValidationResult.Success;
                    }
                    else if (DateTime.TryParseExact(value.ToString(), "yyyy-M-d", CultureInfo.InvariantCulture, DateTimeStyles.None, out Odate))
                    {
                        return ValidationResult.Success;
                    }
                    else if (DateTime.TryParseExact(value.ToString(), "yyyy/M/d", CultureInfo.InvariantCulture, DateTimeStyles.None, out Odate))
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
