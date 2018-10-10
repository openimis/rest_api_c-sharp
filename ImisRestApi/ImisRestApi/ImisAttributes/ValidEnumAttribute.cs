using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.ImisAttributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class ValidEnumAttribute : ValidationAttribute
    {
        public Type _type;

        public ValidEnumAttribute(Type type)
        {
            _type = type;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                Convert.ChangeType(value,_type);
                return ValidationResult.Success;
            }
            catch (Exception)
            {

                return new ValidationResult("Error");
            }


        }
    }

}
