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
            try
            {
                Convert.ToDateTime(value.ToString());
                return ValidationResult.Success;
            }
            catch (Exception)
            {

                return new ValidationResult("6:Wrong format or missing birth date");
            }
           

        }
    }
}
