using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace ImisRestApi.ImisAttributes
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

                if(date.Year > 1753)
                {
                    return ValidationResult.Success;
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
